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
    public class DirLightSystem : ILoad, ISystem
    {       
        ComponentManager cm = ComponentManager.GetInstance();

        public void Load(ContentManager content)
        {
            foreach (var (k, spot) in cm.GetComponentsOfType<DirLightComponent>())
            {
                spot.Effect = content.Load<Effect>(spot.EffectName);
                spot.Texture = content.Load<Texture2D>(spot.TextureName);
            }
        }

        public void Update(GameTime gameTime)
        {
            float time = (float)gameTime.TotalGameTime.TotalMilliseconds / 1000.0f;
            CameraComponent cam = cm.GetComponentsOfType<CameraComponent>().First().Item2;
           
            foreach (var (k, dl) in cm.GetComponentsOfType<DirLightComponent>())
            {
                // Teststuff
                if (Keyboard.GetState().IsKeyDown(Keys.P))
                {
                    dl.LightPower += 0.1f;
                    Console.WriteLine(dl.LightPower);
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.O))
                {
                    dl.LightPower -= 0.1f;
                    Console.WriteLine(dl.LightPower);
                }
                if (Keyboard.GetState().IsKeyDown(Keys.L))
                {
                    dl.AmbientPower += 0.1f;
                    Console.WriteLine(dl.AmbientPower);
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.K))
                {
                    dl.AmbientPower -= 0.1f;
                    Console.WriteLine(dl.AmbientPower);
                }

                // Effect update
                dl.Effect.CurrentTechnique = dl.Effect.Techniques["DirLight"];

                dl.Effect.Parameters["xWorldViewProjection"].SetValue(cam.View * cam.Projection);
                dl.Effect.Parameters["xWorld"].SetValue(Matrix.Identity);
                dl.Effect.Parameters["xLightPos"].SetValue(dl.LightPos);

                dl.Effect.Parameters["xLightPower"].SetValue(dl.LightPower);
                dl.Effect.Parameters["xAmbient"].SetValue(dl.AmbientPower);
                dl.Effect.Parameters["xTexture"].SetValue(dl.Texture);
            }
        }
      
    }
}
