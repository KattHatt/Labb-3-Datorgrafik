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
    }
}
