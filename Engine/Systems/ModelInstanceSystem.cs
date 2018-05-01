using Engine.Components;
using Engine.Managers;
using Engine.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;

namespace Engine.Systems
{
    public class ModelInstanceSystem : IRender, ILoad
    {
        ComponentManager cm = ComponentManager.GetInstance();

        public void Load(ContentManager content)
        {
            HeightMapComponent hmc = cm.GetComponentsOfType<HeightMapComponent>().First().Item2;

            foreach (var (k, bbc, mic) in cm.GetComponentsOfType<BoundingBoxComponent, ModelInstanceComponent>())
            {
                mic.Instance = PlaceVegetation(hmc);
            }
        }

        public void Render(GraphicsDevice gd, BasicEffect be)
        {
            BoundingFrustum frustum = new BoundingFrustum(be.View * be.Projection);

            ComponentManager cm = ComponentManager.GetInstance();
            foreach (var (_, mic, box) in cm.GetComponentsOfType<ModelInstanceComponent, BoundingBoxComponent>())
            {
                ModelComponent mc = cm.GetComponentForEntity<ModelComponent>(mic.ModelEntityId);
                if (frustum.Contains(box.BoundingBox) != ContainmentType.Disjoint)
                    ModelHelper.Render(be, mc, mic.Instance);
            }
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

        private Matrix PlaceVegetation(HeightMapComponent hmc)
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

