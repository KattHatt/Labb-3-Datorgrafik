using System;
using Microsoft.Xna.Framework;
using Labb1_Datorgrafik.Components;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Labb1_Datorgrafik
{
    class ModelSystem : ISystem
    {
        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime gametime)
        {
            ComponentManager cm = ComponentManager.GetInstance();

            foreach(var model in cm.GetComponentsOfType<ModelComponent>())
            {     
                ModelComponent modelComp = (ModelComponent)model.Value;
                if (modelComp.isActive)
                {
                    
                    
                }               
            }
        }

        public void Load(ContentManager content)
        {
            ComponentManager cm = ComponentManager.GetInstance();
            foreach (var entity in cm.GetComponentsOfType<ModelComponent>())
            {
                ModelComponent modelComp = (ModelComponent)entity.Value;
                modelComp.Model = content.Load<Model>(modelComp.ModelPath);
            }
        }
    }
}
