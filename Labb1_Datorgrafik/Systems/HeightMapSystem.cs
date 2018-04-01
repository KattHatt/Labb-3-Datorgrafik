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

                gd.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, hmc.Vertices, 0, hmc.Vertices.Length, hmc.Indices, 0, hmc.Indices.Length / 3);
                //be.Texture = hmc.texture;
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
            hmc.Vertices = new VertexPositionColor[hmc.Width * hmc.Height];
            Color[] data = new Color[hmc.Width * hmc.Height];
            hmc.HeightMap.GetData(data);

            for (int x = 0; x < hmc.Width; x++)
            {
                for (int z = 0; z < hmc.Height; z++)
                {
                    float y = data[z * hmc.Width + x].R / 3.1f;
                    hmc.Vertices[z * hmc.Width + x] = new VertexPositionColor(new Vector3(x, y, -z), Color.White);
                }
            }
        }

        void CreateIndices(HeightMapComponent hmc)
        {
            List<int> indices = new List<int>();

            /*
             * Index into the vertex list like a quadrant with a single for loop,
             * this makes the logic easier with the index creation.
             * 
             * q1       q2
             *   +-----+
             *   |     |
             *   |     |
             *   +-----+
             * q3       q4
             */

            int q1 = 0;
            int q2 = 1;
            int q3 = hmc.Width;
            int q4 = hmc.Width + 1;
            for (; q4 < hmc.Width * hmc.Height; q1++, q2++, q3++, q4++)
            {
                indices.AddRange(new[] {
                    q1, q2, q3, // First triangle
                    q2, q4, q3, // Second triangle
                });
            }

            hmc.Indices = indices.ToArray();
        }
    }
}
