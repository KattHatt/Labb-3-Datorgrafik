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

        public CameraComponent(GraphicsDevice gd)
        {
            UpVector = Vector3.Up;
            Target = new Vector3(0f, 0f, 0f);
            float aspectRatio = gd.DisplayMode.AspectRatio;
            Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspectRatio, 0.1f, 1000.0f, out ProjectionMatrix);
            WorldMatrix = Matrix.CreateWorld(Target, Vector3.Forward, Vector3.Up);
        }
    }
}