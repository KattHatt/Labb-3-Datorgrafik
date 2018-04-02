using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Labb1_Datorgrafik.Components;
using Labb1_Datorgrafik.Managers;
using System.Collections.Generic;

namespace Labb1_Datorgrafik.Systems
{
    class HeightMapSystem : ISystem, IRender
    {
        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime gametime)
        {
            throw new NotImplementedException();
        }

        public void Render(GraphicsDevice gd, BasicEffect be)
        {
            ComponentManager cm = ComponentManager.GetInstance();

            foreach (var entity in cm.GetComponentsOfType<HeightMapComponent>())
            {
                HeightMapComponent hmc = (HeightMapComponent)entity.Value;
                be.CurrentTechnique.Passes[0].Apply();
                
                gd.SetVertexBuffer(hmc.vertexBuffer);
                gd.Indices = hmc.indexBuffer;
                gd.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, hmc.indexBuffer.IndexCount / 3);
            }
        }

        public void Load(ContentManager content)
        {
            ComponentManager cm = ComponentManager.GetInstance();

            foreach(var entity in cm.GetComponentsOfType<HeightMapComponent>())
            {
                HeightMapComponent hmc = (HeightMapComponent)entity.Value;

                hmc.HeightMap = content.Load<Texture2D>(hmc.HeightMapFilePath);
                hmc.texture = content.Load<Texture2D>(hmc.TextureFilePath);
                hmc.Width = hmc.HeightMap.Width;
                hmc.Height = hmc.HeightMap.Height;

                VertexPositionColor[] vertices = CreateVertices(hmc);
                CreateIndices(hmc, vertices);
            }
        }

        Color AngleToColor(VertexPositionColor v1, VertexPositionColor v2, VertexPositionColor v3)
        {
            Plane plane = new Plane(v1.Position, v2.Position, v3.Position);
            Vector3 normal = plane.Normal;
            float dot = Vector3.Dot(normal, Vector3.Up);

            Vector3 green = Color.Green.ToVector3();
            Vector3 gray = Color.Gray.ToVector3();
            dot = Math.Abs(dot);
            Vector3 color = Vector3.Lerp(gray, green, dot);
            return new Color(color);
        }

        VertexPositionColor[] CreateVertices(HeightMapComponent hmc)
        {
            //hmc.vertexBuffer = new VertexBuffer(hmc.graphicsDevice, VertexPositionColor.VertexDeclaration, hmc.Width * hmc.Height, BufferUsage.WriteOnly);
            VertexPositionColor[] vertices = new VertexPositionColor[hmc.Width * hmc.Height];
            Color[] heightMapData = new Color[hmc.Width * hmc.Height];
            hmc.HeightMap.GetData(heightMapData);
            Random rand = new Random(0);

            for (int x = 0; x < hmc.Width; x++)
            {
                for (int z = 0; z < hmc.Height; z++)
                {
                    float y = heightMapData[z * hmc.Width + x].R / 1f;
                    Color color = new Color(rand.Next(255), rand.Next(255), rand.Next(255));
                    vertices[z * hmc.Width + x] = new VertexPositionColor(new Vector3(x - 500, y, z - 500), color);
                }
            }
            
            //hmc.vertexBuffer.SetData(vertices);
            return vertices;
        }

        void CreateIndices(HeightMapComponent hmc, VertexPositionColor[] vertices)
        {
            List<int> indices = new List<int>();
            hmc.indexBuffer = new IndexBuffer(hmc.graphicsDevice, IndexElementSize.ThirtyTwoBits, hmc.Width * hmc.Height * 6, BufferUsage.WriteOnly);

            int q1, q2, q3, q4;
            for (int z = 0; z < hmc.Height - 1; z++)
            {
                for (int x = 0; x < hmc.Width - 1; x++)
                {
                    // Calculate the four corners
                    q1 = z * hmc.Width + x;
                    q2 = z * hmc.Width + x + 1;
                    q3 = (z + 1) * hmc.Width + x;
                    q4 = (z + 1) * hmc.Width + x + 1;
                    // Add indices
                    indices.AddRange(new[]
                    {
                        q1, q2, q3, // First triangle
                        q2, q4, q3, // Second triangle
                    });

                    Color color = AngleToColor(vertices[q1], vertices[q2], vertices[q3]);
                    vertices[q1].Color = vertices[q2].Color = vertices[q3].Color = vertices[q4].Color = color;
                }
            }

            hmc.vertexBuffer = new VertexBuffer(hmc.graphicsDevice, VertexPositionColor.VertexDeclaration, hmc.Width * hmc.Height, BufferUsage.WriteOnly);
            hmc.vertexBuffer.SetData(vertices);

            hmc.indexBuffer.SetData(indices.ToArray());
        }
    }
}
