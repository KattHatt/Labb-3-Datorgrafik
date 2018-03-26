using Microsoft.Xna.Framework;

namespace Labb1_Datorgrafik
{
    public class TransformComponent : IComponent
    {
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public Vector3 Scale { get; set; }
    }
}
