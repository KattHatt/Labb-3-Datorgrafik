using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Labb1_Datorgrafik
{
    public class CameraComponent : IComponent
    {
        public Vector3 Target;
        public Matrix ProjectionMatrix;
        public Matrix ViewMatrix;
        public Vector3 UpVector;

        public CameraComponent(GraphicsDevice gd)
        {
            UpVector = new Vector3(0, 1, 0);
            Target = new Vector3(0f, 0f, 0f);
            float aspectRatio = gd.DisplayMode.AspectRatio;
            Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspectRatio, 0.1f, 100.0f, out ProjectionMatrix);
        }
    }
}