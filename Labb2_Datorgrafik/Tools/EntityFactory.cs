using Labb2_Datorgrafik.Components;
using Labb2_Datorgrafik.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Labb2_Datorgrafik.Tools
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

            TrackingCameraComponent trackingCamera = new TrackingCameraComponent(entityTrackId, new Vector3(0, 2, 10));

            return cm.AddEntityWithComponents(camera, trackingCamera, transform);
        }

        public static int CreateTerrain(GraphicsDevice gd, string heightMapFile, string heightMapTextureFile, string vegetationModelFile, int vegetationNumInstances)
        {
            HeightMapComponent hmc = new HeightMapComponent(gd)
            {
                HeightMapFilePath = heightMapFile,
                TextureFilePath = heightMapTextureFile,
            };
            VegetationComponent vc = new VegetationComponent(vegetationModelFile, vegetationNumInstances);
            return cm.AddEntityWithComponents(hmc, vc);
        }

        /*public static int CreateCubeParent(GraphicsDevice gd, string path, float size, Vector3 pos)
        {
            RectangleComponent cubelord = new RectangleComponent(gd, false, size + 1, size, size, path);
            TransformComponent trans = new TransformComponent()
            {
                Position = pos
            };
            int cube = cm.AddEntityWithComponents(cubelord, trans);

            return cube;
        }

        public static int CreateCubeKid(GraphicsDevice gd, string path, float size, int parent, Vector3 pos)
        {
            RectangleComponent cubekid = new RectangleComponent(gd, true, size, size, size, path);
            cubekid.Parent = parent;
            TransformComponent trans = new TransformComponent()
            {
                Position = pos
            };


            int cube = cm.AddEntityWithComponents(cubekid, trans);

            return cube;
        }*/

        /*public static int CreateCube(GraphicsDevice gd, string path, Vector3 size, Vector3 pos, int? parent)
        {
            RectangleComponent cubekid = new RectangleComponent(gd, false, size.Y, size.X, size.Z, path);
            cubekid.Parent = parent;
            TransformComponent trans = new TransformComponent()
            {
                Position = pos
            };
            int cube = cm.AddEntityWithComponents(cubekid, trans);

            return cube;
        }*/

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

        /*public static int CreateCube(GraphicsDevice gd, string path, Vector3 size, Vector3 pos, int? parent, string name, int root)
        {
            ComponentManager cm = ComponentManager.GetInstance();

            NameComponent nameComp = new NameComponent(name);
            RectangleComponent cubekid = new RectangleComponent(gd, false, size.Y, size.X, size.Z, path);
            cubekid.Parent = parent;
            cubekid.Root = root;
            TransformComponent trans = new TransformComponent()
            {
                Position = pos
            };
            int cube = cm.AddEntityWithComponents(cubekid, trans, nameComp);

            return cube;
        }*/

        /*public static int CreatePlayerBody(GraphicsDevice gd, Vector3 scale)
        {
            int body = CreateCube(gd, "grass", new Vector3(10, 10, 10), new Vector3(20, 350, -170), scale, null, "Body");
            int head = CreateCube(gd, "checkerboard", new Vector3(6, 6, 6), new Vector3(0, 8, 0), scale, body, "Head");
            int rightLegJoint = CreateCube(gd, "checkerboard", new Vector3(1, 1, 1), new Vector3(3, -1, 0), scale, body, "RightLegJoint");
            int leftLegJoint = CreateCube(gd, "checkerboard", new Vector3(1, 1, 1), new Vector3(-3, -1, 0), scale, body, "LeftLegJoint");
            int rightLeg = CreateCube(gd, "checkerboard", new Vector3(2, 10, 2), new Vector3(0, -9, 0), scale, rightLegJoint, "RightLeg");
            int leftLeg = CreateCube(gd, "checkerboard", new Vector3(2, 10, 2), new Vector3(0, -9, 0), scale, leftLegJoint, "LeftLeg");
            int rightArmJoint = CreateCube(gd, "checkerboard", new Vector3(1, 1, 1), new Vector3(6, 4.5f, 0), scale, body, "RightArmJoint");
            int leftArmJoint = CreateCube(gd, "checkerboard", new Vector3(1, 1, 1), new Vector3(-6, 4.5f, 0), scale, body, "LeftArmJoint");
            int rightArm = CreateCube(gd, "checkerboard", new Vector3(2, 10, 2), new Vector3(0, -5, 0), scale, rightArmJoint, "RightArm");
            int leftArm = CreateCube(gd, "checkerboard", new Vector3(2, 10, 2), new Vector3(0, -5, 0), scale, leftArmJoint, "LeftArm");

            AnimationComponent aComp = new AnimationComponent() { Animate = true };
            cm.AddComponentsToEntity(body, aComp);
            return body;
        }*/

        public static int CreatePlayerBodyLegs(GraphicsDevice gd, Vector3 scale)
        {
            int x = -400;
            int y = 200;
            int z = 120;

            /*int body = CreateCube(gd, "grass", new Vector3(10, 10, 10), new Vector3(x, y, z), null, "Body");
            int head = CreateCube(gd, "checkerboard", new Vector3(6, 6, 6), new Vector3(0, 8, 0), body, "Head");
            int rightLegJoint = CreateCube(gd, "checkerboard", new Vector3(1, 1, 1), new Vector3(3, -1, 0), body, "RightLegJoint");
            int leftLegJoint = CreateCube(gd, "checkerboard", new Vector3(1, 1, 1), new Vector3(-3, -1, 0), body, "LeftLegJoint");
            int rightLeg = CreateCube(gd, "grass", new Vector3(2, 10, 2), new Vector3(0, -9, 0), rightLegJoint, "RightLeg", body);
            int leftLeg = CreateCube(gd, "grass", new Vector3(2, 10, 2), new Vector3(0, -9, 0), leftLegJoint, "LeftLeg", body);*/

            int body = CreateCube(gd, "grass", new Vector3(x, y, z), new Vector3(-5, -5, -5), new Vector3(5, 5, 5), scale, null, "Body");
            int head = CreateCube(gd, "checkerboard", new Vector3(0, 5, 0), new Vector3(-3, 0, -3), new Vector3(3, 6, 3), body, "Head");
            int rightLegJoint = CreateCube(gd, "checkerboard", new Vector3(5, 1, 0), new Vector3(0, 0, 0), new Vector3(1, 1, 1), body, "RightLegJoint");
            int leftLegJoint = CreateCube(gd, "checkerboard", new Vector3(-5, 1, 0), new Vector3(-1, 0, 0), new Vector3(0, 1, 1), body, "LeftLegJoint");
            int rightLeg = CreateCube(gd, "grass", new Vector3(0, -9, 0), new Vector3(1, 0, 0), new Vector3(2, 10, 2), rightLegJoint, "RightLeg");
            int leftLeg = CreateCube(gd, "grass", new Vector3(0, -9, 0), new Vector3(-1, 0, 0), new Vector3(-2, 10, 2), leftLegJoint, "LeftLeg");

            AnimationComponent aComp = new AnimationComponent() { Animate = false };
            ComponentManager.GetInstance().AddComponentsToEntity(body, aComp);
            return body;
        }
    }
}
