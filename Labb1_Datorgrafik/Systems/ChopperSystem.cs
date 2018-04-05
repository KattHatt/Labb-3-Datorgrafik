using System;
using Microsoft.Xna.Framework;
using Labb1_Datorgrafik.Managers;
using Labb1_Datorgrafik.Components;
using Microsoft.Xna.Framework.Input;
using Labb1_Datorgrafik.Tools;

namespace Labb1_Datorgrafik.Systems
{
    public class ChopperSystem : ISystem
    {
        ModelHelper mh = new ModelHelper();

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
                        modelComp.Model.Bones[1].Transform = mh.rotateModel(modelComp.Model.Bones[1].Transform, Vector3.Up, gametime.ElapsedGameTime.Milliseconds);

                        // Back rotor
                        modelComp.Model.Bones[3].Transform = mh.rotateModel(modelComp.Model.Bones[3].Transform, Vector3.Up, gametime.ElapsedGameTime.Milliseconds);
                    }
                }
            }
        }        
    }
}