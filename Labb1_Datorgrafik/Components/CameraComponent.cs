using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Labb1_Datorgrafik.Components
{
    public class CameraComponent : IComponent
    {
        public Vector3 Position;
        public Vector3 Up;
        public Vector3 Direction;
        
        public Matrix Projection;
        public Matrix View;

        public CameraComponent(GraphicsDevice gd)
        {
            Position = new Vector3(-10, 350, -170);
            Direction = Vector3.Right;
            Up = Vector3.Up;

            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), gd.DisplayMode.AspectRatio, 0.1f, 10000);
            View = Matrix.CreateLookAt(Position, Position + Direction, Up);
        }

        public void Pitch(float angle)
        {
            Matrix rotation = Matrix.CreateFromAxisAngle(Vector3.Cross(Direction, Up), MathHelper.ToRadians(angle));
            Direction = Vector3.Transform(Direction, rotation);
            Up = Vector3.Transform(Up, rotation);
            View = Matrix.CreateLookAt(Position, Position + Direction, Up);
        }

        public void Yaw(float angle)
        {
            Matrix rotation = Matrix.CreateFromAxisAngle(Up, MathHelper.ToRadians(angle));
            Direction = Vector3.Transform(Direction, rotation);
            View = Matrix.CreateLookAt(Position, Position + Direction, Up);
        }
    }
}