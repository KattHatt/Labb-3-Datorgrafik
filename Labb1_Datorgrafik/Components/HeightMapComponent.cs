using Microsoft.Xna.Framework.Graphics;

namespace Labb1_Datorgrafik.Components
{
    public class HeightMapComponent : IComponent
    {
        public string HeightMapFilePath;
        public Texture2D HeightMap;
        public int Width;
        public int Height;
        public GraphicsDevice GraphicsDevice;
        public VertexBuffer VertexBuffer;
        public IndexBuffer IndexBuffer;

        public HeightMapComponent(GraphicsDevice graphicsDevice)
        {
            GraphicsDevice = graphicsDevice;
        }
    }
}
