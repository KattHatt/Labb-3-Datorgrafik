using Labb2_Datorgrafik;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Labb2_Datorgrafik.Components
{
    public class VegetationComponent : IComponent
    {
        public string ModelFile;
        public Model Model;
        public Matrix[] Instances;
        public int NumInstances;
        public int HeightmapId;
        public string TextureFile;
        public Texture2D Texture;

        public VegetationComponent(int heightmapId, string modelFile, string textureFile, int numInstances)
        {
            HeightmapId = heightmapId;
            ModelFile = modelFile;
            NumInstances = numInstances;
            TextureFile = textureFile;
        }
    }
}
