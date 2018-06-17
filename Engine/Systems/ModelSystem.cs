using Engine.Components;
using Engine.Managers;
using Engine.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
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
                //modelComp.Texture = content.Load<Texture2D>(modelComp.TexturePath);
            }
            CameraComponent cam = cm.GetComponentsOfType<CameraComponent>().First().Item2;

            //ef = content.Load<Effect>("Ambient");
            //ef = content.Load<Effect>("ShaderXXX");
            ef = content.Load<Effect>("Shader");
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
                DirLightComponent dirLight = cm.GetComponentsOfType<DirLightComponent>().First().Item2;
                ShadowMapComponent shadow = cm.GetComponentsOfType<ShadowMapComponent>().First().Item2;

                
                ef.CurrentTechnique = ef.Techniques["Render"];
                ef.Parameters["View"].SetValue(cam.View);
                ef.Parameters["Projection"].SetValue(cam.Projection);
                ef.Parameters["FogStart"].SetValue(100f);
                ef.Parameters["FogEnd"].SetValue(1000f);
                ef.Parameters["FogColor"].SetValue(Color.CornflowerBlue.ToVector3());
                ef.Parameters["EyePosition"].SetValue(cam.Position);
                ef.Parameters["LightDirection"].SetValue(new Vector3(-0.5265408f, -0.5735765f, -0.6275069f));

                ef.Parameters["AmbientColor"].SetValue(Vector3.Zero);
                ef.Parameters["DiffuseColor"].SetValue(Vector3.One * 0.2f);
                ef.Parameters["SpecularColor"].SetValue(Vector3.One);
                ef.Parameters["SpecularPower"].SetValue(120f);

                ef.Parameters["DiffuseIntensity"].SetValue(1f);
                ef.Parameters["SpecularIntensity"].SetValue(1f);

                ef.Parameters["xLightPos"].SetValue(dirLight.LightPos);

                ef.Parameters["xLightPower"].SetValue(dirLight.LightPower);
                ef.Parameters["xAmbient"].SetValue(dirLight.AmbientPower);
                //shadow
                shadow.Effect.CurrentTechnique = shadow.Effect.Techniques["ShadowMap"];

                shadow.Effect.Parameters["xWorldViewProjection"].SetValue(Matrix.Identity * cam.View * cam.Projection);
                shadow.Effect.Parameters["xLightsWorldViewProjection"].SetValue(shadow.LightsView * shadow.LightsProjection);
                
                shadow.Effect.Parameters["xLightPos"].SetValue(shadow.LightPos);
                shadow.Effect.Parameters["xLightPower"].SetValue(shadow.LightPower);
                shadow.Effect.Parameters["xAmbient"].SetValue(shadow.Ambient);
                shadow.Effect.Parameters["xTexture"].SetValue(shadow.Texture);
                //dirlight
                dirLight.Effect.CurrentTechnique = dirLight.Effect.Techniques["DirLight"];

                dirLight.Effect.Parameters["xWorldViewProjection"].SetValue(cam.View * cam.Projection);
                
                dirLight.Effect.Parameters["xLightPos"].SetValue(dirLight.LightPos);

                dirLight.Effect.Parameters["xLightPower"].SetValue(dirLight.LightPower);
                dirLight.Effect.Parameters["xAmbient"].SetValue(dirLight.AmbientPower);
                dirLight.Effect.Parameters["xTexture"].SetValue(dirLight.Texture);

                foreach (ModelMesh mesh in mc.Model.Meshes)
                {
                    Matrix worldMatrix = transforms[mesh.ParentBone.Index] * tc.World;

                    //dirLight.Effect.Parameters["xWorld"].SetValue(worldMatrix);
                    //foreach (ModelMeshPart part in mesh.MeshParts)
                    //{
                    //    part.Effect = dirLight.Effect;
                    //}
                    //mesh.Draw();

                    //shadow.Effect.Parameters["xWorld"].SetValue(worldMatrix);
                    //foreach (ModelMeshPart part in mesh.MeshParts)
                    //{
                    //    part.Effect = shadow.Effect;
                    //}
                    //mesh.Draw();

                    ef.Parameters["Texture"].SetValue(shadow.Texture);
                    
                    ef.Parameters["World"].SetValue(worldMatrix);
                    foreach (ModelMeshPart part in mesh.MeshParts)
                    {
                        part.Effect = ef;
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