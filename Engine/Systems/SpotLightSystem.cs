using Engine.Components;
using Engine.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;

namespace Engine.Systems
{
    public class SpotLightSystem : ILoad, ISystem
    {       
        ComponentManager cm = ComponentManager.GetInstance();

        public void Load(ContentManager content)
        {
            foreach (var (k, spot) in cm.GetComponentsOfType<SpotLightComponent>())
            {
                spot.Effect = content.Load<Effect>(spot.EffectName);
                spot.Texture = content.Load<Texture2D>(spot.TextureName);
            }
        }

        public void Update(GameTime gameTime)
        {
            float time = (float)gameTime.TotalGameTime.TotalMilliseconds / 1000.0f;
            CameraComponent cam = cm.GetComponentsOfType<CameraComponent>().First().Item2;
           
            foreach (var (k, spot) in cm.GetComponentsOfType<SpotLightComponent>())
            {
                // Teststuff
                if (Keyboard.GetState().IsKeyDown(Keys.P))
                {
                    spot.LightPower += 0.1f;
                    Console.WriteLine(spot.LightPower);
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.O))
                {
                    spot.LightPower -= 0.1f;
                    Console.WriteLine(spot.LightPower);
                }
                if (Keyboard.GetState().IsKeyDown(Keys.L))
                {
                    spot.AmbientPower += 0.1f;
                    Console.WriteLine(spot.AmbientPower);
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.K))
                {
                    spot.AmbientPower -= 0.1f;
                    Console.WriteLine(spot.AmbientPower);
                }

                // Effect update
                spot.Effect.CurrentTechnique = spot.Effect.Techniques["SpotLight"];
                spot.Effect.Parameters["xWorldViewProjection"].SetValue(cam.View * cam.Projection);
                spot.Effect.Parameters["xTexture"].SetValue(spot.Texture);

                spot.Effect.Parameters["xWorld"].SetValue(Matrix.Identity);
                spot.Effect.Parameters["xLightPos"].SetValue(spot.LightPos);
                spot.Effect.Parameters["xLightPower"].SetValue(spot.LightPower);
                spot.Effect.Parameters["xAmbient"].SetValue(spot.AmbientPower);
            }
        }
      
    }
}
