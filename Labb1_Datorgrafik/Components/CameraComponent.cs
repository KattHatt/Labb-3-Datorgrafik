using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Labb1_Datorgrafik.Components
{
    public class CameraComponent : IComponent
    {
        public Vector3 Target;
        public Matrix ProjectionMatrix;
        public Matrix ViewMatrix;
        public Matrix WorldMatrix;
        public Vector3 UpVector;
        public Vector3 Location;

        public Vector3 Position;
        public Vector3 Up;
        public Vector3 Direction;

        public Vector3 X, Y, Z;
        public Matrix Projection;
        public Matrix View;
        //public Matrix Rotation;
        public Quaternion Orientation;

        public CameraComponent(GraphicsDevice gd)
        {
            X = Vector3.UnitX;
            Y = Vector3.UnitY;
            Z = Vector3.UnitZ;
            Position = new Vector3(-10, -350, -170);
            Orientation = Quaternion.Identity;

            UpVector = Vector3.Up;
            Target = new Vector3(0f, 0f, 0f);
            Target = Vector3.Zero;
            Location = Vector3.UnitZ;
            float aspectRatio = gd.DisplayMode.AspectRatio;
            Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspectRatio, 0.1f, 1000.0f, out ProjectionMatrix);
            WorldMatrix = Matrix.CreateWorld(Target, Vector3.Forward, Vector3.Up);

            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), gd.DisplayMode.AspectRatio, 0.1f, 10000);
            Orientation *= Quaternion.CreateFromAxisAngle(X, 45);
            Matrix rotation = Matrix.CreateFromQuaternion(Orientation);
            X = Vector3.Transform(X, rotation);
            Y = Vector3.Transform(Y, rotation);
            Z = Vector3.Transform(Z, rotation);

            View = Matrix.CreateTranslation(Position) * rotation;

            //View = Matrix.CreateRotationX(MathHelper.ToRadians(45)) * Matrix.CreateTranslation(Position);
        }
    }
}