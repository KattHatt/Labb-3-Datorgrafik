using System;
using Microsoft.Xna.Framework;
using Labb1_Datorgrafik.Components;
using Labb1_Datorgrafik.Managers;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Labb1_Datorgrafik.Systems
{
    class ModelSystem : ISystem, IRender
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
                if (modelComp.IsActive)
                {
                    // TODO?
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

        public void Render(GraphicsDevice gd, BasicEffect be)
        {
            ComponentManager cm = ComponentManager.GetInstance();

            foreach(var world in cm.GetComponentsOfType<WorldComponent>())
            {
                WorldComponent worldMatrixComponent = (WorldComponent)world.Value;

                foreach (var model in cm.GetComponentsOfType<ModelComponent>())
                {
                    ModelComponent modelComp = (ModelComponent)model.Value;
                    if (modelComp.IsActive)
                    {
                        foreach (ModelMesh mesh in modelComp.Model.Meshes)
                        {
                            foreach (BasicEffect effect in mesh.Effects)
                            {
                                //effect.EnableDefaultLighting();
                                effect.AmbientLightColor = new Vector3(1f, 0, 0);
                                //effect.View = viewMatrix;
                                effect.World = worldMatrixComponent.WorldMatrix;
                                //effect.Projection = projectionMatrix;
                            }
                            mesh.Draw();
                        }
                    }
                }
            }
        }
    }
}
