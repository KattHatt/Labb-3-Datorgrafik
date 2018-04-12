using Labb2_Datorgrafik;
using Microsoft.Xna.Framework;

namespace Labb1_Datorgrafik.Components
{
    public class RectangleComponent : IComponent
    {
        // Rectangle vertex positions
        private Vector3 FRONT_TOP_LEFT;
        private Vector3 FRONT_TOP_RIGHT;
        private Vector3 FRONT_BOTTOM_LEFT;
        private Vector3 FRONT_BOTTOM_RIGHT;
        private Vector3 BACK_TOP_LEFT;
        private Vector3 BACK_TOP_RIGHT;
        private Vector3 BACK_BOTTOM_LEFT;
        private Vector3 BACK_BOTTOM_RIGHT;

        // Rectangle Normals
        private  Vector3 RIGHT = new Vector3(1, 0, 0); // +X
        private  Vector3 LEFT = new Vector3(-1, 0, 0); // -X
        private  Vector3 UP = new Vector3(0, 1, 0); // +Y
        private  Vector3 DOWN = new Vector3(0, -1, 0); // -Y
        private  Vector3 FORWARD = new Vector3(0, 0, 1); // +Z
        private  Vector3 BACKWARD = new Vector3(0, 0, -1); // -Z





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
