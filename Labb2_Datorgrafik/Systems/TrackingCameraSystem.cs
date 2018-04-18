using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Labb2_Datorgrafik.Managers;
using Labb2_Datorgrafik.Components;
using Labb2_Datorgrafik.Tools;

namespace Labb2_Datorgrafik.Systems
{
    public class TrackingCameraSystem : ISystem, IRender
    {
        ComponentManager cm = ComponentManager.GetInstance();

        public void Load(ContentManager content)
        {
        }

        public void Render(GraphicsDevice gd, BasicEffect be)
        {
        }

        public void Update(GameTime gametime)
        {
            foreach(var (_, trackComp, camPos, camComp) in cm.GetComponentsOfType<TrackingCameraComponent, TransformComponent, CameraComponent>())
            {
                TransformComponent trackPos = cm.GetComponentForEntity<TransformComponent>(trackComp.Target);

                Matrix rotationMatrix = Matrix.CreateRotationY(trackPos.Rotation.X);
                Vector3 transformedOffset = Vector3.Transform(trackComp.Offset, rotationMatrix);
                camPos.Position = trackPos.Position + transformedOffset;
                camPos.Rotation = trackPos.Position - camPos.Position;
            }
        }
    }
}
