using Engine.Components;
using Engine.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Systems
{
    public class BoxSystem : IRender, ILoad
    {
        ComponentManager cm = ComponentManager.GetInstance();

        public void Load(ContentManager content)
        {
            foreach (var (id, rect) in cm.GetComponentsOfType<BoxComponent>())
            {
                SetupVertices(rect);
                rect.VertexBuffer = new VertexBuffer(rect.GraphicsDevice, typeof(VertexPositionNormalTexture), rect.Vertices.Length, BufferUsage.WriteOnly);
                rect.VertexBuffer.SetData(rect.Vertices);

                rect.Texture = content.Load<Texture2D>(rect.TexturePath);
            }
        }

        public void Render(GraphicsDevice gd)
        {
            // TODO?
        }

        public void RenderShadow(GraphicsDevice gd, Effect e)
        {
            CameraComponent camera = cm.GetComponentsOfType<CameraComponent>().First().Item2;

            e.Parameters["LightView"].SetValue(camera.View);
            e.Parameters["LightProjection"].SetValue(camera.Projection);
            e.Parameters["FogStart"].SetValue(100f);
            e.Parameters["FogEnd"].SetValue(1000f);
            e.Parameters["FogColor"].SetValue(Color.CornflowerBlue.ToVector3());
            e.Parameters["EyePosition"].SetValue(camera.Position);
            e.Parameters["LightDirection"].SetValue(new Vector3(-0.5265408f, -0.5735765f, -0.6275069f));

            e.Parameters["AmbientColor"].SetValue(Vector3.Zero);
            e.Parameters["DiffuseColor"].SetValue(Vector3.One * 0.2f);
            e.Parameters["SpecularColor"].SetValue(Vector3.One);
            e.Parameters["SpecularPower"].SetValue(120f);
            e.Parameters["DiffuseIntensity"].SetValue(1f);
            e.Parameters["SpecularIntensity"].SetValue(1f);

            foreach (var (_, box, transform) in cm.GetComponentsOfType<BoxComponent, TransformComponent>())
            {
                e.Parameters["Texture"].SetValue(box.Texture);
                e.Parameters["World"].SetValue(transform.World);
                e.Techniques["Render"].Passes[0].Apply();
                gd.DrawUserPrimitives(PrimitiveType.TriangleList, box.Vertices, 0, box.Vertices.Length / 3);
            }
        }

        // Fill rectangle vertex list
        private void SetupVertices(BoxComponent r)
        {
            List<VertexPositionNormalTexture> vertexList = new List<VertexPositionNormalTexture>()
            {
                // Front face
                new VertexPositionNormalTexture(r.FRONT_TOP_LEFT, r.FORWARD, new Vector2(0, 1)),
                new VertexPositionNormalTexture(r.FRONT_BOTTOM_RIGHT, r.FORWARD, new Vector2(1, 0)),
                new VertexPositionNormalTexture(r.FRONT_BOTTOM_LEFT, r.FORWARD, new Vector2(0, 0)),
                new VertexPositionNormalTexture(r.FRONT_TOP_LEFT, r.FORWARD, new Vector2(0, 1)),
                new VertexPositionNormalTexture(r.FRONT_TOP_RIGHT, r.FORWARD, new Vector2(1, 1)),
                new VertexPositionNormalTexture(r.FRONT_BOTTOM_RIGHT, r.FORWARD, new Vector2(1, 0)),

                // Top face
                new VertexPositionNormalTexture(r.BACK_TOP_LEFT, r.UP, new Vector2(0, 1)),
                new VertexPositionNormalTexture(r.FRONT_TOP_RIGHT, r.UP, new Vector2(1, 0)),
                new VertexPositionNormalTexture(r.FRONT_TOP_LEFT, r.UP, new Vector2(0, 0)),
                new VertexPositionNormalTexture(r.BACK_TOP_LEFT, r.UP, new Vector2(0, 1)),
                new VertexPositionNormalTexture(r.BACK_TOP_RIGHT, r.UP, new Vector2(1, 1)),
                new VertexPositionNormalTexture(r.FRONT_TOP_RIGHT, r.UP, new Vector2(1, 0)),

                // Right face
                new VertexPositionNormalTexture(r.FRONT_TOP_RIGHT, r.RIGHT, new Vector2(0, 1)),
                new VertexPositionNormalTexture(r.BACK_BOTTOM_RIGHT, r.RIGHT, new Vector2(1, 0)),
                new VertexPositionNormalTexture(r.FRONT_BOTTOM_RIGHT, r.RIGHT, new Vector2(0, 0)),
                new VertexPositionNormalTexture(r.FRONT_TOP_RIGHT, r.RIGHT, new Vector2(0, 1)),
                new VertexPositionNormalTexture(r.BACK_TOP_RIGHT, r.RIGHT, new Vector2(1, 1)),
                new VertexPositionNormalTexture(r.BACK_BOTTOM_RIGHT, r.RIGHT, new Vector2(1, 0)),

                // Bottom face
                new VertexPositionNormalTexture(r.FRONT_BOTTOM_LEFT, r.DOWN, new Vector2(0, 1)),
                new VertexPositionNormalTexture(r.BACK_BOTTOM_RIGHT, r.DOWN, new Vector2(1, 0)),
                new VertexPositionNormalTexture(r.BACK_BOTTOM_LEFT, r.DOWN, new Vector2(0, 0)),
                new VertexPositionNormalTexture(r.FRONT_BOTTOM_LEFT, r.DOWN, new Vector2(0, 1)),
                new VertexPositionNormalTexture(r.FRONT_BOTTOM_RIGHT, r.DOWN, new Vector2(1, 1)),
                new VertexPositionNormalTexture(r.BACK_BOTTOM_RIGHT, r.DOWN, new Vector2(1, 0)),

                // Left face
                new VertexPositionNormalTexture(r.BACK_TOP_LEFT, r.LEFT, new Vector2(0, 1)),
                new VertexPositionNormalTexture(r.FRONT_BOTTOM_LEFT, r.LEFT, new Vector2(1, 0)),
                new VertexPositionNormalTexture(r.BACK_BOTTOM_LEFT, r.LEFT, new Vector2(0, 0)),
                new VertexPositionNormalTexture(r.BACK_TOP_LEFT, r.LEFT, new Vector2(0, 1)),
                new VertexPositionNormalTexture(r.FRONT_TOP_LEFT, r.LEFT, new Vector2(1, 1)),
                new VertexPositionNormalTexture(r.FRONT_BOTTOM_LEFT, r.LEFT, new Vector2(1, 0)),

                // Back face
                new VertexPositionNormalTexture(r.BACK_TOP_RIGHT, r.BACKWARD, new Vector2(0, 1)),
                new VertexPositionNormalTexture(r.BACK_BOTTOM_LEFT, r.BACKWARD, new Vector2(1, 0)),
                new VertexPositionNormalTexture(r.BACK_BOTTOM_RIGHT, r.BACKWARD, new Vector2(0, 0)),
                new VertexPositionNormalTexture(r.BACK_TOP_RIGHT, r.BACKWARD, new Vector2(0, 1)),
                new VertexPositionNormalTexture(r.BACK_TOP_LEFT, r.BACKWARD, new Vector2(1, 1)),
                new VertexPositionNormalTexture(r.BACK_BOTTOM_LEFT, r.BACKWARD, new Vector2(1, 0))
            };

            r.Vertices = vertexList.ToArray();
        }
    }
}
