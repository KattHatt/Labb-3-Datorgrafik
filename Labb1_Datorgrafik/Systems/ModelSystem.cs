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
            Matrix world = Matrix.Identity;
            Matrix objectWorld;

            foreach (var model in cm.GetComponentsOfType<ModelComponent>())
            {
                ModelComponent modelComp = (ModelComponent)model.Value;
                if (modelComp.IsActive)
                {
                    TransformComponent transComp = cm.GetComponentForEntity<TransformComponent>(model.Key);

                    objectWorld = Matrix.CreateScale(transComp.Scale) * Matrix.CreateTranslation(transComp.Position);

                    Matrix[] transforms = new Matrix[modelComp.Model.Bones.Count];
                    float aspectRatio = gd.Viewport.AspectRatio;
                    modelComp.Model.CopyAbsoluteBoneTransformsTo(transforms);

                    Matrix projection = Matrix.CreatePerspectiveFieldOfView(
                        MathHelper.ToRadians(45.0f),
                        aspectRatio,
                        1.0f,
                        10000.0f);

                    Matrix view = Matrix.CreateLookAt(
                        new Vector3(0.0f, 30.0f, 1.0f),
                        Vector3.Zero,
                        Vector3.Up);

                    foreach (ModelMesh mesh in modelComp.Model.Meshes)
                    {
                        foreach (BasicEffect effect in mesh.Effects)
                        {

                            effect.EnableDefaultLighting();
                            effect.View = be.View;
                            effect.Projection = be.Projection;
                            effect.World = 
                                Matrix.Identity * 
                                transforms[mesh.ParentBone.Index] * 
                                Matrix.CreateTranslation(transComp.Position);
                            //* Matrix.CreateFromQuaternion(Quaternion.CreateFromYawPitchRoll(transComp.Rotation.X, transComp.Rotation.Y, transComp.Rotation.Z)

                            effect.AmbientLightColor = new Vector3(1f, 0, 0);
                            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                            {
                                pass.Apply();
                            }
                            mesh.Draw();

                            /*effect.View = be.View;
                            effect.Projection = be.Projection;
                            effect.World = Matrix.CreateTranslation(transComp.Position);
                            effect.CurrentTechnique.Passes[0].Apply();
                            mesh.Draw();*/
                        }
                    }
                }
            }
        }
    }
}
