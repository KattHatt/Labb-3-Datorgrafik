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

                cam.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(cam.FieldOfView), cam.AspectRatio, cam.NearPlaneDistance, cam.FarPlaneDistance);
                cam.View = Matrix.CreateLookAt(cam.Position, cam.Position + cam.Direction, cam.Up);

                /*if (Keyboard.GetState().IsKeyDown(Keys.Up))
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
                }*/
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

        private void Pitch(CameraComponent cam, float angle)
        {
            Matrix rotation = Matrix.CreateFromAxisAngle(Vector3.Cross(cam.Direction, cam.Up), MathHelper.ToRadians(angle));
            cam.Direction = Vector3.Transform(cam.Direction, rotation);
            cam.Up = Vector3.Transform(cam.Up, rotation);
            cam.View = Matrix.CreateLookAt(cam.Position, cam.Position + cam.Direction, cam.Up);
        }

        private void Yaw(CameraComponent cam, float angle)
        {
            Matrix rotation = Matrix.CreateFromAxisAngle(cam.Up, MathHelper.ToRadians(angle));
            cam.Direction = Vector3.Transform(cam.Direction, rotation);
            cam.View = Matrix.CreateLookAt(cam.Position, cam.Position + cam.Direction, cam.Up);
        }
    }
}
