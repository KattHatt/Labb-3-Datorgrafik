using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Labb1_Datorgrafik
{
    public class Camera
    {
        public Vector3 CamTarget { get; set; }
        public Vector3 CamPosition { get; set; }
        public Matrix ProjectionMatrix { get; set; }
        public Matrix ViewMatrix { get; set; }
        public Matrix WorldMatrix { get; set; }

        public Camera(GraphicsDevice gd)
        {
            CamTarget = new Vector3(0f, 0f, 0f);
            CamPosition = new Vector3(0f, 0f, -100f);
            ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45f), gd.DisplayMode.AspectRatio, 1f, 1000f);

            ViewMatrix = Matrix.CreateLookAt(CamPosition, CamTarget, new Vector3(0f, 1f, 0f));// Y up

            WorldMatrix = Matrix.CreateWorld(CamTarget, Vector3.Forward, Vector3.Up);
        }
    }
}
