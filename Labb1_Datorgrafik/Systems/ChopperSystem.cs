using System;
using Microsoft.Xna.Framework;
using Labb1_Datorgrafik.Managers;
using Labb1_Datorgrafik.Components;
using Microsoft.Xna.Framework.Input;

namespace Labb1_Datorgrafik.Systems
{
    public class ChopperSystem : ISystem
    {
        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime gametime)
        {
            ComponentManager cm = ComponentManager.GetInstance();

            foreach (var model in cm.GetComponentsOfType<ModelComponent>())
            {
                ModelComponent modelComp = (ModelComponent)model.Value;
                if (modelComp.IsActive && cm.GetComponentForEntity<NameComponent>(model.Key) != null)
                {
                    NameComponent nameComp = cm.GetComponentForEntity<NameComponent>(model.Key);
                    TransformComponent transComp = cm.GetComponentForEntity<TransformComponent>(model.Key);
                    if (nameComp.Name == "Chopper")
                    {
                        // Move chopper on x,y,z axis

                        if (Keyboard.GetState().IsKeyDown(Keys.Left))
                        {
                            transComp.Position += Vector3.Left;
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.Right))
                        {
                            transComp.Position += Vector3.Right;
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.Up))
                        {
                            transComp.Position += Vector3.Up;
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.Down))
                        {
                            transComp.Position += Vector3.Down;
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.OemPlus))
                        {
                            transComp.Position += Vector3.Forward;
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.OemMinus))
                        {
                            transComp.Position += Vector3.Backward;
                        }

                        // Rotate the heli rotors

                        // Top rotor
                        modelComp.Model.Bones[1].Transform *= Matrix.CreateRotationY(1f * (float)gametime.ElapsedGameTime.TotalSeconds);

                        // Back rotor

                        modelComp.Model.Bones[3].Transform *= Matrix.CreateRotationX(-1f * (float)gametime.ElapsedGameTime.TotalSeconds);


                    }
                }
            }
        }
    }
}
