using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Labb1_Datorgrafik
{
    public class CameraComponent : IComponent
    {
        public Vector3 Target { get; set; }
        public Matrix ProjectionMatrix { get; set; }
        public Matrix ViewMatrix { get; set; }

        public CameraComponent(GraphicsDevice gd)
        {
            Target = new Vector3(0f, 0f, 0f);
            ProjectionMatrix = new Matrix();
            ViewMatrix = new Matrix();
        }
    }
}