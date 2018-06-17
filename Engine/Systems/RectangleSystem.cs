using Engine.Components;
using Engine.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace Engine.Systems
{
    public class RectangleSystem : IInit, IRender, ILoad
    {
        ComponentManager cm = ComponentManager.GetInstance();
        Texture2D texture;

        public void Init(GraphicsDevice gd)
        {
            foreach (var (_, rectangle) in cm.GetComponentsOfType<RectangleComponent>())
            {
                CreateVertices(gd, rectangle);
            }
        }

        public void Load(ContentManager content)
        {
            texture = content.Load<Texture2D>("grass");
        }

        public void RenderShadow(GraphicsDevice gd, Effect e)
        {
            CameraComponent camera = cm.GetComponentsOfType<CameraComponent>().First().Item2;

            e.Parameters["LightView"].SetValue(camera.View);
            e.Parameters["LightProjection"].SetValue(camera.Projection);
            e.Parameters["World"].SetValue(Matrix.Identity);
            e.Parameters["Texture"].SetValue(texture);
            e.Techniques["Render"].Passes[0].Apply();

            foreach (var (_, rectangle) in cm.GetComponentsOfType<RectangleComponent>())
            {
                gd.SetVertexBuffer(rectangle.VertexBuffer);
                gd.DrawPrimitives(PrimitiveType.TriangleList, 0, rectangle.VertexBuffer.VertexCount / 3);
            }
        }

        public void Render(GraphicsDevice gd)
        {
            // TODO?
        }

        private void CreateVertices(GraphicsDevice gd, RectangleComponent rectangle)
        {
            Vector3 TopLeft = rectangle.Center + new Vector3(-rectangle.Width / 2, 0, -rectangle.Height / 2);
            Vector3 TopRight = rectangle.Center + new Vector3(rectangle.Width / 2, 0, -rectangle.Height / 2);
            Vector3 BottomLeft = rectangle.Center + new Vector3(-rectangle.Width / 2, 0, rectangle.Height / 2);
            Vector3 BottomRight = rectangle.Center + new Vector3(rectangle.Width / 2, 0, rectangle.Height / 2);

            VertexPositionNormalTexture[] vertices =
            {
                new VertexPositionNormalTexture(BottomLeft, Vector3.Up, new Vector2(0, 0)),
                new VertexPositionNormalTexture(BottomRight, Vector3.Up, new Vector2(20, 0)),
                new VertexPositionNormalTexture(TopLeft, Vector3.Up, new Vector2(0, 20)),

                new VertexPositionNormalTexture(TopLeft, Vector3.Up, new Vector2(0, 20)),
                new VertexPositionNormalTexture(TopRight, Vector3.Up, new Vector2(20, 20)),
                new VertexPositionNormalTexture(BottomRight, Vector3.Up, new Vector2(20, 0)),
            };

            rectangle.VertexBuffer = new VertexBuffer(gd, VertexPositionNormalTexture.VertexDeclaration, vertices.Length, BufferUsage.WriteOnly);
            rectangle.VertexBuffer.SetData(vertices);
        }
    }
}
