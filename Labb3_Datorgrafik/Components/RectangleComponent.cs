using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Labb3_Datorgrafik.Components
{
    public class RectangleComponent : IComponent
    {
        // Graphics device
        public GraphicsDevice graphicsDevice;

        // Rectangle vertex positions
        public Vector3 FRONT_TOP_LEFT;
        public Vector3 FRONT_TOP_RIGHT;
        public Vector3 FRONT_BOTTOM_LEFT;
        public Vector3 FRONT_BOTTOM_RIGHT;
        public Vector3 BACK_TOP_LEFT;
        public Vector3 BACK_TOP_RIGHT;
        public Vector3 BACK_BOTTOM_LEFT;
        public Vector3 BACK_BOTTOM_RIGHT;

        // Bounding box
        public BoundingBox boundingBox;

        // Vertices
        public VertexPositionNormalTexture[] vertices;

        // Vetrex buffer
        public VertexBuffer vertexBuffer;

        // Indices
        public short[] indices;

        // Index buffer
        public IndexBuffer indexBuffers;
        
        // Children objecs
        public List<int> Children { get; set; }

        // Child flag
        public bool IsChild { get; set; }

        // Parent object
        public int? Parent { get; set; }

        // Root rectangle
        public int? Root { get; set; }

        // Textures
        public List<Texture2D> Textures { get; set; }

        // Texture paths
        public List<string> TexturePaths { get; set; }

        // Rectangle normals
        public Vector3 RIGHT = new Vector3(1, 0, 0); // +X
        public Vector3 LEFT = new Vector3(-1, 0, 0); // -X
        public Vector3 UP = new Vector3(0, 1, 0); // +Y
        public Vector3 DOWN = new Vector3(0, -1, 0); // -Y
        public Vector3 FORWARD = new Vector3(0, 0, 1); // +Z
        public Vector3 BACKWARD = new Vector3(0, 0, -1); // -Z

        // Custom Rectangle with one texture
        public RectangleComponent(
            GraphicsDevice gd,
            bool isChild,
            Vector3 corner1,
            Vector3 corner2,
            string texturePath)
        {
            Children = new List<int>();
            Textures = new List<Texture2D>();
            TexturePaths = new List<string>();

            IsChild = isChild;
            graphicsDevice = gd;
            TexturePaths.Add(texturePath);

            boundingBox = new BoundingBox(corner1, corner2);
            Vector3[] corners = boundingBox.GetCorners();

            FRONT_TOP_LEFT = corners[0];
            FRONT_TOP_RIGHT = corners[1];
            FRONT_BOTTOM_LEFT = corners[3];
            FRONT_BOTTOM_RIGHT = corners[2];
            BACK_TOP_LEFT = corners[4];
            BACK_TOP_RIGHT = corners[5];
            BACK_BOTTOM_LEFT = corners[7];
            BACK_BOTTOM_RIGHT = corners[6];
        }

        // Custom Rectangle with different textures
        public RectangleComponent(
            GraphicsDevice gd,
            bool isChild,
            float height,
            float width,
            float depth,
            string frontTP,
            string rightTP,
            string leftTP,
            string topTP,
            string botTP,
            string backTP)
        {
            graphicsDevice = gd;
            Children = new List<int>();
            Textures = new List<Texture2D>();
            TexturePaths = new List<string>
            {
                frontTP,
                rightTP,
                leftTP,
                topTP,
                botTP,
                backTP
            };
            IsChild = isChild;

            FRONT_TOP_LEFT = new Vector3(-width / 2, height / 2, depth / 2);
            FRONT_TOP_RIGHT = new Vector3(width / 2, height / 2, depth / 2);
            FRONT_BOTTOM_LEFT = new Vector3(-width / 2, -height / 2, depth / 2);
            FRONT_BOTTOM_RIGHT = new Vector3(width / 2, -height / 2, depth / 2);
            BACK_TOP_LEFT = new Vector3(-width / 2, height / 2, -depth / 2);
            BACK_TOP_RIGHT = new Vector3(width / 2, height / 2, -depth / 2);
            BACK_BOTTOM_LEFT = new Vector3(-width / 2, -height / 2, -depth / 2);
            BACK_BOTTOM_RIGHT = new Vector3(width / 2, -height / 2, -depth / 2);
        }
    }
}
