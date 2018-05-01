using Microsoft.Xna.Framework;

namespace Labb3_Datorgrafik.Components
{
    public class CameraComponent : IComponent
    {
        public Vector3 Position;
        public Vector3 Up;
        public Vector3 Direction;

        public Matrix Projection;
        public Matrix View;
        public BoundingFrustum BoundingFrustum;

        public float FieldOfView;
        public float AspectRatio;
        public float NearPlaneDistance;
        public float FarPlaneDistance;
    }
}