using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb1_Datorgrafik.Components
{
    class HeightMapComponent
    {
        public string HeightMapFilePath;
        public string TextureFilePath;
        public Texture2D HeightMap;
        public Texture2D texture;
        public VertexPositionTexture[] Vertices;
        public int Width;
        public int Height;
        public int[] Indices;
        public float[,] HeightMapData;
    }
}
