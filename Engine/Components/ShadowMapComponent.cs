using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Components
{
    public class ShadowMapComponent : IComponent
    {

        public string TextureName;
        public string EffectName;
        public Effect Effect;
        public Texture2D Texture;

        public float Ambient;
        public float LightPower;

        public Vector3 LightPos;
        public Matrix LightsView;
        public Matrix LightsProjection;

        public ShadowMapComponent()
        {
        }

        public ShadowMapComponent(string effectName, string textureName )
        {
            EffectName = effectName;
            TextureName = textureName;

            Ambient = 0.2f;
            LightPos = new Vector3(-97.40422f, 628.7715f, -375.7787f);
            LightPower = 1.0f;
            LightsView = Matrix.CreateLookAt(LightPos, new Vector3(-2, 3, -10), new Vector3(0, 1, 0));
            LightsProjection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver2, 1f, 5f, 100f);
        }
    }
}
