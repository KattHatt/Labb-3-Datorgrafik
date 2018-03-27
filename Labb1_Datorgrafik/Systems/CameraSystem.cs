using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Labb1_Datorgrafik.Managers;
using Labb1_Datorgrafik.Components;

namespace Labb1_Datorgrafik.Systems
{
    class CameraSystem : ISystem, IRender
    {
        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime gametime)
        {
            ComponentManager cm = ComponentManager.GetInstance();

            foreach (var entity in cm.GetComponentsOfType<CameraComponent>())
            {
                CameraComponent cam = (CameraComponent)entity.Value;
                TransformComponent transform = cm.GetComponentForEntity<TransformComponent>(entity.Key);

                if (Keyboard.GetState().IsKeyDown(Keys.Left))
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

                cam.ViewMatrix = Matrix.CreateLookAt(transform.Position, cam.Target, cam.UpVector);

                //Console.WriteLine("hej" + transform.Position);
                // TODO
            }
        }

        public void Render(GraphicsDevice gd, BasicEffect be)
        {
            ComponentManager cm = ComponentManager.GetInstance();

            foreach (var entity in cm.GetComponentsOfType<CameraComponent>())
            {
                CameraComponent cam = (CameraComponent)entity.Value;

                be.Projection = cam.ProjectionMatrix;
                be.View = cam.ViewMatrix;
                be.World = cam.WorldMatrix;
            }
        }
    }
}
