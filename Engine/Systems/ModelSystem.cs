using Engine.Components;
using Engine.Managers;
using Engine.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace Engine.Systems
{
    public class ModelSystem : IRender, ILoad, IInit
    {
        ComponentManager cm = ComponentManager.GetInstance();
        BasicEffect be;
        Effect ef;

        public void Init(GraphicsDevice gd)
        {
            CameraComponent cam = cm.GetComponentsOfType<CameraComponent>().First().Item2;
            be = new BasicEffect(gd)
            {
                VertexColorEnabled = false,
                TextureEnabled = true
            };
            
        }

        public void Load(ContentManager content)
        {
            foreach (var (_, modelComp) in cm.GetComponentsOfType<ModelComponent>())
            {
                modelComp.Model = content.Load<Model>(modelComp.ModelPath);
            }
            CameraComponent cam = cm.GetComponentsOfType<CameraComponent>().First().Item2;

            ef = content.Load<Effect>("Ambient");
            
        }

        public void Render(GraphicsDevice gd)
        {
            foreach (var (entity, mc) in cm.GetComponentsOfType<ModelComponent>())
            {
                if (!mc.IsActive)
                    continue;

                TransformComponent tc = cm.GetComponentForEntity<TransformComponent>(entity);
                Matrix[] transforms = new Matrix[mc.Model.Bones.Count];
                mc.Model.CopyAbsoluteBoneTransformsTo(transforms);

                CameraComponent cam = cm.GetComponentsOfType<CameraComponent>().First().Item2;

                foreach (ModelMesh mesh in mc.Model.Meshes)
                {
                    foreach (ModelMeshPart part in mesh.MeshParts)
                    {
                        part.Effect = ef;
                        ef.Parameters["World"].SetValue(tc.World * mesh.ParentBone.Transform);
                        ef.Parameters["View"].SetValue(cam.View);
                        ef.Parameters["Projection"].SetValue(cam.Projection);

                        // Optional, cuz there is default params in the shader
                        ef.Parameters["AmbientColor"].SetValue(Color.Green.ToVector4());
                        ef.Parameters["AmbientIntensity"].SetValue(0.5f);
                    }
                    mesh.Draw();
                }
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