using Engine.Components;
using Engine.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Systems
{
    public class BoxSystem : IRender, ILoad
    {
        ComponentManager cm = ComponentManager.GetInstance();

        Effect ef1;
        Effect ef2;
        Effect ef3;
        Effect ef4;

        public void Load(ContentManager content)
        {
            foreach (var (id, rect) in cm.GetComponentsOfType<BoxComponent>())
            {
                SetupVertices(rect);
                rect.VertexBuffer = new VertexBuffer(rect.GraphicsDevice, typeof(VertexPositionNormalTexture), rect.Vertices.Length, BufferUsage.WriteOnly);
                rect.VertexBuffer.SetData(rect.Vertices);

                rect.Texture = content.Load<Texture2D>(rect.TexturePath);
            }

            ef4 = content.Load<Effect>("specularTex");
            ef1 = content.Load<Effect>("ShaderXXX");
            ef2 = content.Load<Effect>("Fog");
            ef3 = content.Load<Effect>("Ambient");

        }

        public void Render(GraphicsDevice gd)
        {
            ComponentManager cm = ComponentManager.GetInstance();
            CameraComponent cam = cm.GetComponentsOfType<CameraComponent>().First().Item2;

            foreach (var (_, rect, tc) in cm.GetComponentsOfType<BoxComponent, TransformComponent>())
            {

                Matrix wit = Matrix.Transpose(Matrix.Invert(tc.World));

                // VertexShader (ShaderXXX)
                ef1.CurrentTechnique = ef1.Techniques["VertexShading"];
                ef1.Parameters["xWorld"].SetValue(tc.World);
                ef1.Parameters["xView"].SetValue(cam.View);
                ef1.Parameters["xProjection"].SetValue(cam.Projection);
                ef1.Parameters["xLightPosition"].SetValue(new Vector3(1, 5, 0));

                /*foreach (EffectPass pass in ef1.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    gd.DrawUserPrimitives(PrimitiveType.TriangleList, rect.vertices, 0, rect.vertices.Length / 3);
                }*/

                // Fog shader
                //ef2.CurrentTechnique = ef2.Techniques["Fog"];
                //ef2.Parameters["World"].SetValue(tc.World);
                //ef2.Parameters["View"].SetValue(cam.View);
                //ef2.Parameters["Projection"].SetValue(cam.Projection);
                //ef2.Parameters["FogEnabled"].SetValue(1.0f);
                //ef2.Parameters["FogStart"].SetValue(0.0f);
                //ef2.Parameters["FogEnd"].SetValue(0.4f);
                //ef2.Parameters["FogColor"].SetValue(Color.CornflowerBlue.ToVector3());
                //ef2.Parameters["cameraPos"].SetValue(cam.Position);

                //foreach (EffectPass pass in ef2.CurrentTechnique.Passes)
                //{
                //    pass.Apply();
                //    gd.DrawUserPrimitives(PrimitiveType.TriangleList, rect.vertices, 0, rect.vertices.Length / 3);
                //}

                // Ambient shader
                //ef3.CurrentTechnique = ef3.Techniques["Ambient"];
                //ef3.Parameters["World"].SetValue(tc.World);
                //ef3.Parameters["View"].SetValue(cam.View);
                //ef3.Parameters["Projection"].SetValue(cam.Projection);

                //foreach (EffectPass pass in ef3.CurrentTechnique.Passes)
                //{
                //    pass.Apply();
                //    gd.DrawUserPrimitives(PrimitiveType.TriangleList, rect.vertices, 0, rect.vertices.Length / 3);
                //}


                // SpecularTex
                Vector3 cameraLocation = 20 * new Vector3((float)Math.Sin(0), 0, (float)Math.Cos(0));
                Vector3 cameraTarget = new Vector3(0, 0, 0);
                Vector3 viewVector = Vector3.Transform(cameraTarget - cameraLocation, Matrix.CreateRotationY(0));
                viewVector.Normalize();

                ef4.CurrentTechnique = ef4.Techniques["Textured"];
                ef4.Parameters["World"].SetValue(tc.World);
                ef4.Parameters["View"].SetValue(cam.View);
                ef4.Parameters["Projection"].SetValue(cam.Projection);
                ef4.Parameters["ViewVector"].SetValue(viewVector);

                ef4.Parameters["WorldInverseTranspose"].SetValue(wit);
                //ef4.Parameters["ModelTexture"].SetValue(rect.Texture);

                foreach (EffectPass pass in ef4.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    gd.DrawUserPrimitives(PrimitiveType.TriangleList, rect.Vertices, 0, rect.Vertices.Length / 3);
                }
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
