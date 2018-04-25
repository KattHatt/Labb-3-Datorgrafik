using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Labb3_Datorgrafik.Components
{
    public class BoundingBoxComponent : IComponent
    {
        public GraphicsDevice GraphicsDevice;
        public BoundingBox BoundingBox { get; set; }
        public bool Collision { get; set; }
        public int? CollidedBoxID { get; set; }

        public int BelongsToID { get; set; }
        public bool Render { get; set; }

        public VertexBuffer Vertices { get; set; }
        public int VertexCount { get; set; }
        public IndexBuffer Indices { get; set; }
        public int PrimitiveCount { get; set; }

        public BoundingBoxComponent(GraphicsDevice gd)
        {
            GraphicsDevice = gd;
        }
    }
}

