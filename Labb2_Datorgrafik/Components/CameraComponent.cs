using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Labb2_Datorgrafik.Components
{
    public class CameraComponent : IComponent
    {     
        public Matrix Projection;
        public Matrix View;

        public float FieldOfView;
        public float AspectRatio;
        public float NearPlaneDistance;
        public float FarPlaneDistance;
    }
}