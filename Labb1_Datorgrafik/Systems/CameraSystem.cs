using System;
using Microsoft.Xna.Framework;

namespace Labb1_Datorgrafik
{
    class CameraSystem : ISystem
    {
        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime gametime)
        {
            ComponentManager cm = ComponentManager.GetInstance();

            foreach(var entity in cm.GetComponentsOfType<CameraComponent>())
            {
                CameraComponent cam = (CameraComponent)entity.Value;

                // TODO
            }
        }
    }
}
