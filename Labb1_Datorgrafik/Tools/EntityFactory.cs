using Labb1_Datorgrafik.Components;
using Labb1_Datorgrafik.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Labb1_Datorgrafik.Tools
{
    public static class EntityFactory
    {
        public static int CreateCamera(GraphicsDevice gd, int entityTrackId)
        {
            ComponentManager cm = ComponentManager.GetInstance();

            CameraComponent camera = new CameraComponent()
            {
                Position = new Vector3(-10, 350, -170),
                Direction = Vector3.Right,
                Up = Vector3.Up,
                FieldOfView = 45,
                NearPlaneDistance = 1,
                FarPlaneDistance = 10000,
                AspectRatio = gd.DisplayMode.AspectRatio,
            };

            TrackingCameraComponent trackComp = new TrackingCameraComponent(entityTrackId, new Vector3(0, 10, 20));

            int cam = cm.AddEntityWithComponents(new IComponent[] { camera, trackComp } );
            return cam;
        }

        public static int CreateHeightMap(GraphicsDevice gd, string heightMapFilePath)
        {
            ComponentManager cm = ComponentManager.GetInstance();

            HeightMapComponent hmComp = new HeightMapComponent(gd)
            {
                HeightMapFilePath = heightMapFilePath
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
    }
}
