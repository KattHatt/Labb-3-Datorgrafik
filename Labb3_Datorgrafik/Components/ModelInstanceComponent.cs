using Microsoft.Xna.Framework;

namespace Labb3_Datorgrafik.Components
{
    public class ModelInstanceComponent : IComponent
    {
        public int ModelEntityId;
        public Matrix Instance;

        public ModelInstanceComponent(int modelEntityId, Matrix instance)
        {
            ModelEntityId = modelEntityId;
            Instance = instance;
        }
        public ModelInstanceComponent(int modelEntityId)
        {
            ModelEntityId = modelEntityId; 
        }
    }
}
