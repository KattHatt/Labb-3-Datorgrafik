namespace Engine.Components
{
    public class LightComponent : IComponent
    {
        public float Ambient;

        public LightComponent()
        {
        }

        public LightComponent(float ambient)
        {
            Ambient = ambient;
        }
    }
}
