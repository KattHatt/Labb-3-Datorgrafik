using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb1_Datorgrafik.Components
{
    public class HeightMapComponent : IComponent
    {
        public string HeightMapFilePath;
        public string TextureFilePath;
        public Texture2D HeightMap;
        public Texture2D texture;
        public int Width;
        public int Height;
        public GraphicsDevice graphicsDevice;
        public VertexBuffer vertexBuffer;
        public IndexBuffer indexBuffer;

        public HeightMapComponent(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
        }
    }
}
