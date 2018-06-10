using Microsoft.Xna.Framework.Graphics;

namespace Engine.Components
{
    public class ShadowMapComponent : IComponent
    {

        public string TextureName;
        public string EffectName;
        public Effect Effect;
        public Texture2D Texture;

        public int Key;

        public ShadowMapComponent()
        {
        }

        public ShadowMapComponent(string effectName, string textureName )
        {
            EffectName = effectName;
            TextureName = textureName;
        }
    }
}
