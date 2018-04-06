using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Labb1_Datorgrafik.Managers;
using Labb1_Datorgrafik.Components;
using Labb1_Datorgrafik.Tools;

namespace Labb1_Datorgrafik.Systems
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
            foreach(var tracker in cm.GetComponentsOfType<TrackingCameraComponent>())
            {
                TrackingCameraComponent trackComp = (TrackingCameraComponent)tracker.Value;
                TransformComponent trackPos = cm.GetComponentForEntity<TransformComponent>(trackComp.Target);
                TransformComponent camPos = cm.GetComponentForEntity<TransformComponent>(tracker.Key);
                CameraComponent camComp = cm.GetComponentForEntity<CameraComponent>(tracker.Key);

                Matrix rotationMatrix = Matrix.CreateRotationY(trackPos.Rotation.X);
                Vector3 transformedOffset = Vector3.Transform(trackComp.Offset, rotationMatrix);
                camPos.Position = trackPos.Position + transformedOffset;
                camPos.Rotation = trackPos.Position - camPos.Position;
            }
        }
    }
}
