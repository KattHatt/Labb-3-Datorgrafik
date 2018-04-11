using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Labb2_Datorgrafik.Managers;
using Labb2_Datorgrafik.Components;

namespace Labb2_Datorgrafik.Systems
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
                cam.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(cam.FieldOfView), cam.AspectRatio, cam.NearPlaneDistance, cam.FarPlaneDistance);
                cam.View = Matrix.CreateLookAt(transform.Position, transform.Position + transform.Rotation, transform.Up);

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

        private void Pitch(CameraComponent cam, TransformComponent transform, float angle)
        {
            Matrix rotation = Matrix.CreateFromAxisAngle(Vector3.Cross(transform.Rotation, transform.Up), MathHelper.ToRadians(angle));
            transform.Rotation = Vector3.Transform(transform.Rotation, rotation);
            transform.Up = Vector3.Transform(transform.Up, rotation);
            cam.View = Matrix.CreateLookAt(transform.Position, transform.Position + transform.Rotation, transform.Up);
        }

        private void Yaw(CameraComponent cam, TransformComponent transform, float angle)
        {
            Matrix rotation = Matrix.CreateFromAxisAngle(transform.Up, MathHelper.ToRadians(angle));
            transform.Rotation = Vector3.Transform(transform.Rotation, rotation);
            cam.View = Matrix.CreateLookAt(transform.Position, transform.Position + transform.Rotation, transform.Up);
        }
    }
}
