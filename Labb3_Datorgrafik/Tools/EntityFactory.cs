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
            ShadowMapComponent shadow = new ShadowMapComponent()
            {
                EffectName = "ShadowMap",
                TextureName = "grass",
                Ambient = 0.2f,
                LightPos = new Vector3(0, 100, 0),
                LightPower = 1.0f
            };
            shadow.LightsView = Matrix.CreateLookAt(shadow.LightPos, new Vector3(-2, 3, -10), new Vector3(0, 1, 0));
            shadow.LightsProjection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver2, 1f, 5f, 100f);

            cm.AddEntityWithComponents(shadow);
        }

        public static int CreateDirLight()
        {
            DirLightComponent spot = new DirLightComponent()
            {
                TextureName = "lich_knight_sprite__by_epicspiderboss",
                EffectName = "DirLight",
                LightPos = new Vector3(0, 300, 0),
                LightPower = 1.0f,
                AmbientPower = 0.2f,
            };

            return cm.AddEntityWithComponents(spot);
        }

        public static int CreateCamera(GraphicsDevice gd)
        {
            CameraComponent camera = new CameraComponent()
            {
                Position = new Vector3(0, 20, 0),
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
            TransformComponent transform = new TransformComponent() { Position = new Vector3(0, 0, 0) };

            RectangleComponent r = new RectangleComponent(gd, corner1, corner2) { TexturePath = "grass" };
            int cube = cm.AddEntityWithComponents(transform, r);
            return cube;
        }

        public static void CreateGround(GraphicsDevice gd)
        {
            TransformComponent t = new TransformComponent() { Position = new Vector3(0, 0, 0) };
            RectangleComponent ground = new RectangleComponent(gd, 200, 200, 200, "grass");
        }

        public static void CreateCubeR(GraphicsDevice gd, int height, int width, int depth)
        {
            TransformComponent t = new TransformComponent() { Position = new Vector3(0, 10, 0) };
            RectangleComponent cube1 = new RectangleComponent(gd, height, width, depth, "rogert");
        }
    }
}
