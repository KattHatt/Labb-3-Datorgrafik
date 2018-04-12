using Labb2_Datorgrafik;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Labb1_Datorgrafik.Components
{
    public class RectangleComponent : IComponent
    {
        // Rectangle vertex positions
        public Vector3 FRONT_TOP_LEFT;
        public Vector3 FRONT_TOP_RIGHT;
        public Vector3 FRONT_BOTTOM_LEFT;
        public Vector3 FRONT_BOTTOM_RIGHT;
        public Vector3 BACK_TOP_LEFT;
        public Vector3 BACK_TOP_RIGHT;
        public Vector3 BACK_BOTTOM_LEFT;
        public Vector3 BACK_BOTTOM_RIGHT;

        // Vertices
        public VertexPositionNormalTexture[] vertices;

        // Rectangle Normals
        public Vector3 RIGHT = new Vector3(1, 0, 0); // +X
        public Vector3 LEFT = new Vector3(-1, 0, 0); // -X
        public Vector3 UP = new Vector3(0, 1, 0); // +Y
        public Vector3 DOWN = new Vector3(0, -1, 0); // -Y
        public Vector3 FORWARD = new Vector3(0, 0, 1); // +Z
        public Vector3 BACKWARD = new Vector3(0, 0, -1); // -Z





        public RectangleComponent()
        {
            FRONT_TOP_LEFT = new Vector3(-0.5f, 0.5f, 0.5f);
            FRONT_TOP_RIGHT = new Vector3(0.5f, 0.5f, 0.5f);
            FRONT_BOTTOM_LEFT = new Vector3(-0.5f, -0.5f, 0.5f);
            FRONT_BOTTOM_RIGHT = new Vector3(0.5f, -0.5f, 0.5f);
            BACK_TOP_LEFT = new Vector3(-0.5f, 0.5f, -0.5f);
            BACK_TOP_RIGHT = new Vector3(0.5f, 0.5f, -0.5f);
            BACK_BOTTOM_LEFT = new Vector3(-0.5f, -0.5f, -0.5f);
            BACK_BOTTOM_RIGHT = new Vector3(0.5f, -0.5f, -0.5f);
        }

        public RectangleComponent(float height, float width, float depth)
        {
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
