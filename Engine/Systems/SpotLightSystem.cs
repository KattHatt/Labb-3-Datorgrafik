using Engine.Components;
using Engine.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace Engine.Systems
{
    public class SpotLightSystem : IRender, ILoad
    {       
        ComponentManager cm = ComponentManager.GetInstance();

        public SpotLightSystem()
        {
        }

        public void Load(ContentManager content)
        {
            foreach (var (k, spot) in cm.GetComponentsOfType<SpotLightComponent>())
            {
                spot.Effect = content.Load<Effect>(spot.EffectName);
                InitVertices(spot);
            }
        }

        public void Render(GraphicsDevice gd)
        {
            CameraComponent cam = cm.GetComponentsOfType<CameraComponent>().First().Item2;

            foreach(var (k, spot) in cm.GetComponentsOfType<SpotLightComponent>())
            {
                TransformComponent tc = cm.GetComponentForEntity<TransformComponent>(k);

                if (tc != null && cam != null)
                {
                    spot.Effect.CurrentTechnique = spot.Effect.Techniques["SpotLight"];
                    spot.Effect.Parameters["xWorld"].SetValue(tc.World);
                    spot.Effect.Parameters["xView"].SetValue(cam.View);
                    spot.Effect.Parameters["xProjection"].SetValue(cam.Projection);

                    spot.Effect.Parameters["xAmbient"].SetValue(spot.Ambient);
                    spot.Effect.Parameters["xLightPosition"].SetValue(tc.Position);
                    spot.Effect.Parameters["xConeDirection"].SetValue(spot.ConeDirection);
                    spot.Effect.Parameters["xConeAngle"].SetValue(spot.Angle);
                    spot.Effect.Parameters["xConeDecay"].SetValue(spot.ConeDecay);
                    spot.Effect.Parameters["xLightStrength"].SetValue(spot.LightStrength);

                    foreach (EffectPass pass in spot.Effect.CurrentTechnique.Passes)
                    {
                        pass.Apply();
                        gd.DrawUserPrimitives(PrimitiveType.TriangleStrip, spot.VertexPNT, 0, 6);
                    }
                }
            }
        }

        private void InitVertices(SpotLightComponent s)
        {
            s.VertexPNT = new VertexPositionNormalTexture[8];

            s.VertexPNT[0] = new VertexPositionNormalTexture(new Vector3(0, 0, 0), new Vector3(0, 1, 0), new Vector2(0, 1));
            s.VertexPNT[1] = new VertexPositionNormalTexture(new Vector3(0, 0, -30), new Vector3(0, 1, 0), new Vector2(0, 0));

            s.VertexPNT[2] = new VertexPositionNormalTexture(new Vector3(10, 0, 0), new Vector3(0, 1, 0), new Vector2(1, 1));
            s.VertexPNT[3] = new VertexPositionNormalTexture(new Vector3(10, 0, -30), new Vector3(0, 1, 0), new Vector2(1, 0));

            s.VertexPNT[4] = new VertexPositionNormalTexture(new Vector3(10, 0, 0), new Vector3(-1, 0, 0), new Vector2(0, 1));
            s.VertexPNT[5] = new VertexPositionNormalTexture(new Vector3(10, 0, -30), new Vector3(-1, 0, 0), new Vector2(0, 0));

            s.VertexPNT[6] = new VertexPositionNormalTexture(new Vector3(10, 10, 0), new Vector3(-1, 0, 0), new Vector2(1, 1));
            s.VertexPNT[7] = new VertexPositionNormalTexture(new Vector3(10, 10, -30), new Vector3(-1, 0, 0), new Vector2(1, 0));

            
        }
    }
}
