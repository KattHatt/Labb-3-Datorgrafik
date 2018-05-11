using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Components
{
    public class SpotLightComponent : IComponent
    {
        public float Ambient;
        public Vector3 ConeDirection;
        public float Angle;
        public float ConeDecay;
        public float LightStrength;
        public string EffectName;
        public Effect Effect;
        public VertexPositionNormalTexture[] VertexPNT;
        public int Key;

        public SpotLightComponent()
        {
            Ambient = 0.2f;
            ConeDirection = Vector3.Down;
            Angle = 0.5f;
            ConeDecay = 2.0f;
            LightStrength = 0.7f;
            EffectName = "SpotLight";
        }

        public SpotLightComponent(float ambient, Vector3 coneDir, float angle, float coneDecay, float lightStrenght, string effectName)
        {
            Ambient = ambient;
            ConeDirection = coneDir;
            Angle = angle;
            ConeDecay = coneDecay;
            LightStrength = lightStrenght;
            EffectName = effectName;
        }
    }
}
