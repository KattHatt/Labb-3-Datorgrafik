using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Components
{
    public class RectangleComponent : IComponent
    {
        public Vector3 Center;
        public float Width;
        public float Height;
        public VertexBuffer VertexBuffer;

        public RectangleComponent(Vector3 center, float width, float height)
        {
            Center = center;
            Width = width;
            Height = height;
        }
    }
}
