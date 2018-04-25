using Labb3_Datorgrafik.Components;
using Labb3_Datorgrafik.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Labb3_Datorgrafik.Systems
{
    public class RectangleSystem : ISystem, IRender
    {
        public void Load(ContentManager content)
        {
            ComponentManager cm = ComponentManager.GetInstance();

            foreach (var (id, rect) in cm.GetComponentsOfType<RectangleComponent>())
            {
                if(rect.Parent != null)
                {
                    RectangleComponent rectParent = cm.GetComponentForEntity<RectangleComponent>((int)rect.Parent);

                    rectParent.Children.Add(id);
                }

                SetupVertices(rect);
                SetupIndices(rect);
                rect.vertexBuffer = new VertexBuffer(rect.graphicsDevice, typeof(VertexPositionNormalTexture), rect.vertices.Length, BufferUsage.WriteOnly);
                rect.vertexBuffer.SetData(rect.vertices);
                rect.indexBuffers = new IndexBuffer(rect.graphicsDevice, typeof(short), rect.indices.Length, BufferUsage.WriteOnly);
                rect.indexBuffers.SetData(rect.indices);

                foreach (string tp in rect.TexturePaths)
                {
                    rect.Textures.Add(content.Load<Texture2D>(tp));
                }
                
            }
        }

        public void Render(GraphicsDevice gd, BasicEffect be)
        {
            ComponentManager cm = ComponentManager.GetInstance();

            foreach (var (_, rect, trans) in cm.GetComponentsOfType<RectangleComponent, TransformComponent>())
            {
                be.TextureEnabled = true;
                be.World = trans.World;
                foreach (Texture2D txt in rect.Textures)
                {
                    be.Texture = txt;
                }
                be.CurrentTechnique.Passes[0].Apply();
               

                gd.SetVertexBuffer(rect.vertexBuffer);
                gd.Indices = rect.indexBuffers;
                gd.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, rect.indexBuffers.IndexCount / 3);

            }
        }

        public void Update(GameTime gametime)
        {
            ComponentManager cm = ComponentManager.GetInstance();

            Stack<(int, RectangleComponent)> fringe = new Stack<(int, RectangleComponent)>();

            foreach (var (id, rect, trans) in cm.GetComponentsOfType<RectangleComponent, TransformComponent>())
            {                
                if (rect.Parent == null)
                {
                    fringe.Push((id, rect));

                    while (fringe.Count > 0)
                    {
                        (int, RectangleComponent) node = fringe.Pop();

                        if (node.Item2.Parent != null)
                        {
                            TransformComponent parentTrans = cm.GetComponentForEntity<TransformComponent>((int)node.Item2.Parent);
                            TransformComponent kidTrans = cm.GetComponentForEntity<TransformComponent>(node.Item1);

                            kidTrans.World *= parentTrans.World;
                        }
                        foreach (int kid in node.Item2.Children)
                        {
                            fringe.Push((kid, cm.GetComponentForEntity<RectangleComponent>(kid)));
                        }
                    }
                }
            }
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
            List<short> indexList = new List<short>(36);

            for (short i = 0; i < 36; ++i)
                indexList.Add(i);

            r.indices = indexList.ToArray();
        }
    }
}
