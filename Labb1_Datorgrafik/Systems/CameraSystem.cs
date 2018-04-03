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
        public void Update(GameTime gametime)
        {
            ComponentManager cm = ComponentManager.GetInstance();

            foreach (var entity in cm.GetComponentsOfType<CameraComponent>())
            {
                CameraComponent cam = (CameraComponent)entity.Value;
                TransformComponent transform = cm.GetComponentForEntity<TransformComponent>(entity.Key);

                if (Keyboard.GetState().IsKeyDown(Keys.Up))
                {
                    //cam.Orientation *= Quaternion.CreateFromAxisAngle(cam.X, MathHelper.ToRadians(1));
                    cam.Orientation *= Quaternion.CreateFromYawPitchRoll(0, MathHelper.ToRadians(1), 0);
                    Matrix rotation = Matrix.CreateFromQuaternion(cam.Orientation);
                    //cam.View = Matrix.CreateTranslation(cam.Position) * rotation;
                    /*cam.X = Vector3.Transform(cam.X, rotation);
                    cam.Y = Vector3.Transform(cam.Y, rotation);
                    cam.Z = Vector3.Transform(cam.Z, rotation);*/
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Down))
                {
                    //cam.Orientation *= Quaternion.CreateFromAxisAngle(cam.X, MathHelper.ToRadians(-1));
                    cam.Orientation *= Quaternion.CreateFromYawPitchRoll(0, MathHelper.ToRadians(-1), 0);
                    Matrix rotation = Matrix.CreateFromQuaternion(cam.Orientation);
                    //cam.View = Matrix.CreateTranslation(cam.Position) * rotation;
                    /*cam.X = Vector3.Transform(cam.X, rotation);
                    cam.Y = Vector3.Transform(cam.Y, rotation);
                    cam.Z = Vector3.Transform(cam.Z, rotation);*/
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    //cam.Orientation *= Quaternion.CreateFromAxisAngle(Vector3.Up, MathHelper.ToRadians(-1));
                    cam.Orientation *= Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(1), 0, 0);
                    Matrix rotation = Matrix.CreateFromQuaternion(cam.Orientation);
                    cam.View = Matrix.CreateTranslation(cam.Position) * rotation;
                    //cam.X = Vector3.Transform(cam.X, rotation);
                    /*cam.Y = Vector3.Transform(cam.Y, rotation);
                    cam.Z = Vector3.Transform(cam.Z, rotation);*/
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    //cam.Orientation *= Quaternion.CreateFromAxisAngle(Vector3.Up, MathHelper.ToRadians(1));
                    cam.Orientation *= Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-1), 0, 0);
                    Matrix rotation = Matrix.CreateFromQuaternion(cam.Orientation);
                    cam.View = Matrix.CreateTranslation(cam.Position) * rotation;
                    //cam.X = Vector3.Transform(cam.X, rotation);
                    /*cam.Y = Vector3.Transform(cam.Y, rotation);
                    cam.Z = Vector3.Transform(cam.Z, rotation);*/
                }

                /*if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    transform.Position = new Vector3(transform.Position.X - 1f, transform.Position.Y, transform.Position.Z);
                    cam.Target = new Vector3(cam.Target.X - 1f, cam.Target.Y, cam.Target.Z);
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    transform.Position = new Vector3(transform.Position.X + 1f, transform.Position.Y, transform.Position.Z);
                    cam.Target = new Vector3(cam.Target.X + 1f, cam.Target.Y, cam.Target.Z);
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Up))
                {
                    transform.Position = new Vector3(transform.Position.X, transform.Position.Y - 1f, transform.Position.Z);
                    cam.Target = new Vector3(cam.Target.X, cam.Target.Y - 1f, cam.Target.Z);
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Down))
                {
                    transform.Position = new Vector3(transform.Position.X, transform.Position.Y + 1f, transform.Position.Z);
                    cam.Target = new Vector3(cam.Target.X, cam.Target.Y + 1f, cam.Target.Z);
                }
                if (Keyboard.GetState().IsKeyDown(Keys.OemPlus))
                {
                    transform.Position = new Vector3(transform.Position.X, transform.Position.Y, transform.Position.Z + 1f);
                }
                if (Keyboard.GetState().IsKeyDown(Keys.OemMinus))
                {
                    transform.Position = new Vector3(transform.Position.X, transform.Position.Y, transform.Position.Z - 1f);
                }

                cam.ViewMatrix = Matrix.CreateLookAt(transform.Position, cam.Target, cam.UpVector);*/
            }
        }

        public void Render(GraphicsDevice gd, BasicEffect be)
        {
            ComponentManager cm = ComponentManager.GetInstance();

            foreach (var entity in cm.GetComponentsOfType<CameraComponent>())
            {
                CameraComponent cam = (CameraComponent)entity.Value;

                be.Projection = cam.Projection;
                be.View = cam.View;
                be.World = Matrix.Identity;

            }
        }

        public void Load(ContentManager content)
        {
        }
    }
}
