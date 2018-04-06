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
                        double speedx = (Math.Sin(transComp.Rotation.X));
                        double speedz = (Math.Cos(transComp.Rotation.X));
                        float speedxdouble, speedzdouble;
                        speedxdouble = (float)speedx;
                        speedzdouble = (float)speedz;

                        if (Keyboard.GetState().IsKeyDown(Keys.Q))
                        {
                            transComp.Position += Vector3.Left * speedzdouble;
                            transComp.Position += Vector3.Backward * speedxdouble;
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.E))
                        {
                            transComp.Position += Vector3.Right * speedzdouble;
                            transComp.Position += Vector3.Forward * speedxdouble;
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.R))
                        {
                            transComp.Position += Vector3.Up;
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.F))
                        {
                            transComp.Position += Vector3.Down;
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.W))
                        {
                            transComp.Position += Vector3.Forward * speedzdouble;
                            transComp.Position += Vector3.Left * speedxdouble;
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.S))
                        {
                            transComp.Position += Vector3.Backward;
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.A))
                        {
                            transComp.Rotation.X += .03f;
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.D))
                        {
                            transComp.Rotation.X -= .03f;
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.Add))
                        {
                            transComp.Scale *= 1.1f;
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.Subtract))
                        {
                            transComp.Scale *= 0.9f;
                        }

                        // Rotate the heli rotors

                        // Top rotor
                        modelComp.Model.Bones[1].Transform = ModelHelper.rotateModel(modelComp.Model.Bones[1].Transform, Vector3.Up, gametime.ElapsedGameTime.Milliseconds);

                        // Back rotor
                        modelComp.Model.Bones[3].Transform = ModelHelper.rotateModel(modelComp.Model.Bones[3].Transform, Vector3.Up, gametime.ElapsedGameTime.Milliseconds);
                    }
                }
            }
        }        
    }
}