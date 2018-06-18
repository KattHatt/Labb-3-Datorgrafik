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

        public void Render(GraphicsDevice gd, Effect e, string technique)
        {
            CameraComponent camera = cm.GetComponentsOfType<CameraComponent>().First().Item2;

            ShadowMapComponent shadow = ComponentManager.GetInstance().GetComponentsOfType<ShadowMapComponent>().First().Item2;

            e.CurrentTechnique = e.Techniques[technique];
            e.Parameters["View"].SetValue(camera.View);
            e.Parameters["Projection"].SetValue(camera.Projection);
            e.Parameters["FogStart"].SetValue(100f);
            e.Parameters["FogEnd"].SetValue(1000f);
            e.Parameters["FogColor"].SetValue(Color.CornflowerBlue.ToVector3());
            e.Parameters["EyePosition"].SetValue(camera.Position);
            e.Parameters["LightDirection"].SetValue(new Vector3(0.6f, 0.6f, 0.7f));

            //Vector3 lightDirection = e.Parameters["LightDirection"].GetValueVector3();
            //Matrix lightView = Matrix.CreateLookAt(lightDirection, lightDirection * 0.5f, Vector3.Up);
            //Matrix lightProjection = Matrix.CreateOrthographic(2048, 2048, 0, 1000);
            //e.Parameters["LightView"].SetValue(lightView);
            //e.Parameters["LightProjection"].SetValue(lightProjection);
            e.Parameters["AmbientPower"].SetValue(1f);

            e.Parameters["AmbientColor"].SetValue(Vector3.Zero);
            e.Parameters["DiffuseColor"].SetValue(Vector3.One * 0.2f);
            e.Parameters["SpecularColor"].SetValue(Vector3.One);
            e.Parameters["SpecularPower"].SetValue(120f);
            e.Parameters["DiffuseIntensity"].SetValue(1f);
            e.Parameters["SpecularIntensity"].SetValue(1f);
            e.Parameters["xShadowMap"].SetValue(shadow.Texture);
            e.Techniques[technique].Passes[0].Apply();

            foreach (var (_, rectangle) in cm.GetComponentsOfType<RectangleComponent>())
            {
                gd.SetVertexBuffer(rectangle.VertexBuffer);
                gd.DrawPrimitives(PrimitiveType.TriangleList, 0, rectangle.VertexBuffer.VertexCount / 3);
            }
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
