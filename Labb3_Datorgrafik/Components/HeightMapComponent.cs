﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Labb3_Datorgrafik.Components
{
    public class HeightMapComponent : IComponent
    {
        public string HeightMapFilePath;
        public Texture2D HeightMap;
        public string TextureFilePath;
        public Texture2D Texture;
        public int Width;
        public int Height;
        public GraphicsDevice GraphicsDevice;
        public VertexBuffer[] VertexBuffers;
        public IndexBuffer[] IndexBuffers;
        public Vector3[][] Vertices;
        public int[][] Indices;
        public BoundingBox[] BoundingBoxes;
        public BoundingBox BoundingBox;
        public bool RenderBoundingBoxes;
        public float[,] HeightData;

        public HeightMapComponent(GraphicsDevice graphicsDevice)
        {
            GraphicsDevice = graphicsDevice;
        }
    }
}
