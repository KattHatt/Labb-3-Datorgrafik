using Labb2_Datorgrafik.Components;
using Labb2_Datorgrafik.Managers;
using Labb2_Datorgrafik.Tools;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Labb2_Datorgrafik.Systems
{
    public class ModelInstanceSystem : IRender
    {
        public void Load(ContentManager content)
        {
        }

        public void Render(GraphicsDevice gd, BasicEffect be)
        {
            ComponentManager cm = ComponentManager.GetInstance();
            foreach (var (_, mic) in cm.GetComponentsOfType<ModelInstanceComponent>())
            {
                ModelComponent mc = cm.GetComponentForEntity<ModelComponent>(mic.ModelEntityId);
                ModelHelper.Render(be, mc, mic.Instance);
            }
        }
    }
}
