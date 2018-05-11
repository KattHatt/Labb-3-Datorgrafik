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
                spot.Key = k;
            }
        }

        public void Update(GameTime gameTime)
        {
            float time = (float)gameTime.TotalGameTime.TotalMilliseconds / 1000.0f;
            CameraComponent cam = cm.GetComponentsOfType<CameraComponent>().First().Item2;
           
            foreach (var (k, spot, trans) in cm.GetComponentsOfType<SpotLightComponent, TransformComponent>())
            {
                // Teststuff
                if (Keyboard.GetState().IsKeyDown(Keys.P))
                {
                    spot.LightStrength += 0.1f;
                    Console.WriteLine(spot.LightStrength);
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.O))
                {
                    spot.LightStrength -= 0.1f;
                    Console.WriteLine(spot.LightStrength);
                }
                if (Keyboard.GetState().IsKeyDown(Keys.L))
                {
                    spot.ConeDecay += 0.1f;
                    Console.WriteLine(spot.ConeDecay);
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.K))
                {
                    spot.ConeDecay -= 0.1f;
                    Console.WriteLine(spot.ConeDecay);
                }


                //spot.LightStrength = (float)Math.Sin(time) * 8.0f;

                // Effect update
                spot.Effect.CurrentTechnique = spot.Effect.Techniques["SpotLight"];

                spot.Effect.Parameters["xWorld"].SetValue(Matrix.Identity);
                spot.Effect.Parameters["xView"].SetValue(cam.View);
                spot.Effect.Parameters["xProjection"].SetValue(cam.Projection);

                spot.Effect.Parameters["xAmbient"].SetValue(spot.Ambient);
                spot.Effect.Parameters["xLightPosition"].SetValue(trans.Position);
                spot.Effect.Parameters["xConeDirection"].SetValue(spot.ConeDirection);
                spot.Effect.Parameters["xConeAngle"].SetValue(spot.Angle);
                spot.Effect.Parameters["xConeDecay"].SetValue(spot.ConeDecay);
                spot.Effect.Parameters["xLightStrength"].SetValue(spot.LightStrength);
            }
        }
      
    }
}
