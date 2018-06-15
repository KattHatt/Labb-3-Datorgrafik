using Microsoft.Xna.Framework.Graphics;

namespace Engine.Components
{
    public class ModelComponent : IComponent
    {
        public Model Model;
        public Texture2D Texture;
        public string ModelFile;
        public string TextureFile;

        public ModelComponent(string modelFile, string textureFile)
        {
            ModelFile = modelFile;
            TextureFile = textureFile;
        }
    }
}