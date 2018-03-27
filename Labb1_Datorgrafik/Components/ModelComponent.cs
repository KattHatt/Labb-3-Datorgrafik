using Microsoft.Xna.Framework.Graphics;

namespace Labb1_Datorgrafik.Components
{
    public class ModelComponent : IComponent
    {
        public Model Model { get; set; }

        public ModelComponent(Model model)
        {
            Model = model;
        }
    }
}
