using Engine.Components;
using Engine.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace Engine.Systems
{
    public class ShadowMapSystem : ILoad, ISystem
    {
        ComponentManager cm = ComponentManager.GetInstance();

        public void Load(ContentManager content)
        {
            foreach(var (k, shadow) in cm.GetComponentsOfType<ShadowMapComponent>())
            {
                shadow.Effect = content.Load<Effect>(shadow.EffectName);
                shadow.Texture = content.Load<Texture2D>(shadow.TextureName);
                shadow.Key = k;
            }
        }

        public void Update(GameTime gametime)
        {
            CameraComponent cam = cm.GetComponentsOfType<CameraComponent>().First().Item2;
            SpotLightComponent spot = cm.GetComponentsOfType<SpotLightComponent>().First().Item2;
            TransformComponent spotTrans = cm.GetComponentForEntity<TransformComponent>(spot.Key);

            foreach (var (k, shadow) in cm.GetComponentsOfType<ShadowMapComponent>())
            {


                Matrix lightsView = Matrix.CreateLookAt(spotTrans.Position, new Vector3(-2, 3, -10), new Vector3(0, 1, 0));
                Matrix lightsProjection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver2, 1f, 5f, 100f);

                shadow.Effect.CurrentTechnique = shadow.Effect.Techniques["ShadowMap"];

                shadow.Effect.Parameters["xWorldViewProjection"].SetValue(cam.View);
                shadow.Effect.Parameters["xTexture"].SetValue(shadow.Texture);
                shadow.Effect.Parameters["xWorld"].SetValue(Matrix.Identity);
                shadow.Effect.Parameters["xLightPos"].SetValue(spotTrans.Position);
                shadow.Effect.Parameters["xLightPower"].SetValue(spot.LightStrength);
                shadow.Effect.Parameters["xAmbient"].SetValue(spot.Ambient);
                shadow.Effect.Parameters["xLightsWorldViewProjection"].SetValue(lightsView * lightsProjection);


            }
        }
    }
}
