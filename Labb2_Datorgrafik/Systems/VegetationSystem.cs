using Labb2_Datorgrafik.Components;
using Labb2_Datorgrafik.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
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
            var frustum = cm.GetComponentsOfType<CameraComponent>().First().Item2.BoundingFrustum;

            foreach (var (_, vc) in cm.GetComponentsOfType<VegetationComponent>())
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

                            BoundingSphere sphere = mesh.BoundingSphere;
                            //sphere.Transform(effect.World);
                            if (frustum.Contains(sphere) != ContainmentType.Disjoint)
                            {
                                effect.CurrentTechnique.Passes[0].Apply();
                                mesh.Draw();
                            }
                        }
                    }
                }
            }
        }

        public void Update(GameTime gametime)
        {
        }

        private Matrix GetRotation(Vector3 normal)
        {
            normal.Normalize();
            normal *= -1;

            Vector3 rotationAxis = Vector3.Cross(normal, Vector3.Up);
            rotationAxis.Normalize();
            float scalar = Vector3.Dot(normal, Vector3.Up);
            float angle = (float)Math.Acos(scalar);

            return Matrix.CreateFromAxisAngle(rotationAxis, angle);
        }

        private Matrix PlaceVegetation(VegetationComponent vc, HeightMapComponent hmc)
        {
            Random random = new Random();
            VertexPositionTexture[] vertices;
            int[] indices;

            float x = random.Next((int)Math.Ceiling(hmc.BoundingBox.Min.X), (int)hmc.BoundingBox.Max.X);
            float y = (float)Math.Ceiling(hmc.BoundingBox.Max.Y);
            float z = random.Next((int)Math.Ceiling(hmc.BoundingBox.Min.Z), (int)hmc.BoundingBox.Max.Z);

            float? distance;
            Matrix rotation = Matrix.Identity;

            Ray ray = new Ray(new Vector3(x, y, z), Vector3.Down);
            for (int i = 0; i < hmc.BoundingBoxes.Length; i++)
            {
                distance = ray.Intersects(hmc.BoundingBoxes[i]);
                if (distance != null)
                {
                    vertices = new VertexPositionTexture[hmc.VertexBuffers[i].VertexCount];
                    hmc.VertexBuffers[i].GetData(vertices);
                    indices = new int[hmc.IndexBuffers[i].IndexCount];
                    hmc.IndexBuffers[i].GetData(indices);

                    for (int j = 0; j < indices.Length; j += 3)
                    {
                        Vector3 vertex1 = vertices[indices[j]].Position;
                        Vector3 vertex2 = vertices[indices[j + 1]].Position;
                        Vector3 vertex3 = vertices[indices[j + 2]].Position;
                        distance = ray.Intersects(vertex1, vertex2, vertex3);

                        if (distance != null)
                        {
                            y = ray.Position.Y - distance.Value;
                            //rotation = GetRotation(Vector3.Cross(vertex2 - vertex1, vertex3 - vertex1));
                            break;
                        }
                    }

                    break;
                }
            }

            return Matrix.CreateScale(0.05f) * rotation * Matrix.CreateTranslation(x, y, z);
        }
    }
}
