using Labb2_Datorgrafik;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Labb1_Datorgrafik.Components
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

        // Vertices
        public VertexPositionNormalTexture[] vertices;

        // Vetrex buffer
        public VertexBuffer vertexBuffer;

        // Indices
        public int[] indices;

        // Index buffer
        public IndexBuffer indexBuffer;

        // Textures
        public Texture2D frontFace;
        public Texture2D leftFace;
        public Texture2D rightFace;
        public Texture2D topFace;
        public Texture2D botFace;
        public Texture2D backFace;

        // Texture paths
        public string FrontTexturePath { set; get; }
        public string LeftTexturePath { set; get; }
        public string RightTexturePath { set; get; }
        public string TopTexturePath { set; get; }
        public string BotTexturePath { set; get; }
        public string BackTexturePath { set; get; }


        // Rectangle normals
        public Vector3 RIGHT = new Vector3(1, 0, 0); // +X
        public Vector3 LEFT = new Vector3(-1, 0, 0); // -X
        public Vector3 UP = new Vector3(0, 1, 0); // +Y
        public Vector3 DOWN = new Vector3(0, -1, 0); // -Y
        public Vector3 FORWARD = new Vector3(0, 0, 1); // +Z
        public Vector3 BACKWARD = new Vector3(0, 0, -1); // -Z

        // Cube with no texture
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

        // Custom Rectangle with texture
        public RectangleComponent(
            GraphicsDevice gd,
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

            FrontTexturePath = frontTP;
            LeftTexturePath = leftTP;
            RightTexturePath = rightTP;
            TopTexturePath = topTP;
            BotTexturePath = botTP;
            BackTexturePath = backTP;

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
