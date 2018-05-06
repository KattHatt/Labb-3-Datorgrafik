using Engine.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Tools
{
    public static class ModelHelper
    {
        // Rotates a model on the given axis the number of degrees
        public static Matrix rotateModel(Matrix toRotate, Vector3 axis, float degrees)
        {
            Matrix old = toRotate;

            return Matrix.CreateTranslation(Vector3.Zero) * Matrix.CreateFromAxisAngle(axis, degrees) * old;
        }

        public static Matrix rotateAroundPoint(Matrix toRotate, Matrix pointOfRotation, Vector3 axis, float degrees)
        {
            return Matrix.CreateTranslation(Vector3.Zero) * Matrix.CreateFromAxisAngle(axis, degrees) * pointOfRotation;
        }

        public static void Render(BasicEffect be, ModelComponent mc, Matrix world)
        {
            if (!mc.IsActive)
                return;

            Matrix[] transforms = new Matrix[mc.Model.Bones.Count];
            mc.Model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in mc.Model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {

                    //effect.EnableDefaultLighting();
                    effect.View = be.View;
                    effect.Projection = be.Projection;
                    effect.World = transforms[mesh.ParentBone.Index] * world;

                    effect.AmbientLightColor = new Vector3(1f, 0, 0);
                    effect.CurrentTechnique.Passes[0].Apply();
                    mesh.Draw();
                }
            }
        }

        public static void DrawModelWithAmbientEffect(Model model, Matrix world, Matrix view, Matrix projection, Effect effect)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = effect;
                    effect.Parameters["World"].SetValue(world * mesh.ParentBone.Transform);
                    effect.Parameters["View"].SetValue(view);
                    effect.Parameters["Projection"].SetValue(projection);

                    // Optional, cuz there is default params in the shader
                    //effect.Parameters["AmbientColor"].SetValue(Color.Green.ToVector4());
                    //effect.Parameters["AmbientIntensity"].SetValue(0.5f);
                }
                mesh.Draw();
            }
        }

        public static bool ShouldRender(BoundingFrustum frustum, BoundingBox box)
        {
            return true;
        }
    }
}
