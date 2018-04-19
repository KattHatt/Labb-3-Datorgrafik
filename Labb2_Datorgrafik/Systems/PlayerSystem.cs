using Labb2_Datorgrafik.Components;
using Labb2_Datorgrafik.Managers;
using Labb2_Datorgrafik.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Labb1_Datorgrafik.Systems
{
    public class PlayerSystem : ISystem
    {
        ComponentManager cm = ComponentManager.GetInstance();
        int leftLegID;
        int rightLegID;
        int heightMapID;

        public void Update(GameTime gametime)
        {

            foreach (var nam in cm.GetComponentsOfType<NameComponent>())
            {
                NameComponent n = nam.Item2;
                if (n.Name == "LeftLeg")
                    leftLegID = nam.Item1;
                else if (n.Name == "RightLeg")
                    rightLegID = nam.Item1;
            }

            foreach (var h in cm.GetComponentsOfType<HeightMapComponent>())
            {
                heightMapID = h.Item1;
            }

            foreach (var (_,nameComp, transComp) in cm.GetComponentsOfType<NameComponent, TransformComponent>())
            {
                if (nameComp.Name == "Body")
                {
                    double speedx = (Math.Sin(transComp.Rotation.X));
                    double speedz = (Math.Cos(transComp.Rotation.X));
                    float speedxdouble, speedzdouble;
                    speedxdouble = (float)speedx;
                    speedzdouble = (float)speedz;

                    if (Keyboard.GetState().IsKeyDown(Keys.W))
                    {
                        transComp.Position += Vector3.Forward * speedzdouble;
                        transComp.Position += Vector3.Left * speedxdouble;
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.S))
                    {
                        transComp.Position += Vector3.Backward * speedzdouble;
                        transComp.Position += Vector3.Right * speedxdouble;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.A))
                    {
                        transComp.Rotation.X += .03f;
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.D))
                    {
                        transComp.Rotation.X -= .03f;
                    }

                    TiltModelAccordingToTerrain(heightMapID, leftLegID, rightLegID);
                }
            }
        }


        private void TiltModelAccordingToTerrain(int heightMapID, int leftLegID, int rightLegID)
        {
            ComponentManager cm = ComponentManager.GetInstance();

            TransformComponent leftTransComp = cm.GetComponentForEntity<TransformComponent>(leftLegID);
            TransformComponent rightTransComp = cm.GetComponentForEntity<TransformComponent>(rightLegID);

            RectangleComponent child = cm.GetComponentForEntity<RectangleComponent>(leftLegID);
            RectangleComponent rootRect = cm.GetComponentForEntity<RectangleComponent>((int)child.Root);

            TransformComponent rootTransComp = cm.GetComponentForEntity<TransformComponent>((int)child.Root);

            float[,] heightData = cm.GetComponentForEntity<HeightMapComponent>(heightMapID).HeightData;

            Vector3 leftLegOrigin = leftTransComp.Position;
            Vector3 rightLegOrigin = rightTransComp.Position;

            Matrix leftLegMatrix = leftTransComp.World;
            Matrix rightLegMatrix = rightTransComp.World;

            Vector3 leftLeg = Vector3.Transform(leftLegOrigin, leftLegMatrix * Matrix.Identity);
            Vector3 rightLeg = Vector3.Transform(rightLegOrigin, rightLegMatrix * Matrix.Identity);

            Vector3 middle = leftLeg + rightLeg / 2.0f;

            float leftLegHeight = GetExactHeightAt(leftLeg.X, -leftLeg.Z, heightData);
            float rightLegHeight = GetExactHeightAt(rightLeg.X, -rightLeg.Z, heightData);
            float lrHeightDiff = leftLegHeight - rightLegHeight;

            float lrAngle = (float)Math.Atan2(lrHeightDiff, middle.Length());
            Quaternion lrRot = Quaternion.CreateFromAxisAngle(new Vector3(1, 0, 0), -lrAngle);

            Matrix rotatedModelWorld = Matrix.CreateFromQuaternion(lrRot) * Matrix.Identity;

            Vector3 rotLeft = Vector3.Transform(leftLegOrigin, leftLegMatrix * rotatedModelWorld);
            Vector3 rotRight = Vector3.Transform(rightLegOrigin, rightLegMatrix * rotatedModelWorld);

            float lTerHeight = GetExactHeightAt(rotLeft.X, -rotLeft.Z, heightData);
            float rTerHeight = GetExactHeightAt(rotRight.X, -rotRight.Z, heightData);

            float lHeightDiff = rotLeft.Y - lTerHeight;
            float rHeightDiff = rotRight.Y - rTerHeight;

            float finalHeightDiff = (lHeightDiff + rHeightDiff) / 2.0f;

            Matrix worldMatrix = rotatedModelWorld * Matrix.CreateTranslation(new Vector3(0, -finalHeightDiff, 0));


            // mixtra med dessa
            rootTransComp.World = worldMatrix;
            leftTransComp.World = worldMatrix;
            rightTransComp.World = worldMatrix;
            
        }

        // Gets the exact height at a sertain position in a heightmap texture
        public float GetExactHeightAt(float x, float z, float[,] heightData)
        {
            bool invalid = x < 0;
            invalid |= z < 0;
            invalid |= x > heightData.GetLength(0) - 1;
            invalid |= z > heightData.GetLength(1) - 1;
            if (invalid)
                return 10.0f;

            int xLower = (int)x;
            int xHigher = xLower + 1;
            float xRelative = (x - xLower) / ((float)xHigher - (float)xLower);

            int zLower = (int)z;
            int zHigher = zLower + 1;
            float zRelative = (z - zLower) / ((float)zHigher - (float)zLower);

            float heightLxLz = heightData[xLower, zLower];
            float heightLxHz = heightData[xLower, zHigher];
            float heightHxLz = heightData[xHigher, zLower];
            float heightHxHz = heightData[xHigher, zHigher];

            bool pointAboveLowerTriangle = (xRelative + zRelative < 1);

            float finalHeight;
            if (pointAboveLowerTriangle)
            {
                finalHeight = heightLxLz;
                finalHeight += zRelative * (heightLxHz - heightLxLz);
                finalHeight += xRelative * (heightHxLz - heightLxLz);
            }
            else
            {
                finalHeight = heightHxHz;
                finalHeight += (1.0f - zRelative) * (heightHxLz - heightHxHz);
                finalHeight += (1.0f - xRelative) * (heightLxHz - heightHxHz);
            }

            return finalHeight;    
        }
    }
}
