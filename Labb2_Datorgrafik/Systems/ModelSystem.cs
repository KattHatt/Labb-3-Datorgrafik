using System;
using Microsoft.Xna.Framework;
using Labb2_Datorgrafik.Components;
using Labb2_Datorgrafik.Managers;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Labb2_Datorgrafik.Systems
{
    class ModelSystem : IRender
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
                if (!modelComp.IsActive)
                    continue;

                Matrix[] transforms = new Matrix[modelComp.Model.Bones.Count];
                modelComp.Model.CopyAbsoluteBoneTransformsTo(transforms);

                foreach (ModelMesh mesh in modelComp.Model.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {

                        effect.EnableDefaultLighting();
                        effect.View = be.View;
                        effect.Projection = be.Projection;
                        effect.World = transforms[mesh.ParentBone.Index] * transComp.World;

                        effect.AmbientLightColor = new Vector3(1f, 0, 0);
                        effect.CurrentTechnique.Passes[0].Apply();
                        mesh.Draw();
                    }
                }
            }
        }
    }
}
