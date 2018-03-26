using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Labb1_Datorgrafik
{
    public class CameraComponent : IComponent
    {
        public Vector3 Position { get; set; }
        public Vector3 Target { get; set; }
        public Matrix ProjectionMatrix { get; set; }
        public Matrix ViewMatrix { get; set; }

        public CameraComponent(GraphicsDevice gd)
        {
            Target = new Vector3(0f, 0f, 0f);
            Position = new Vector3(0f, 0f, -100f);
            ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(45f),
                gd.DisplayMode.AspectRatio,
                1f,
                1000f
                );
            ViewMatrix = Matrix.CreateLookAt(
                Position,
                Target,
                new Vector3(0f, 1f, 0f)
                );
        }
    }
}
