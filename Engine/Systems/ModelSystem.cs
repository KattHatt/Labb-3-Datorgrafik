using Engine.Components;
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
            foreach (var (_, m) in cm.GetComponentsOfType<ModelComponent>())
            {
                m.Model = content.Load<Model>(m.ModelFile);
                m.Texture = content.Load<Texture2D>(m.TextureFile);
            }
        }

        public void RenderShadow(GraphicsDevice gd, Effect e)
        {
            //CameraComponent camera = cm.GetComponentsOfType<CameraComponent>().First().Item2;

            //e.Parameters["LightView"].SetValue(camera.View);
            //e.Parameters["LightProjection"].SetValue(camera.Projection);
            //e.Parameters["FogStart"].SetValue(100f);
            //e.Parameters["FogEnd"].SetValue(1000f);
            //e.Parameters["FogColor"].SetValue(Color.CornflowerBlue.ToVector3());
            //e.Parameters["EyePosition"].SetValue(camera.Position);
            //e.Parameters["LightDirection"].SetValue(new Vector3(-0.5265408f, -0.5735765f, -0.6275069f));

            //e.Parameters["AmbientColor"].SetValue(Vector3.Zero);
            //e.Parameters["DiffuseColor"].SetValue(Vector3.One);
            //e.Parameters["SpecularColor"].SetValue(Vector3.One);
            //e.Parameters["SpecularPower"].SetValue(120f);

            //foreach (var (_, model, trans) in cm.GetComponentsOfType<ModelComponent, TransformComponent>())
            //{
            //    foreach (ModelMesh mesh in model.Model.Meshes)
            //    {
            //        Matrix[] modelTransforms = new Matrix[model.Model.Bones.Count];
            //        model.Model.CopyAbsoluteBoneTransformsTo(modelTransforms);

            //        Matrix worldMatrix = modelTransforms[mesh.ParentBone.Index] * trans.World;
            //        e.Parameters["Texture"].SetValue(model.Texture);
            //        e.Parameters["World"].SetValue(worldMatrix);
            //        e.Techniques["Render"].Passes[0].Apply();

            //        mesh.Draw();
            //    }
                
            //}
        }

        public void Render(GraphicsDevice gd)
        {
            throw new System.NotImplementedException();
        }
    }
}