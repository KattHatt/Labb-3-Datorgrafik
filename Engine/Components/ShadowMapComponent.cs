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
    }
}
