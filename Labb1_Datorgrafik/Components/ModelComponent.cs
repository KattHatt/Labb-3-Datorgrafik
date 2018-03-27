using Microsoft.Xna.Framework.Graphics;

namespace Labb1_Datorgrafik.Components
{
    public class ModelComponent : IComponent
    {
        public Model Model { get; set; }
        public string ModelPath { get; set; }
        public bool isActive { get; set; }

        public ModelComponent(string modelPath)
        {
            Model = null;
            ModelPath = modelPath;
            isActive = true;
        }
    }
}
