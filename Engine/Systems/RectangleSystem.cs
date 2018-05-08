using Engine.Components;
using Engine.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Systems
{
    public class RectangleSystem : IRender, ILoad
    {
        ComponentManager cm = ComponentManager.GetInstance();

        Effect ef1;
        Effect ef2;
        Effect ef3;

        public void Load(ContentManager content)
        {
            foreach (var (id, rect) in cm.GetComponentsOfType<RectangleComponent>())
            {
                SetupVertices(rect);
                SetupIndices(rect);
                rect.vertexBuffer = new VertexBuffer(rect.graphicsDevice, typeof(VertexPositionNormalTexture), rect.vertices.Length, BufferUsage.WriteOnly);
                rect.vertexBuffer.SetData(rect.vertices);
                rect.indexBuffers = new IndexBuffer(rect.graphicsDevice, typeof(short), rect.indices.Length, BufferUsage.WriteOnly);
                rect.indexBuffers.SetData(rect.indices);


                rect.Texture = content.Load<Texture2D>(rect.TexturePath);

            }

            ef1 = content.Load<Effect>("ShaderXXX");
            ef2 = content.Load<Effect>("Fog");
            ef3 = content.Load<Effect>("Ambient");

        }

        public void Render(GraphicsDevice gd)
        {
            ComponentManager cm = ComponentManager.GetInstance();
            CameraComponent cam = cm.GetComponentsOfType<CameraComponent>().First().Item2;

            foreach (var (_, rect, tc) in cm.GetComponentsOfType<RectangleComponent, TransformComponent>())
            {
                // Ambient shader
                ef3.CurrentTechnique = ef1.Techniques["Ambient"];
                ef3.Parameters["World"].SetValue(tc.World);
                ef3.Parameters["View"].SetValue(cam.View);
                ef3.Parameters["Projection"].SetValue(cam.Projection);

                foreach (EffectPass pass in ef1.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    gd.DrawUserPrimitives(PrimitiveType.TriangleList, rect.vertices, 0, rect.vertices.Length / 3);
                }

                // VertexShader (ShaderXXX)
                ef1.CurrentTechnique = ef1.Techniques["VertexShading"];
                ef1.Parameters["xWorld"].SetValue(tc.World);
                ef1.Parameters["xView"].SetValue(cam.View);
                ef1.Parameters["xProjection"].SetValue(cam.Projection);
                ef1.Parameters["xLightDirection"].SetValue(new Vector3(1, 0, 0));

                foreach (EffectPass pass in ef1.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    gd.DrawUserPrimitives(PrimitiveType.TriangleList, rect.vertices, 0, rect.vertices.Length / 3);
                }

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

        private void UpdateNormals(RectangleComponent r)
        {
            //generate normals
            for (int i = 0; i < r.indices.Length / 3; i++)
            {
                Vector3 firstvec = r.vertices[r.indices[i * 3 + 1]].Position - r.vertices[r.indices[i * 3]].Position;
                Vector3 secondvec = r.vertices[r.indices[i * 3]].Position - r.vertices[r.indices[i * 3 + 2]].Position;
                Vector3 normal = Vector3.Cross(firstvec, secondvec);
                normal.Normalize();
                r.vertices[r.indices[i * 3]].Normal += normal;
                r.vertices[r.indices[i * 3 + 1]].Normal += normal;
                r.vertices[r.indices[i * 3 + 2]].Normal += normal;
            }

            // normalize normals
            for (int i = 0; i < r.vertices.Length; i++)
                r.vertices[i].Normal.Normalize();
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
