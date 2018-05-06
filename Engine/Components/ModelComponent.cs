using Microsoft.Xna.Framework.Graphics;

namespace Engine.Components
{
    public class ModelComponent : IComponent
    {
        public Model Model { get; set; }
        public string ModelPath { get; set; }
        public bool IsActive { get; set; }

        public ModelComponent()
        {
            Model = null;
            ModelPath = null;
            IsActive = false;
        }

        public ModelComponent(string modelPath, bool isActive)
        {
            Model = null;
            ModelPath = modelPath;
            IsActive = isActive;
        }
    }
}
