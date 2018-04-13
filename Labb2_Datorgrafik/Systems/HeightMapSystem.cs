using Labb2_Datorgrafik.Components;
using Labb2_Datorgrafik.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Labb2_Datorgrafik.Systems
{
    class HeightMapSystem : IRender
    {
        ComponentManager cm = ComponentManager.GetInstance();

        public void Render(GraphicsDevice gd, BasicEffect be)
        {
            foreach (var entity in cm.GetComponentsOfType<HeightMapComponent>())
            {
                HeightMapComponent hmc = (HeightMapComponent)entity.Value;
                be.CurrentTechnique.Passes[0].Apply();
                for (int i = 0; i < hmc.VertexBuffers.Length; i++)
                {
                    gd.SetVertexBuffer(hmc.VertexBuffers[i]);
                    gd.Indices = hmc.IndexBuffers[i];
                    gd.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, hmc.IndexBuffers[i].IndexCount / 3);
                }
            }
        }

        public void Load(ContentManager content)
        {
            foreach(var entity in cm.GetComponentsOfType<HeightMapComponent>())
            {
                HeightMapComponent hmc = (HeightMapComponent)entity.Value;
                List<VertexBuffer> vertexBuffers = new List<VertexBuffer>();
                List<IndexBuffer> indexBuffers = new List<IndexBuffer>();

                hmc.HeightMap = content.Load<Texture2D>(hmc.HeightMapFilePath);
                hmc.Width = hmc.HeightMap.Width;
                hmc.Height = hmc.HeightMap.Height;
                
                foreach (var heights in Split(hmc))
                {
                    VertexPositionColor[] vertices = CreateVertices(heights.Value, heights.Key);
                    int[] indices = CreateIndicesAndUpdateColor(vertices, heights.Value.GetLength(0), heights.Value.GetLength(1));

                    VertexBuffer vertexBuffer = new VertexBuffer(hmc.GraphicsDevice, VertexPositionColor.VertexDeclaration, heights.Value.Length, BufferUsage.WriteOnly);
                    vertexBuffer.SetData(vertices);
                    vertexBuffers.Add(vertexBuffer);
                    IndexBuffer indexBuffer = new IndexBuffer(hmc.GraphicsDevice, IndexElementSize.ThirtyTwoBits, heights.Value.Length * 6, BufferUsage.WriteOnly);
                    indexBuffer.SetData(indices);
                    indexBuffers.Add(indexBuffer);
                }

                hmc.VertexBuffers = vertexBuffers.ToArray();
                hmc.IndexBuffers = indexBuffers.ToArray();
            }
        }

        private Dictionary<Vector3, float[,]> Split(HeightMapComponent hmc)
        {
            Dictionary<Vector3, float[,]> cells = new Dictionary<Vector3, float[,]>();
            Color[] heightMapData = new Color[hmc.Width * hmc.Height];
            hmc.HeightMap.GetData(heightMapData);

            int rows = hmc.Height / 256;
            int cols = hmc.Width / 256;

            for (int z = 0; z < rows; z++)
            {
                for (int x = 0; x < cols; x++)
                {
                    int x2 = MathHelper.Clamp((x + 1) * 256 + 1, 0, hmc.Width);
                    int z2 = MathHelper.Clamp((z + 1) * 256 + 1, 0, hmc.Height);
                    cells.Add(new Vector3(x * 256, 0, z * 256), getCellHeights(x * 256, x2, z * 256, z2));
                }
            }

            return cells;

            float[,] getCellHeights(int x1, int x2, int y1, int y2)
            {
                float[,] heights = new float[x2 - x1, y2 - y1];

                for (int y = y1; y < y2; y++)
                {
                    for (int x = x1; x < x2; x++)
                    {
                        heights[x - x1, y - y1] = heightMapData[y * hmc.Width + x].R;
                    }
                }

                return heights;
            }
        }

        private Color ToColor(VertexPositionColor v1, VertexPositionColor v2, VertexPositionColor v3)
        {
            Vector3 normal = Vector3.Cross(v1.Position - v2.Position, v1.Position - v3.Position);
            normal.Normalize();

            float dot = Vector3.Dot(normal, Vector3.Up);
            Vector3 gray = Color.Gray.ToVector3();
            Vector3 green = Color.Green.ToVector3();
            dot = Math.Abs(dot);
            Vector3 color = Vector3.Lerp(gray, green, dot);

            return new Color(color);
        }

        private VertexPositionColor[] CreateVertices(float[,] heights, Vector3 offset)
        {
            VertexPositionColor[] vertices = new VertexPositionColor[heights.Length];

            for (int z = 0; z < heights.GetLength(1); z++)
            {
                for (int x = 0; x < heights.GetLength(0); x++)
                {
                    vertices[z * heights.GetLength(0) + x] = new VertexPositionColor(new Vector3(x - 500, heights[x, z], z - 500) + offset, Color.White);
                }
            }

            return vertices;
        }

        private int[] CreateIndicesAndUpdateColor(VertexPositionColor[] vertices, int width, int height)
        {
            List<int> indices = new List<int>();
            int q1, q2, q3, q4;

            for (int z = 0; z < height - 1; z++)
            {
                for (int x = 0; x < width - 1; x++)
                {
                    // Calculate the four corners
                    q1 = z * width + x;
                    q2 = z * width + x + 1;
                    q3 = (z + 1) * width + x;
                    q4 = (z + 1) * width + x + 1;
                    // Add indices
                    indices.AddRange(new[]
                    {
                        q1, q2, q3, // First triangle
                        q2, q4, q3, // Second triangle
                    });

                    Color color = ToColor(vertices[q1], vertices[q2], vertices[q3]);
                    vertices[q1].Color = vertices[q2].Color = vertices[q3].Color = vertices[q4].Color = color;
                }
            }

            return indices.ToArray();
        }
    }
}
