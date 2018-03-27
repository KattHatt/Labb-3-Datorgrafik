using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb1_Datorgrafik
{
    public static class EntityFactory
    {
        public static int CreateCamera(GraphicsDevice gd)
        {
            ComponentManager cm = ComponentManager.GetInstance();

            int cam = cm.AddEntityWithComponents( new IComponent[] { new CameraComponent(gd), new TransformComponent() });
            TransformComponent transform = cm.GetComponentForEntity<TransformComponent>(cam);
            CameraComponent camComp = cm.GetComponentForEntity<CameraComponent>(cam);
            Matrix.CreateLookAt(ref transform.Position, ref camComp.Target, ref camComp.UpVector, out camComp.ViewMatrix);

            return cam;
        }
    }
}
