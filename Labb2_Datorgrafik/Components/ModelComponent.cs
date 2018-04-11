using Microsoft.Xna.Framework.Graphics;

namespace Labb2_Datorgrafik.Components
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

        public ModelComponent(string modelPath)
        {
            Model = null;
            ModelPath = modelPath;
            IsActive = true;
        }
    }
}
