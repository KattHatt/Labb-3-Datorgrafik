using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Labb1_Datorgrafik
{
    public class CameraComponent : IComponent
    {
        GraphicsDevice GraphicsDevice;
        public Vector3 Target { get; set; }
        public Matrix ProjectionMatrix { get; set; }
        public Matrix ViewMatrix { get; set; }

        public CameraComponent(GraphicsDevice gd)
        {
            GraphicsDevice = gd;
        }
    }
}
