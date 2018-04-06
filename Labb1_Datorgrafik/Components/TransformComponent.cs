using Microsoft.Xna.Framework;

namespace Labb1_Datorgrafik.Components
{
    public class TransformComponent : IComponent
    {
        public Vector3 Position;
        public Vector3 Rotation;
        public Vector3 Scale;
        public Vector3 Up;
        public Matrix World;

        public TransformComponent()
        {
            Position = Vector3.Zero;
            Rotation = Vector3.Zero;
            Scale = Vector3.One;
            Up = Vector3.Up;
        }
    }
}
