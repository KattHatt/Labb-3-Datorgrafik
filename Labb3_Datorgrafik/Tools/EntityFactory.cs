﻿using Engine.Components;
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

        public static int CreateGrassBox(GraphicsDevice gd, Vector3 corner1, Vector3 corner2)
        {
            Vector3 position = Vector3.Lerp(corner1, corner2, 0.5f);
            corner2 = (corner2 - corner1) / 2;
            corner1 = -corner2;

            TransformComponent transform = new TransformComponent() { Position = position };
            BoxComponent r = new BoxComponent(gd, corner1, corner2) { TexturePath = "grass" };
            int cube = cm.AddEntityWithComponents(transform, r);
            return cube;
        }

        public static void CreateModel(string model, string texture, Vector3 position)
        {
            TransformComponent transform = new TransformComponent() { Position = position };
            ModelComponent modelComp = new ModelComponent(model, texture);

            cm.AddEntityWithComponents(transform, modelComp);
        }
    }
}
