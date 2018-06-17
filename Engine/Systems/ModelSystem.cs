﻿using Engine.Components;
using Engine.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace Engine.Systems
{
    public class ModelSystem : IRender, ILoad
    {
        ComponentManager cm = ComponentManager.GetInstance();

        public void Load(ContentManager content)
        {
            Effect effect = content.Load<Effect>("shader");
            Texture2D defaultTexture = content.Load<Texture2D>("grass");

            foreach (var (_, m) in cm.GetComponentsOfType<ModelComponent>())
            {
                m.Model = content.Load<Model>(m.ModelFile);

                foreach (ModelMesh mesh in m.Model.Meshes)
                    foreach (BasicEffect currentEffect in mesh.Effects)
                        if (currentEffect.Texture == null)
                        {
                            m.Textures.Add(defaultTexture);
                        }
                        else
                        {
                            m.Textures.Add(currentEffect.Texture);
                        }
                            
                foreach (ModelMesh mesh in m.Model.Meshes)
                    foreach (ModelMeshPart meshPart in mesh.MeshParts)
                        meshPart.Effect = effect.Clone();
            }
        }

      
        public void RenderShadow(GraphicsDevice gd, Effect e)
        {
            CameraComponent camera = cm.GetComponentsOfType<CameraComponent>().First().Item2;

            foreach (var (_, model, trans) in cm.GetComponentsOfType<ModelComponent, TransformComponent>())
            {
                DrawModel(model.Model, model.Textures.ToArray(), trans.World, camera);
            }
        }

        private void DrawModel(Model model, Texture2D[] textures, Matrix wMatrix, CameraComponent camera)
        {
            int i = 0;
            Matrix[] modelTransforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(modelTransforms);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (Effect e in mesh.Effects)
                {
                    Matrix worldMatrix = modelTransforms[mesh.ParentBone.Index] * wMatrix;
                    e.CurrentTechnique = e.Techniques["Render"];
                    e.Parameters["LightView"].SetValue(camera.View);
                    e.Parameters["LightProjection"].SetValue(camera.Projection);
                    e.Parameters["FogStart"].SetValue(100f);
                    e.Parameters["FogEnd"].SetValue(1000f);
                    e.Parameters["FogColor"].SetValue(Color.CornflowerBlue.ToVector3());
                    e.Parameters["EyePosition"].SetValue(camera.Position);
                    e.Parameters["LightDirection"].SetValue(new Vector3(-0.5265408f, -0.5735765f, -0.6275069f));

                    e.Parameters["AmbientColor"].SetValue(Vector3.Zero);
                    e.Parameters["DiffuseColor"].SetValue(Vector3.One * 0.2f);
                    e.Parameters["SpecularColor"].SetValue(Vector3.One);
                    e.Parameters["SpecularPower"].SetValue(120f);
                   
                    e.Parameters["Texture"].SetValue(textures[i++]);
                    e.Parameters["World"].SetValue(worldMatrix);
                    e.Parameters["DiffuseIntensity"].SetValue(1f);
                    e.Parameters["SpecularIntensity"].SetValue(1f);
                    //e.Techniques["Render"].Passes[0].Apply();
                }
                mesh.Draw();
            }
        }

        public void Render(GraphicsDevice gd)
        {
            throw new System.NotImplementedException();
        }
    }
}