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

        public static int CreateCamera(GraphicsDevice gd)
        {
            CameraComponent camera = new CameraComponent()
            {
                Position = new Vector3(-70, 315, -340),
                Up = Vector3.Up,
                Direction = Vector3.Right,
                FieldOfView = 45,
                NearPlaneDistance = 1,
                FarPlaneDistance = 10000,
                AspectRatio = gd.DisplayMode.AspectRatio,
            };

            camera.Pitch(-20);
            camera.RotateY(20);

            return cm.AddEntityWithComponents(camera);
        }

        public static int CreateGrassBox(GraphicsDevice gd, Vector3 corner1, Vector3 corner2)
        {
            Vector3 position = Vector3.Lerp(corner1, corner2, 0.5f) + new Vector3(100, -270, 1000);
            corner2 = (corner2 - corner1) / 2;
            corner1 = -corner2;

            TransformComponent transform = new TransformComponent() { Position = position };
            BoxComponent r = new BoxComponent(gd, corner1, corner2) { TexturePath = "grass" };
            int cube = cm.AddEntityWithComponents(transform, r);
            return cube;
        }

        public static void CreateModel(string model, Vector3 position)
        {
            TransformComponent transform = new TransformComponent() { Position = position };
            ModelComponent modelComp = new ModelComponent(model);

            cm.AddEntityWithComponents(transform, modelComp);
        }

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
    }
}
