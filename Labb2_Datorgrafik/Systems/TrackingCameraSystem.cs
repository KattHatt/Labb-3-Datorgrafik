using Labb2_Datorgrafik.Components;
using Labb2_Datorgrafik.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Labb2_Datorgrafik.Systems
{
    public class TrackingCameraSystem : ISystem
    {
        ComponentManager cm = ComponentManager.GetInstance();

        public void Load(ContentManager content)
        {
        }

        public void Update(GameTime gametime)
        {
            foreach(var (_, trackingCamera, cameraTransform, camera) in cm.GetComponentsOfType<TrackingCameraComponent, TransformComponent, CameraComponent>())
            {
                TransformComponent targetTransform = cm.GetComponentForEntity<TransformComponent>(trackingCamera.Target);

                Matrix rotationMatrix = Matrix.CreateRotationY(targetTransform.Rotation.X);
                Vector3 transformedOffset = Vector3.Transform(trackingCamera.Offset, rotationMatrix);
                cameraTransform.Position = targetTransform.Position + transformedOffset;
                cameraTransform.Rotation = targetTransform.Position - cameraTransform.Position;
            }
        }
    }
}
