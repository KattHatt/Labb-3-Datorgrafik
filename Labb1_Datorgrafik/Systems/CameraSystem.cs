using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Labb1_Datorgrafik.Managers;
using Labb1_Datorgrafik.Components;

namespace Labb1_Datorgrafik.Systems
{
    class CameraSystem : ISystem, IRender
    {
        ComponentManager cm = ComponentManager.GetInstance();

        public void Update(GameTime gametime)
        {
            foreach (var entity in cm.GetComponentsOfType<CameraComponent>())
            {
                CameraComponent cam = (CameraComponent)entity.Value;
                TransformComponent transform = cm.GetComponentForEntity<TransformComponent>(entity.Key);

                if (Keyboard.GetState().IsKeyDown(Keys.Up))
                {
                    cam.Pitch(1);
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Down))
                {
                    cam.Pitch(-1);
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    cam.Yaw(1);
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    cam.Yaw(-1);
                }
            }
        }

        public void Render(GraphicsDevice graphicsDevice, BasicEffect basicEffect)
        {
            foreach (var entity in cm.GetComponentsOfType<CameraComponent>())
            {
                CameraComponent cam = (CameraComponent)entity.Value;

                basicEffect.Projection = cam.Projection;
                basicEffect.View = cam.View;
                basicEffect.World = Matrix.Identity;
            }
        }

        public void Load(ContentManager content)
        {
        }
    }
}
