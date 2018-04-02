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

                CreateVertices(hmc);
                CreateIndices(hmc);
            }
        }

        void CreateVertices(HeightMapComponent hmc)
        {
            hmc.vertexBuffer = new VertexBuffer(hmc.graphicsDevice, VertexPositionColor.VertexDeclaration, hmc.Width * hmc.Height, BufferUsage.WriteOnly);
            VertexPositionColor[] vertices = new VertexPositionColor[hmc.Width * hmc.Height];
            Color[] data = new Color[hmc.Width * hmc.Height];
            hmc.HeightMap.GetData(data);
            Random rand = new Random(0);

            for (int x = 0; x < hmc.Width; x++)
            {
                for (int y = 0; y < hmc.Height; y++)
                {
                    float z = 1 + data[y * hmc.Width + x].R / 255f;
                    Color color = new Color(rand.Next(255), rand.Next(255), rand.Next(255));
                    vertices[y * hmc.Width + x] = new VertexPositionColor(new Vector3(x - 500, z, y - 500), color);
                }
            }

            hmc.vertexBuffer.SetData(vertices);
        }

        void CreateIndices(HeightMapComponent hmc)
        {
            List<int> indices = new List<int>();
            hmc.indexBuffer = new IndexBuffer(hmc.graphicsDevice, IndexElementSize.ThirtyTwoBits, hmc.Width * hmc.Height * 6, BufferUsage.WriteOnly);

            int q1, q2, q3, q4;        
            for (int y = 0; y < hmc.Height - 1; y++)
            {
                for (int x = 0; x < hmc.Width - 1; x++)
                {
                    // Calculate the four corners
                    q1 = y * hmc.Width + x;
                    q2 = y * hmc.Width + x + 1;
                    q3 = (y + 1) * hmc.Width + x;
                    q4 = (y + 1) * hmc.Width + x + 1;
                    // Add indices
                    indices.AddRange(new[] {
                    q1, q2, q3, // First triangle
                    q2, q4, q3, // Second triangle
                });
                }
            }

            hmc.indexBuffer.SetData(indices.ToArray());
        }
    }
}
