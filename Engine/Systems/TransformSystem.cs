using Engine.Components;
using Engine.Managers;
using Microsoft.Xna.Framework;

namespace Engine.Systems
{
    public class TransformSystem : ISystem
    {
        public void Update(GameTime gametime)
        {
            ComponentManager cm = ComponentManager.GetInstance();
            foreach (var (_, transform) in cm.GetComponentsOfType<TransformComponent>())
            {
                transform.World =
                    Matrix.CreateScale(transform.Scale) *
                    Matrix.CreateFromYawPitchRoll(transform.Rotation.X, transform.Rotation.Y, transform.Rotation.Z) *
                    Matrix.CreateTranslation(transform.Position);
            }
        }
    }
}
