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

            AnimationComponent aComp = new AnimationComponent() { Animate = false };
            ComponentManager.GetInstance().AddComponentsToEntity(body, aComp);
            return body;
        }

        public static int CreateBoundingBoxForEnt(GraphicsDevice gd, int entity)
        {
            BoundingBoxComponent bbc = new BoundingBoxComponent(gd) { BelongsToID = entity };

            return cm.AddEntityWithComponents(bbc);
        }
    }
}
