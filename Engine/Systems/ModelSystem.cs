using Engine.Components;
using Engine.Managers;
using Engine.Tools;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace Engine.Systems
{
    public class ModelSystem : IRender, ILoad
    {
        public void Load(ContentManager content)
        {
            ComponentManager cm = ComponentManager.GetInstance();
            foreach (var (_, modelComp) in cm.GetComponentsOfType<ModelComponent>())
            {
                modelComp.Model = content.Load<Model>(modelComp.ModelPath);
            }
        }

        public void Render(GraphicsDevice gd, BasicEffect be)
        {
            ComponentManager cm = ComponentManager.GetInstance();

            foreach (var (_, modelComp, transComp) in cm.GetComponentsOfType<ModelComponent, TransformComponent>())
            {
                ModelHelper.Render(be, modelComp, transComp.World);
            }
        }

        public void RenderWithEffect(GraphicsDevice gd, Effect ef)
        {
            ComponentManager cm = ComponentManager.GetInstance();
            CameraComponent cam = cm.GetComponentsOfType<CameraComponent>().First().Item2;

            foreach (var (_, modelComp, transComp) in cm.GetComponentsOfType<ModelComponent, TransformComponent>())
            {
                ModelHelper.DrawModelWithAmbientEffect(modelComp.Model, transComp.World, cam.View, cam.Projection, ef);
            }
        }
    }
}