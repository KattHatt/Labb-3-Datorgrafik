using Engine.Components;
using Engine.Managers;
using Engine.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Labb3_Datorgrafik.Tools
{
    public static class EntityFactory
    {
        static ComponentManager cm = ComponentManager.GetInstance();

        public static int CreateCamera(GraphicsDevice gd)
        {
            CameraComponent camera = new CameraComponent()
            {
                Position = new Vector3(-155, 270, -287),
                Up = Vector3.Right,
                Direction = Vector3.Down,
                FieldOfView = 45,
                NearPlaneDistance = 1,
                FarPlaneDistance = 10000,
                AspectRatio = gd.DisplayMode.AspectRatio,
            };

            return cm.AddEntityWithComponents(camera);
        }

        public static int CreateTerrain(GraphicsDevice gd, string heightMapFile, string heightMapTextureFile)
        {
            HeightMapComponent hmc = new HeightMapComponent(gd)
            {
                HeightMapFilePath = heightMapFile,
                TextureFilePath = heightMapTextureFile,
            };
            return cm.AddEntityWithComponents(hmc);
        }

        public static int CreateCube(GraphicsDevice gd, string path, Vector3 position, Vector3 corner1, Vector3 corner2, Vector3 scale, int? parent, string name)
        {
            NameComponent nameComp = new NameComponent(name);
            RectangleComponent cubekid = new RectangleComponent(gd, false, corner1, corner2, path);
            cubekid.Parent = parent;
            TransformComponent trans = new TransformComponent()
            {
                Position = position
            };

            if (!parent.HasValue)
                trans.Scale = scale;

            int cube = cm.AddEntityWithComponents(cubekid, trans, nameComp);

            return cube;
        }

        public static int CreateCube(GraphicsDevice gd, string path, Vector3 position, Vector3 corner1, Vector3 corner2, int? parent, string name)
        {
            return CreateCube(gd, path, position, corner1, corner2, Vector3.One, parent, name);
        }

        public static int CreateBoundingBoxForEnt(GraphicsDevice gd, int entity)
        {
            BoundingBoxComponent bbc = new BoundingBoxComponent(gd) { BelongsToID = entity, Render = true };

            return cm.AddEntityWithComponents(bbc);
        }

        public static int CreateModel(string texure)
        {
            ModelComponent m = new ModelComponent()
            {
                IsActive = true,
                ModelPath = texure
            };

            return cm.AddEntityWithComponents(m);

        }
        public static void CreateVeggies(GraphicsDevice gd, int model, int count)
        {
            for(int i = 0; i< count; i++)
            {
                cm.AddEntityWithComponents(new ModelInstanceComponent(model), new BoundingBoxComponent(gd) { BelongsToID = model, Render = true});
            }
        }


        private static  Matrix PlaceVegetation(HeightMapComponent hmc)
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
