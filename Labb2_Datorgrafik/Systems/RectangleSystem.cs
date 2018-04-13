using Labb1_Datorgrafik.Components;
using Labb2_Datorgrafik.Components;
using Labb2_Datorgrafik.Managers;
using Labb2_Datorgrafik.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Labb1_Datorgrafik.Systems
{
    public class RectangleSystem : ISystem, IRender
    {
        public void Load(ContentManager content)
        {
            ComponentManager cm = ComponentManager.GetInstance();

            foreach(var entity in cm.GetComponentsOfType<RectangleComponent>())
            {
                RectangleComponent rect = (RectangleComponent)entity.Value;

                SetupVertices(rect);

                foreach(string tp in rect.TexturePaths)
                {
                    rect.Textures.Add(content.Load<Texture2D>(tp));
                }
                SetupIndices(rect);
                SetupIndexBuffer(rect);
            }
        }

        public void Render(GraphicsDevice gd, BasicEffect be)
        {
            be.TextureEnabled = true;


            ComponentManager cm = ComponentManager.GetInstance();

            foreach (var entity in cm.GetComponentsOfType<RectangleComponent>())
            {
                RectangleComponent rect = (RectangleComponent)entity.Value;
                TransformComponent trans = cm.GetComponentForEntity<TransformComponent>(entity.Key);

                Matrix[] transforms = new Matrix[rect.Children.Count];
                // Vill ha typ "CopyAbsoluteBoneTransformsTo(transforms)" här
                // vet inte hur jag ska få tag i alla childrens transform komponenter.

                // något i stil med: 
                foreach(var child in rect.Children)
                {
                    // få tag i childens transformcomponent och lägg till i "transforms" listan ovan
                }
              


                // sedan efterlikna modelsystem och rita ut barnens och föräldramodellen. 

                // Jag kanske krånglar till det i dunno

                foreach(Texture2D texture in rect.Textures)
                {
                    be.Texture = texture;

                }

                
            }
        }

        public void Update(GameTime gametime)
        {
            throw new NotImplementedException();
        }

        // Fill rectangle vertex list
        private void SetupVertices(RectangleComponent r)
        {
            List<VertexPositionNormalTexture> vertexList = new List<VertexPositionNormalTexture>(36);

            // Front face
            vertexList.Add(new VertexPositionNormalTexture(r.FRONT_TOP_LEFT, r.FORWARD, new Vector2(0, 1)));
            vertexList.Add(new VertexPositionNormalTexture(r.FRONT_BOTTOM_RIGHT, r.FORWARD, new Vector2(1, 0)));
            vertexList.Add(new VertexPositionNormalTexture(r.FRONT_BOTTOM_LEFT, r.FORWARD, new Vector2(0, 0)));
            vertexList.Add(new VertexPositionNormalTexture(r.FRONT_TOP_LEFT, r.FORWARD, new Vector2(0, 1)));
            vertexList.Add(new VertexPositionNormalTexture(r.FRONT_TOP_RIGHT, r.FORWARD, new Vector2(1, 1)));
            vertexList.Add(new VertexPositionNormalTexture(r.FRONT_BOTTOM_RIGHT, r.FORWARD, new Vector2(1, 0)));

            // Top face
            vertexList.Add(new VertexPositionNormalTexture(r.BACK_TOP_LEFT, r.UP, new Vector2(0, 1)));
            vertexList.Add(new VertexPositionNormalTexture(r.FRONT_TOP_RIGHT, r.UP, new Vector2(1, 0)));
            vertexList.Add(new VertexPositionNormalTexture(r.FRONT_TOP_LEFT, r.UP, new Vector2(0, 0)));
            vertexList.Add(new VertexPositionNormalTexture(r.BACK_TOP_LEFT, r.UP, new Vector2(0, 1)));
            vertexList.Add(new VertexPositionNormalTexture(r.BACK_TOP_RIGHT, r.UP, new Vector2(1, 1)));
            vertexList.Add(new VertexPositionNormalTexture(r.FRONT_TOP_RIGHT, r.UP, new Vector2(1, 0)));

            // Right face
            vertexList.Add(new VertexPositionNormalTexture(r.FRONT_TOP_RIGHT, r.RIGHT, new Vector2(0, 1)));
            vertexList.Add(new VertexPositionNormalTexture(r.BACK_BOTTOM_RIGHT, r.RIGHT, new Vector2(1, 0)));
            vertexList.Add(new VertexPositionNormalTexture(r.FRONT_BOTTOM_RIGHT, r.RIGHT, new Vector2(0, 0)));
            vertexList.Add(new VertexPositionNormalTexture(r.FRONT_TOP_RIGHT, r.RIGHT, new Vector2(0, 1)));
            vertexList.Add(new VertexPositionNormalTexture(r.BACK_TOP_RIGHT, r.RIGHT, new Vector2(1, 1)));
            vertexList.Add(new VertexPositionNormalTexture(r.BACK_BOTTOM_RIGHT, r.RIGHT, new Vector2(1, 0)));

            // Bottom face
            vertexList.Add(new VertexPositionNormalTexture(r.FRONT_BOTTOM_LEFT, r.DOWN, new Vector2(0, 1)));
            vertexList.Add(new VertexPositionNormalTexture(r.BACK_BOTTOM_RIGHT, r.DOWN, new Vector2(1, 0)));
            vertexList.Add(new VertexPositionNormalTexture(r.BACK_BOTTOM_LEFT, r.DOWN, new Vector2(0, 0)));
            vertexList.Add(new VertexPositionNormalTexture(r.FRONT_BOTTOM_LEFT, r.DOWN, new Vector2(0, 1)));
            vertexList.Add(new VertexPositionNormalTexture(r.FRONT_BOTTOM_RIGHT, r.DOWN, new Vector2(1, 1)));
            vertexList.Add(new VertexPositionNormalTexture(r.BACK_BOTTOM_RIGHT, r.DOWN, new Vector2(1, 0)));

            // Left face
            vertexList.Add(new VertexPositionNormalTexture(r.BACK_TOP_LEFT, r.LEFT, new Vector2(0, 1)));
            vertexList.Add(new VertexPositionNormalTexture(r.FRONT_BOTTOM_LEFT, r.LEFT, new Vector2(1, 0)));
            vertexList.Add(new VertexPositionNormalTexture(r.BACK_BOTTOM_LEFT, r.LEFT, new Vector2(0, 0)));
            vertexList.Add(new VertexPositionNormalTexture(r.BACK_TOP_LEFT, r.LEFT, new Vector2(0, 1)));
            vertexList.Add(new VertexPositionNormalTexture(r.FRONT_TOP_LEFT, r.LEFT, new Vector2(1, 1)));
            vertexList.Add(new VertexPositionNormalTexture(r.FRONT_BOTTOM_LEFT, r.LEFT, new Vector2(1, 0)));

            // Back face
            vertexList.Add(new VertexPositionNormalTexture(r.BACK_TOP_RIGHT, r.BACKWARD, new Vector2(0, 1)));
            vertexList.Add(new VertexPositionNormalTexture(r.BACK_BOTTOM_LEFT, r.BACKWARD, new Vector2(1, 0)));
            vertexList.Add(new VertexPositionNormalTexture(r.BACK_BOTTOM_RIGHT, r.BACKWARD, new Vector2(0, 0)));
            vertexList.Add(new VertexPositionNormalTexture(r.BACK_TOP_RIGHT, r.BACKWARD, new Vector2(0, 1)));
            vertexList.Add(new VertexPositionNormalTexture(r.BACK_TOP_LEFT, r.BACKWARD, new Vector2(1, 1)));
            vertexList.Add(new VertexPositionNormalTexture(r.BACK_BOTTOM_LEFT, r.BACKWARD, new Vector2(1, 0)));

            r.vertices = vertexList.ToArray();
        }

        // Fills a rectangles index list
        private void SetupIndices(RectangleComponent r)
        {
            List<int> indexList = new List<int>(36);

            for (int i = 0; i < 36; ++i)
                indexList.Add(i);

            r.indices = indexList.ToArray();
        }

        // Set Index buffer
        private void SetupIndexBuffer(RectangleComponent r)
        {
            r.indexBuffer = new IndexBuffer(r.graphicsDevice, typeof(short), r.indices.Length, BufferUsage.None);
            r.indexBuffer.SetData(r.indices);
        }
    }
}
