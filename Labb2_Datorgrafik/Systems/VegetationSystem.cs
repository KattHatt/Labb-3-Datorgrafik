using Labb2_Datorgrafik.Components;
using Labb2_Datorgrafik.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Labb2_Datorgrafik.Systems
{
    public class VegetationSystem : ISystem, IRender
    {
        ComponentManager cm = ComponentManager.GetInstance();

        public void Load(ContentManager content)
        {
            foreach (var (_, hmc, vc) in cm.GetComponentsOfType<HeightMapComponent, VegetationComponent>())
            {
                vc.Model = content.Load<Model>(vc.ModelFile);
                List<Matrix> instances = new List<Matrix>();

                for (int i = 0; i < vc.NumInstances; i++)
                {
                    instances.Add(PlaceVegetation(vc, hmc));
                }

                vc.Instances = instances.ToArray();
            }
        }

        public void Render(GraphicsDevice gd, BasicEffect be)
        {
            foreach (var (_, vc) in cm.GetComponentsOfType2<VegetationComponent>())
            {
                Matrix[] transforms = new Matrix[vc.Model.Bones.Count];
                vc.Model.CopyAbsoluteBoneTransformsTo(transforms);

                foreach (var instance in vc.Instances)
                {
                    foreach (var mesh in vc.Model.Meshes)
                    {
                        foreach (BasicEffect effect in mesh.Effects)
                        {
                            effect.EnableDefaultLighting();
                            effect.View = be.View;
                            effect.Projection = be.Projection;
                            effect.World = transforms[mesh.ParentBone.Index] * instance;
                            effect.AmbientLightColor = Color.DarkGreen.ToVector3();
                            
                            effect.CurrentTechnique.Passes[0].Apply();
                            mesh.Draw();
                        }
                    }
                }
            }

        }

        public void Update(GameTime gametime)
        {
        }

        private Matrix PlaceVegetation(VegetationComponent vc, HeightMapComponent hmc)
        {
            Random random = new Random();
            VertexPositionTexture[] vertices;
            int[] indices;
            int x = random.Next((int)Math.Ceiling(hmc.BoundingBox.Min.X), (int)hmc.BoundingBox.Max.X);
            int y = (int)Math.Ceiling(hmc.BoundingBox.Max.Y);
            int z = random.Next((int)Math.Ceiling(hmc.BoundingBox.Min.Z), (int)hmc.BoundingBox.Max.Z);

            Ray ray = new Ray(new Vector3(x, y, z), Vector3.Down);
            for (int i = 0; i < hmc.BoundingBoxes.Length; i++)
            {
                float? distance = ray.Intersects(hmc.BoundingBoxes[i]);
                if (distance != null)
                {
                    vertices = new VertexPositionTexture[hmc.VertexBuffers[i].VertexCount];
                    hmc.VertexBuffers[i].GetData(vertices);
                    indices = new int[hmc.IndexBuffers[i].IndexCount];
                    hmc.IndexBuffers[i].GetData(indices);

                    for (int j = 0; j < indices.Length; j += 3)
                    {
                        Plane plane = new Plane(vertices[indices[j]].Position, vertices[indices[j + 1]].Position, vertices[indices[j + 2]].Position);
                        ray.Intersects(plane);
                    }
                }
            }

            return Matrix.CreateScale(0.01f) * Matrix.CreateTranslation(x, y, z);
        }
    }
}
