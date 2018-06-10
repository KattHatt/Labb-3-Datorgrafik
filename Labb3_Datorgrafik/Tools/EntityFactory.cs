using Engine.Components;
using Engine.Managers;
using Engine.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Labb3_Datorgrafik.Tools
{
    public static class EntityFactory
    {
        static ComponentManager cm = ComponentManager.GetInstance();

        public static void CreateShadowMap()
        {
            ShadowMapComponent shadow = new ShadowMapComponent("ShadowMap", "grass");

            cm.AddEntityWithComponents(shadow);
        }

        public static int CreateSpotLight()
        {
            TransformComponent trans = new TransformComponent() { Position = new Vector3(-97.40422f, 628.7715f, -375.7787f) };
            SpotLightComponent spot = new SpotLightComponent(){};

            return cm.AddEntityWithComponents(trans, spot);
        }

        public static int CreateCamera(GraphicsDevice gd)
        {
            CameraComponent camera = new CameraComponent()
            {
                Position = new Vector3(-70, 315, -340),
                Up = Vector3.Up,
                Direction = Vector3.Left,
                FieldOfView = 45,
                NearPlaneDistance = 1,
                FarPlaneDistance = 10000,
                AspectRatio = gd.DisplayMode.AspectRatio,
            };

            camera.Pitch(-20);
            camera.RotateY(20);

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

        
        public static int CreateModel(string model, string texture, bool isActive, Vector3 pos)
        {
            ModelComponent m = new ModelComponent()
            {
                IsActive = isActive,
                ModelPath = model,
                TexturePath = texture
            };

            TransformComponent t = new TransformComponent()
            {
                Position = pos
            };

            return cm.AddEntityWithComponents(m, t);
        }

        public static int CreateGrassBox(GraphicsDevice gd, Vector3 corner1, Vector3 corner2)
        {
            Vector3 position = Vector3.Lerp(corner1, corner2, 0.5f);
            corner2 = (corner2 - corner1) / 2;
            corner1 = -corner2;

            //TransformComponent transform = new TransformComponent() { Position = position };
            TransformComponent transform = new TransformComponent() { Position = new Vector3(97.03926f, 259.7693f, -371.4293f) };

            RectangleComponent r = new RectangleComponent(gd, corner1, corner2) { TexturePath = "grass" };
            int cube = cm.AddEntityWithComponents(transform, r);
            return cube;
        }
    }
}
