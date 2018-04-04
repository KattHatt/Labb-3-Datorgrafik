using Labb1_Datorgrafik.Components;
using Labb1_Datorgrafik.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Labb1_Datorgrafik.Tools
{
    public static class EntityFactory
    {
        public static int CreateCamera(GraphicsDevice gd)
        {
            ComponentManager cm = ComponentManager.GetInstance();

            int cam = cm.AddEntityWithComponents( new IComponent[] { new CameraComponent(gd), new TransformComponent() });
            TransformComponent transform = cm.GetComponentForEntity<TransformComponent>(cam);
            CameraComponent camComp = cm.GetComponentForEntity<CameraComponent>(cam);
            //Matrix.CreateLookAt(ref transform.Position, ref camComp.Target, ref camComp.UpVector, out camComp.ViewMatrix);
            camComp.ViewMatrix = Matrix.CreateLookAt(new Vector3(60, 80, -80), new Vector3(0, 0, 0), new Vector3(0, 1, 0));

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
            TransformComponent transComp = new TransformComponent();
            NameComponent nameComp = new NameComponent("Chopper");


            int chop = cm.AddEntityWithComponents(modComp, transComp, nameComp);

            return chop;
        }
    }
}
