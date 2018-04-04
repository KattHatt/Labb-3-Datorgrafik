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
                    switch (nameComp.Name)
                    {
                        case "Chopper":

                            // Move chopper on x,y,z axis

                            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                            {
                                transComp.Position = new Vector3(transComp.Position.X - 1f, transComp.Position.Y, transComp.Position.Z);
                            }
                            if (Keyboard.GetState().IsKeyDown(Keys.Right))
                            {
                                transComp.Position = new Vector3(transComp.Position.X + 1f, transComp.Position.Y, transComp.Position.Z);
                            }
                            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                            {
                                transComp.Position = new Vector3(transComp.Position.X, transComp.Position.Y - 1f, transComp.Position.Z);
                            }
                            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                            {
                                transComp.Position = new Vector3(transComp.Position.X, transComp.Position.Y + 1f, transComp.Position.Z);
                            }
                            if (Keyboard.GetState().IsKeyDown(Keys.OemPlus))
                            {
                                transComp.Position = new Vector3(transComp.Position.X, transComp.Position.Y, transComp.Position.Z + 1f);
                            }
                            if (Keyboard.GetState().IsKeyDown(Keys.OemMinus))
                            {
                                transComp.Position = new Vector3(transComp.Position.X, transComp.Position.Y, transComp.Position.Z - 1f);
                            }

                            // Update top and back rotor on chopper.. (TODO)

                            // Top rotor
                            modelComp.Model.Bones[1].Transform *= Matrix.CreateRotationY(1f * (float)gametime.ElapsedGameTime.TotalSeconds);

                            // Back rotor
                            modelComp.Model.Bones[3].Transform *= Matrix.CreateRotationX(1f * (float)gametime.ElapsedGameTime.TotalSeconds);


                            break;
                        default:
                            throw new Exception("unrecognized name in name component: " + nameComp.ToString());
                    }
                }
            }
        }
    }
}
