using System;
using Microsoft.Xna.Framework;
using Labb2_Datorgrafik.Managers;
using Labb2_Datorgrafik.Components;

namespace Labb2_Datorgrafik.Systems
{
    class TransformSystem : ISystem
    {
        public void Update(GameTime gametime)
        {
            ComponentManager cm = ComponentManager.GetInstance();
            foreach (var entity in cm.GetComponentsOfType<TransformComponent>())
            {
                TransformComponent transform = (TransformComponent)entity.Value;
                transform.World =
                    Matrix.CreateScale(transform.Scale) *
                    Matrix.CreateFromYawPitchRoll(transform.Rotation.X, transform.Rotation.Y, transform.Rotation.Z) *
                    Matrix.CreateTranslation(transform.Position);
            }
        }
    }
}
