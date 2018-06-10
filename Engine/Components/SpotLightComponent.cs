using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Components
{
    public class SpotLightComponent : IComponent
    {
        public float AmbientPower;
        public float LightPower;
        public string EffectName;
        public Effect Effect;
        public Vector3 LightPos;
        public Texture2D Texture;
        public string TextureName;

        public SpotLightComponent(string effectName, string textureName)
        {
            EffectName = effectName;
            LightPos = new Vector3(-97.40422f, 628.7715f, -375.7787f);
            LightPower = 1.0f;
            AmbientPower = 0.2f;
            TextureName = textureName;
        }
    }
}
