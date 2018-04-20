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
            foreach (var (_, cam, transform) in cm.GetComponentsOfType<CameraComponent, TransformComponent>())
            {
                cam.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(cam.FieldOfView), cam.AspectRatio, cam.NearPlaneDistance, cam.FarPlaneDistance);
                cam.View = Matrix.CreateLookAt(transform.Position, transform.Position + transform.Rotation, transform.Up);
            }
        }

        public void Render(GraphicsDevice graphicsDevice, BasicEffect basicEffect)
        {
            foreach (var (_, cam) in cm.GetComponentsOfType<CameraComponent>())
            {
                basicEffect.Projection = cam.Projection;
                basicEffect.View = cam.View;
                basicEffect.World = Matrix.Identity;
                cam.BoundingFrustum = new BoundingFrustum(cam.View * cam.Projection);
            }
        }

        public void Load(ContentManager content)
        {
        }
    }
}
