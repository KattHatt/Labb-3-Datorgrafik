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
        Texture2D texture;

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
                modelComp.Texture = content.Load<Texture2D>(modelComp.TexturePath);
            }
            CameraComponent cam = cm.GetComponentsOfType<CameraComponent>().First().Item2;

            //ef = content.Load<Effect>("Ambient");
            ef = content.Load<Effect>("Fog");
            //ef = content.Load<Effect>("DiffuseLight");

            texture = content.Load<Texture2D>("grass");

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
                        // General stuff
                        part.Effect = ef;
                        ef.Parameters["World"].SetValue(tc.World * mesh.ParentBone.Transform);
                        ef.Parameters["View"].SetValue(cam.View);
                        ef.Parameters["Projection"].SetValue(cam.Projection);
                        //ef.Parameters["ViewVector"].SetValue(cam.View.Translation);
                        //ef.Parameters["ModelTexture"].SetValue(mc.Texture);

                        //// For Fog - cant see no fog yo
                        ef.Parameters["FogEnabled"].SetValue(1.0f);
                        ef.Parameters["FogStart"].SetValue(0.0f); // Dunno vilket värde de ska ha här
                        ef.Parameters["FogEnd"].SetValue(1.0f); // Dunno vilket värde de ska ha här
                        ef.Parameters["FogColor"].SetValue(Color.Blue.ToVector3()); // väljs: Color.CornflowerBlue.ToVector3() så försvinner modellerna :S
                        ef.Parameters["cameraPos"].SetValue(cam.Position);
                        ////ef.Parameters["Texture"].SetValue(texture);


                        // For DiffuseLight - dosent work :S
                        //Matrix worldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(mesh.ParentBone.Transform * tc.World));
                        //ef.Parameters["WorldInverseTranspose"].SetValue(worldInverseTransposeMatrix);




                        // for Ambient
                        //ef.Parameters["AmbientColor"].SetValue(Color.Green.ToVector4());
                        //ef.Parameters["AmbientIntensity"].SetValue(0.5f);
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