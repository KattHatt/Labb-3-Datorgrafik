using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Engine.Components
{
    public class BoxComponent : IComponent
    {
        // Graphics device
        public GraphicsDevice GraphicsDevice;

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
        public BoundingBox BoundingBox;

        // Vertices
        public VertexPositionNormalTexture[] Vertices;

        // Vetrex buffer
        public VertexBuffer VertexBuffer;

        // Textures
        public Texture2D Texture;

        // Texture paths
        public string TexturePath;

        // Rectangle normals
        public Vector3 RIGHT = new Vector3(1, 0, 0); // +X
        public Vector3 LEFT = new Vector3(-1, 0, 0); // -X
        public Vector3 UP = new Vector3(0, 1, 0); // +Y
        public Vector3 DOWN = new Vector3(0, -1, 0); // -Y
        public Vector3 FORWARD = new Vector3(0, 0, 1); // +Z
        public Vector3 BACKWARD = new Vector3(0, 0, -1); // -Z


        public BoxComponent(GraphicsDevice gd)
        {
            GraphicsDevice = gd;
        }

        // Custom Rectangle with one texture
        public BoxComponent(
            GraphicsDevice gd,
            Vector3 corner1,
            Vector3 corner2)
        {
            GraphicsDevice = gd;

            BoundingBox = new BoundingBox(corner1, corner2);
            Vector3[] corners = BoundingBox.GetCorners();

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
        public BoxComponent(
            GraphicsDevice gd,
            float height,
            float width,
            float depth,
            string texturePath)
        {
            GraphicsDevice = gd;
            TexturePath = texturePath;

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