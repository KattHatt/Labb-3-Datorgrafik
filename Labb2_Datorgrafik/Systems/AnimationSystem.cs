using Labb1_Datorgrafik.Components;
using Labb2_Datorgrafik.Components;
using Labb2_Datorgrafik.Managers;
using Labb2_Datorgrafik.Systems;
using Labb2_Datorgrafik.Tools;
using Microsoft.Xna.Framework;
using System;

public class AnimationSystem : ISystem
{
    public void Update(GameTime gametime)
    {
        ComponentManager cm = ComponentManager.GetInstance();

        foreach(var entity in cm.GetComponentsOfType<AnimationComponent>())
        {
            AnimationComponent animComp = (AnimationComponent)entity.Value;

            if (animComp.Animate)
            {
                RectangleComponent body = cm.GetComponentForEntity<RectangleComponent>(entity.Key);
                TransformComponent bTrans = cm.GetComponentForEntity<TransformComponent>(entity.Key);
                TransformComponent rightArm = null, leftArm = null, rightLeg = null, leftLeg = null;

                foreach(int child in body.Children)
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

                Console.WriteLine(animComp.Progress);

                if (animComp.Progress >= 1)
                {
                    animComp.Reverse = true;
                }
                else if(animComp.Progress <= -1)
                {
                    animComp.Reverse = false;
                }

                if (rightLeg != null)
                    rightLeg.Rotation.Y = animComp.Progress;
                if (leftLeg != null)
                    leftLeg.Rotation.Y = -animComp.Progress;
                if(leftArm != null)
                    leftArm.Rotation.Y = animComp.Progress;
                if(rightArm != null)
                    rightArm.Rotation.Y = -animComp.Progress;
            }
        }
    }
}
