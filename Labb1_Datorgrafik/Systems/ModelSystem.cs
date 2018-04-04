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
            throw new NotImplementedException();
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


            foreach (var model in cm.GetComponentsOfType<ModelComponent>())
            {
                ModelComponent modelComp = (ModelComponent)model.Value;
                if (modelComp.IsActive)
                {
                    Matrix[] transforms = new Matrix[modelComp.Model.Bones.Count];
                    float aspectRatio = gd.Viewport.AspectRatio;
                    modelComp.Model.CopyAbsoluteBoneTransformsTo(transforms);
                    Matrix projection = Matrix.CreatePerspectiveFieldOfView(
                        MathHelper.ToRadians(45.0f),
                        aspectRatio,
                        1.0f,
                        10000.0f);
                    Matrix view = Matrix.CreateLookAt(
                        new Vector3(0.0f, 50.0f, 1.0f),
                        Vector3.Zero,
                        Vector3.Up);

                    foreach (ModelMesh mesh in modelComp.Model.Meshes)
                    {
                        foreach (BasicEffect effect in mesh.Effects)
                        {
                            effect.EnableDefaultLighting();
                            effect.View = view;
                            effect.Projection = projection;
                            effect.World = Matrix.Identity;
                            effect.AmbientLightColor = new Vector3(1f, 0, 0);
                        }
                        mesh.Draw();
                    }
                }
            }

        }
    }
}
