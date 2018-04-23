using Microsoft.Xna.Framework;

namespace Labb2_Datorgrafik.Components
{
    public class ModelInstanceComponent
    {
        int ModelEntityId;
        Matrix Instance;

        public ModelInstanceComponent(int modelEntityId, Matrix instance)
        {
            ModelEntityId = modelEntityId;
            Instance = instance;
        }
    }
}
