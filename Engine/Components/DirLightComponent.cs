using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Components
{
    public class DirLightComponent : IComponent
    {
        public float AmbientPower;
        public float LightPower;
        public string EffectName;
        public string TextureName;
        public Effect Effect;
        public Vector3 LightPos;
        public Texture2D Texture;

        public DirLightComponent()
        {
        }
    }
}
