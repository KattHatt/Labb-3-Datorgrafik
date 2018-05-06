using Engine.Components;
using Engine.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Engine.Systems
{
    public class CameraSystem : ISystem, IRender, IInit
    {
        ComponentManager cm = ComponentManager.GetInstance();

        BasicEffect be;

        public void Init(GraphicsDevice gd)
        {    
            be = new BasicEffect(gd)
            {
                Alpha = 1f,
                // Want to see the colors of the vertices, this needs to be on
                VertexColorEnabled = true,
                //Lighting requires normal information which VertexPositionColor does not have
                //If you want to use lighting and VPC you need to create a  custom def
                LightingEnabled = false
            };
            
        }

        public void Update(GameTime gametime)
        {
            foreach (var (_, camera) in cm.GetComponentsOfType<CameraComponent>())
            {
                camera.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(camera.FieldOfView), camera.AspectRatio, camera.NearPlaneDistance, camera.FarPlaneDistance);
                camera.View = Matrix.CreateLookAt(camera.Position, camera.Position + camera.Direction, camera.Up);

                if (Keyboard.GetState().IsKeyDown(Keys.Up))
                {
                    camera.Pitch(1);
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Down))
                {
                    camera.Pitch(-1);
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    camera.RotateY(1);
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    camera.RotateY(-1);
                }

                if (Keyboard.GetState().IsKeyDown(Keys.W))
                {
                    camera.Position += camera.Direction;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.S))
                {
                    camera.Position -= camera.Direction;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.A))
                {
                    camera.Position -= Vector3.Cross(camera.Direction, camera.Up);
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.D))
                {
                    camera.Position += Vector3.Cross(camera.Direction, camera.Up);
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Q))
                {
                    camera.Position -= Vector3.Up;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.E))
                {
                    camera.Position += Vector3.Up;
                }
            }
        }

        public void Render(GraphicsDevice graphicsDevice)
        {
            foreach (var (_, cam) in cm.GetComponentsOfType<CameraComponent>())
            {
                be.Projection = cam.Projection;
                be.View = cam.View;
                be.World = Matrix.Identity;
                cam.BoundingFrustum = new BoundingFrustum(cam.View * cam.Projection);
            }
        }

        public void RenderWithEffect(GraphicsDevice gd, Effect ef)
        {
            throw new NotImplementedException();
        }
    }

    public static class CameraComponentMethods
    {
        public static void Pitch(this CameraComponent cc, float angle)
        {
            Matrix rotation = Matrix.CreateFromAxisAngle(Vector3.Cross(cc.Direction, cc.Up), MathHelper.ToRadians(angle));
            cc.Direction = Vector3.Transform(cc.Direction, rotation);
            cc.Up = Vector3.Transform(cc.Up, rotation);
            cc.View = Matrix.CreateLookAt(cc.Position, cc.Position + cc.Direction, cc.Up);
        }

        public static void Yaw(this CameraComponent cc, float angle)
        {
            Matrix rotation = Matrix.CreateFromAxisAngle(cc.Up, MathHelper.ToRadians(angle));
            cc.Direction = Vector3.Transform(cc.Direction, rotation);
            cc.View = Matrix.CreateLookAt(cc.Position, cc.Position + cc.Direction, cc.Up);
        }

        public static void RotateY(this CameraComponent cc, float angle)
        {
            Matrix rotation = Matrix.CreateRotationY(MathHelper.ToRadians(angle));
            cc.Direction = Vector3.Transform(cc.Direction, rotation);
            cc.Up = Vector3.Transform(cc.Up, rotation);
            cc.View = Matrix.CreateLookAt(cc.Position, cc.Position + cc.Direction, cc.Up);
        }
    }
}
