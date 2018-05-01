﻿using Engine.Components;
using Engine.Managers;
using Microsoft.Xna.Framework;

namespace Engine.Systems
{
    public class AnimationSystem : ISystem
    {
        public void Update(GameTime gametime)
        {
            ComponentManager cm = ComponentManager.GetInstance();

            foreach (var (_, animComp, body, bTrans) in cm.GetComponentsOfType<AnimationComponent, RectangleComponent, TransformComponent>())
            {
                if (animComp.Animate)
                {
                    TransformComponent rightArm = null, leftArm = null, rightLeg = null, leftLeg = null;

                    foreach (int child in body.Children)
                    {
                        NameComponent name = cm.GetComponentForEntity<NameComponent>(child);

                        switch (name.Name)
                        {
                            case "RightLegJoint":
                                rightLeg = cm.GetComponentForEntity<TransformComponent>(child);
                                break;
                            case "LeftLegJoint":
                                leftLeg = cm.GetComponentForEntity<TransformComponent>(child);
                                break;
                            case "RightArmJoint":
                                rightArm = cm.GetComponentForEntity<TransformComponent>(child);
                                break;
                            case "LeftArmJoint":
                                leftArm = cm.GetComponentForEntity<TransformComponent>(child);
                                break;
                            default:
                                break;
                        }
                    }

                    if (animComp.Reverse)
                        animComp.Progress -= (float)gametime.ElapsedGameTime.TotalSeconds * 2;
                    else
                        animComp.Progress += (float)gametime.ElapsedGameTime.TotalSeconds * 2;

                    if (animComp.Progress >= 1)
                    {
                        animComp.Reverse = true;
                    }
                    else if (animComp.Progress <= -1)
                    {
                        animComp.Reverse = false;
                    }

                    if (rightLeg != null)
                        rightLeg.Rotation.Y = animComp.Progress;
                    if (leftLeg != null)
                        leftLeg.Rotation.Y = -animComp.Progress;
                    if (leftArm != null)
                        leftArm.Rotation.Y = animComp.Progress;
                    if (rightArm != null)
                        rightArm.Rotation.Y = -animComp.Progress;
                }
            }
        }
    }
}