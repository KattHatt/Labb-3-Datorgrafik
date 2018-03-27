using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Labb1_Datorgrafik.Components;
using Labb1_Datorgrafik.Managers;

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
            throw new NotImplementedException();
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

            }
        }
        public void SetHeights(HeightMapComponent hmc)
        {
            Color[] greyValues = new Color[hmc.Width * hmc.Height];
            hmc.HeightMap.GetData(greyValues);
            hmc.HeightMapData = new float[hmc.Width, hmc.Height];
            for (int x = 0; x < hmc.Width; x++)
            {
                for (int y = 0; y < hmc.Height; y++)
                {
                    hmc.HeightMapData[x, y] = greyValues[x + y * hmc.Width].G / 3.1f;
                }
            }
        }

        public void SetIndices(HeightMapComponent hmc)
        {
            // amount of triangles
            hmc.Indices = new int[6 * (hmc.Width - 1) * (hmc.Height - 1)];
            int number = 0;
            // collect data for corners
            for (int y = 0; y < hmc.Height - 1; y++)
                for (int x = 0; x < hmc.Width - 1; x++)
                {
                    // create double triangles
                    hmc.Indices[number] = x + (y + 1) * hmc.Width;      // up left
                    hmc.Indices[number + 1] = x + y * hmc.Width + 1;        // down right
                    hmc.Indices[number + 2] = x + y * hmc.Width;            // down left
                    hmc.Indices[number + 3] = x + (y + 1) * hmc.Width;      // up left
                    hmc.Indices[number + 4] = x + (y + 1) * hmc.Width + 1;  // up right
                    hmc.Indices[number + 5] = x + y * hmc.Width + 1;        // down right
                    number += 6;
                }
        }

        public void SetVertices(HeightMapComponent hmc)
        {
            hmc.Vertices = new VertexPositionTexture[hmc.Width * hmc.Height];
            Vector2 texturePosition;
            for (int x = 0; x < hmc.Width; x++)
            {
                for (int y = 0; y < hmc.Height; y++)
                {
                    texturePosition = new Vector2((float)x / 25.5f, (float)y / 25.5f);
                    hmc.Vertices[x + y * hmc.Width] = new VertexPositionTexture(new Vector3(x, hmc.HeightMapData[x, y], -y), texturePosition);
                }
            }
        }



        public BasicEffect SetEffects(HeightMapComponent hmc, GraphicsDevice gd)
        {
            BasicEffect basicEffect = new BasicEffect(gd);
            basicEffect.Texture = hmc.texture;
            basicEffect.TextureEnabled = true;

            return basicEffect;
        }
    }
}
