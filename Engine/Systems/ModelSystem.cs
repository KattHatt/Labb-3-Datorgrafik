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
            ef = content.Load<Effect>("ShaderXXX");

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
                        // VertexShader
                        part.Effect = ef;
                        ef.CurrentTechnique = ef.Techniques["VertexShading"];
                        ef.Parameters["xWorld"].SetValue(tc.World * mesh.ParentBone.Transform);
                        ef.Parameters["xView"].SetValue(cam.View);
                        ef.Parameters["xProjection"].SetValue(cam.Projection);
                        ef.Parameters["xLightDirection"].SetValue(new Vector3(1, 0, 0));

                        foreach(EffectPass pass in ef.CurrentTechnique.Passes)
                        {
                            pass.Apply();
                            gd.SetVertexBuffer(part.VertexBuffer);

                            VertexPositionNormalTexture[] vertices = new VertexPositionNormalTexture[part.NumVertices]; // <-- Wtf?
                            part.VertexBuffer.GetData(vertices);

                            gd.DrawUserPrimitives(PrimitiveType.TriangleList, vertices, 0, 2);
                        }
                        
                    }
                    
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