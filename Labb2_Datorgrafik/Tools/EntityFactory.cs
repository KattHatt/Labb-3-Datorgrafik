using Labb1_Datorgrafik.Components;
using Labb2_Datorgrafik.Components;
using Labb2_Datorgrafik.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Labb2_Datorgrafik.Tools
{
    public static class EntityFactory
    {
        public static int CreateCamera(GraphicsDevice gd, int entityTrackId)
        {
            ComponentManager cm = ComponentManager.GetInstance();

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

            TrackingCameraComponent trackComp = new TrackingCameraComponent(entityTrackId, new Vector3(0, 10, 20));

            int cam = cm.AddEntityWithComponents(new IComponent[] { camera, trackComp, transform } );
            return cam;
        }

        public static int CreateHeightMap(GraphicsDevice gd, string heightMapFilePath, string textureFilePath)
        {
            ComponentManager cm = ComponentManager.GetInstance();

            HeightMapComponent hmComp = new HeightMapComponent(gd)
            {
                HeightMapFilePath = heightMapFilePath,
                TextureFilePath = textureFilePath,
            };
            int hm = cm.AddEntityWithComponents(hmComp);

            return hm;
        }

        public static int CreateChopper(GraphicsDevice gd, string modelPath)
        {
            ComponentManager cm = ComponentManager.GetInstance();

            ModelComponent modComp = new ModelComponent(modelPath)
            {
                IsActive = true
            };
            TransformComponent transComp = new TransformComponent()
            {
                Position = new Vector3(20, 350, -170)
            };
            NameComponent nameComp = new NameComponent("Chopper");


            int chop = cm.AddEntityWithComponents(modComp, transComp, nameComp);

            return chop;
        }

        public static int CreateCubeParent(GraphicsDevice gd, string path, float size, Vector3 pos)
        {
            ComponentManager cm = ComponentManager.GetInstance();

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
            ComponentManager cm = ComponentManager.GetInstance();

            RectangleComponent cubekid = new RectangleComponent(gd, true, size, size, size, path);
            cubekid.Parent = parent;
            TransformComponent trans = new TransformComponent()
            {
                Position = pos
            };


            int cube = cm.AddEntityWithComponents(cubekid, trans);

            return cube;
        }

        public static int CreateCube(GraphicsDevice gd, string path, Vector3 size, Vector3 pos, int? parent)
        {
            ComponentManager cm = ComponentManager.GetInstance();

            RectangleComponent cubekid = new RectangleComponent(gd, false, size.Y, size.X, size.Z, path);
            cubekid.Parent = parent;
            TransformComponent trans = new TransformComponent()
            {
                Position = pos
            };
            int cube = cm.AddEntityWithComponents(cubekid, trans);

            return cube;
        }

        public static int CreateCube(GraphicsDevice gd, string path, Vector3 size, Vector3 pos, int? parent, string name)
        {
            ComponentManager cm = ComponentManager.GetInstance();

            NameComponent nameComp = new NameComponent(name);
            RectangleComponent cubekid = new RectangleComponent(gd, false, size.Y, size.X, size.Z, path);
            cubekid.Parent = parent;
            TransformComponent trans = new TransformComponent()
            {
                Position = pos
            };
            int cube = cm.AddEntityWithComponents(cubekid, trans, nameComp);

            return cube;
        }

        public static int CreatePlayerBody(GraphicsDevice gd)
        {
            int body = CreateCube(gd, "grass", new Vector3(10, 10, 10), new Vector3(20, 350, -170), null, "Body");
            int head = CreateCube(gd, "checkerboard", new Vector3(6, 6, 6), new Vector3(0, 8, 0), body, "Head");
            int rightLegJoint = CreateCube(gd, "checkerboard", new Vector3(1, 1, 1), new Vector3(3, -1, 0), body, "RightLegJoint");
            int leftLegJoint = CreateCube(gd, "checkerboard", new Vector3(1, 1, 1), new Vector3(-3, -1, 0), body, "LeftLegJoint");
            int rightLeg = CreateCube(gd, "checkerboard", new Vector3(2, 10, 2), new Vector3(0, -9, 0), rightLegJoint, "RightLeg");
            int leftLeg = CreateCube(gd, "checkerboard", new Vector3(2, 10, 2), new Vector3(0, -9, 0), leftLegJoint, "LeftLeg");
            int rightArmJoint = CreateCube(gd, "checkerboard", new Vector3(1, 1, 1), new Vector3(6, 4.5f, 0), body, "RightArmJoint");
            int leftArmJoint = CreateCube(gd, "checkerboard", new Vector3(1, 1, 1), new Vector3(-6, 4.5f, 0), body, "LeftArmJoint");
            int rightArm = CreateCube(gd, "checkerboard", new Vector3(2, 10, 2), new Vector3(0, -5, 0), rightArmJoint, "RightArm");
            int leftArm = CreateCube(gd, "checkerboard", new Vector3(2, 10, 2), new Vector3(0, -5, 0), leftArmJoint, "LeftArm");

            AnimationComponent aComp = new AnimationComponent() { Animate = true };
            ComponentManager.GetInstance().AddComponentsToEntity(body, aComp);
            return body;
        }
    }
}
