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

        public VegetationComponent(string modelFile, int numInstances)
        {
            ModelFile = modelFile;
            NumInstances = numInstances;
        }
    }
}
