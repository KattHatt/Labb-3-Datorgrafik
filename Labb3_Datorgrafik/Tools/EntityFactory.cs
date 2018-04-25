using Labb3_Datorgrafik.Components;
using Labb3_Datorgrafik.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;

namespace Labb3_Datorgrafik.Tools
{
    public static class EntityFactory
    {
        static ComponentManager cm = ComponentManager.GetInstance();

        public static int CreateTrackingCamera(GraphicsDevice gd, int entityTrackId)
        {
            TransformComponent transform = new TransformComponent()
            {
                Position = new Vector3(-10, 350, -170),
                Rotation = Vector3.Right,
                Up = Vector3.Up
            };

            CameraComponent camera = new CameraComponent()
            {
                FieldOfView = 45,
                NearPlaneDistance = 1,
                FarPlaneDistance = 10000,
                AspectRatio = gd.DisplayMode.AspectRatio,
            };

            TrackingCameraComponent trackingCamera = new TrackingCameraComponent(entityTrackId, new Vector3(0, 3, 10));

            return cm.AddEntityWithComponents(camera, trackingCamera, transform);
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

        public static int CreatePlayerBodyLegs(GraphicsDevice gd, Vector3 scale)
        {
            int x = -400;
            int y = 200;
            int z = 120;

            int body = CreateCube(gd, "grass", new Vector3(x, y, z), new Vector3(-5, -5, -5), new Vector3(5, 5, 5), scale, null, "Body");
            int head = CreateCube(gd, "checkerboard", new Vector3(0, 5, 0), new Vector3(-3, 0, -3), new Vector3(3, 6, 3), body, "Head");
            int rightLegJoint = CreateCube(gd, "checkerboard", new Vector3(5, 1, 0), new Vector3(0, 0, 0), new Vector3(1, 1, 1), body, "RightLegJoint");
            int leftLegJoint = CreateCube(gd, "checkerboard", new Vector3(-5, 1, 0), new Vector3(-1, 0, 0), new Vector3(0, 1, 1), body, "LeftLegJoint");
            int rightLeg = CreateCube(gd, "grass", new Vector3(0, -9, 0), new Vector3(1, 0, 0), new Vector3(2, 10, 2), rightLegJoint, "RightLeg");
            int leftLeg = CreateCube(gd, "grass", new Vector3(0, -9, 0), new Vector3(-1, 0, 0), new Vector3(-2, 10, 2), leftLegJoint, "LeftLeg");
            int rightArmJoint = CreateCube(gd, "checkerboard", new Vector3(3, -6, 0), new Vector3(0, 0, 0), new Vector3(1, 1, 1), body, "RightArmJoint");
            int leftArmJoint = CreateCube(gd, "checkerboard", new Vector3(-3, -6, 0), new Vector3(0, 0, 0), new Vector3(1, 1, 1), body, "LeftArmJoint");
            int rightArm = CreateCube(gd, "grass", new Vector3(0, -9, 0), new Vector3(1, 0, 0), new Vector3(2, 10, 2), rightArmJoint, "RightArm");
            int leftArm = CreateCube(gd, "grass", new Vector3(0, -9, 0), new Vector3(-1, 0, 0), new Vector3(-2, 10, 2), leftArmJoint, "LeftArm");

            AnimationComponent aComp = new AnimationComponent() { Animate = true };
            ComponentManager.GetInstance().AddComponentsToEntity(body, aComp);
            return body;
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
