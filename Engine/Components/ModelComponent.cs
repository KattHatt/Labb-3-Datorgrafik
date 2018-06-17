using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Engine.Components
{
    public class ModelComponent : IComponent
    {
        public Model Model;
        public List<Texture2D> Textures;
        public string ModelFile;

        public ModelComponent(string modelFile)
        {
            ModelFile = modelFile;
            Textures = new List<Texture2D>();
        }
    }
}