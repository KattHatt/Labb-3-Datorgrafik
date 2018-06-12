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
            }
        }

        public void Update(GameTime gametime)
        {
            CameraComponent cam = cm.GetComponentsOfType<CameraComponent>().First().Item2;

            foreach (var (k, shadow) in cm.GetComponentsOfType<ShadowMapComponent>())
            {

                shadow.Effect.CurrentTechnique = shadow.Effect.Techniques["ShadowMap"];

                shadow.Effect.Parameters["xWorldViewProjection"].SetValue(Matrix.Identity * cam.View * cam.Projection);
                shadow.Effect.Parameters["xLightsWorldViewProjection"].SetValue(shadow.LightsView * shadow.LightsProjection);
                shadow.Effect.Parameters["xWorld"].SetValue(Matrix.Identity);
                shadow.Effect.Parameters["xLightPos"].SetValue(shadow.LightPos);
                shadow.Effect.Parameters["xLightPower"].SetValue(shadow.LightPower);
                shadow.Effect.Parameters["xAmbient"].SetValue(shadow.Ambient);
                shadow.Effect.Parameters["xTexture"].SetValue(shadow.Texture);
            }
        }
    }
}
