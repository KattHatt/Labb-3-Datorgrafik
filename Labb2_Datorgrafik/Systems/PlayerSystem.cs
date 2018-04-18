using Labb2_Datorgrafik.Components;
using Labb2_Datorgrafik.Managers;
using Labb2_Datorgrafik.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Labb1_Datorgrafik.Systems
{
    public class PlayerSystem : ISystem
    {
        public void Update(GameTime gametime)
        {
            ComponentManager cm = ComponentManager.GetInstance();
            int leftLegID;
            int rightLegID;
            int heightMapID;

            foreach(var nam in cm.GetComponentsOfType<NameComponent>())
            {
                NameComponent n = nam.Item2;
                if (n.Name == "LeftLeg")
                    leftLegID = nam.Item1;
                else if (n.Name == "RightLeg")
                    rightLegID = nam.Item1; 
            }

            foreach(var h in cm.GetComponentsOfType<HeightMapComponent>())
            {
                heightMapID = h.Item1;
            }

            /// TODO ///
            /// use the "TiltModelAccordingToTerrain" 

        }


        private Matrix TiltModelAccordingToTerrain(int heightMapID, int leftLegID, int rightLegID)
        {
            ComponentManager cm = ComponentManager.GetInstance();

            TransformComponent leftTransComp = cm.GetComponentForEntity<TransformComponent>(leftLegID);
            TransformComponent rightTransComp = cm.GetComponentForEntity<TransformComponent>(rightLegID);
            HeightMapComponent hmc = cm.GetComponentForEntity<HeightMapComponent>(heightMapID);

            Vector3 leftLegOrigin = leftTransComp.Position;
            Vector3 rightLegOrigin = rightTransComp.Position;

            Matrix leftLegMatrix = leftTransComp.World;
            Matrix rightLegMatrix = rightTransComp.World;

            Vector3 leftLeg = Vector3.Transform(leftLegOrigin, leftLegMatrix * Matrix.Identity);
            Vector3 rightLeg = Vector3.Transform(rightLegOrigin, rightLegMatrix * Matrix.Identity);

            Vector3 middle = leftLeg + rightLeg / 2.0f;

            float leftLegHeight = GetExactHeightAt(leftLeg.X, -leftLeg.Z, heightMapID);
            float rightLegHeight = GetExactHeightAt(rightLeg.X, -rightLeg.Z, heightMapID);
            float lrHeightDiff = leftLegHeight - rightLegHeight;

            float lrAngle = (float)Math.Atan2(lrHeightDiff, middle.Length());
            Quaternion lrRot = Quaternion.CreateFromAxisAngle(new Vector3(1, 0, 0), -lrAngle);

            Matrix rotatedModelWorld = Matrix.CreateFromQuaternion(lrRot) * Matrix.Identity;

            Vector3 rotLeft = Vector3.Transform(leftLegOrigin, leftLegMatrix * rotatedModelWorld);
            Vector3 rotRight = Vector3.Transform(rightLegOrigin, rightLegMatrix * rotatedModelWorld);

            float lTerHeight = GetExactHeightAt(rotLeft.X, -rotLeft.Z, heightMapID);
            float rTerHeight = GetExactHeightAt(rotRight.X, -rotRight.Z, heightMapID);

            float lHeightDiff = rotLeft.Y - lTerHeight;
            float rHeightDiff = rotRight.Y - rTerHeight;

            float finalHeightDiff = (lHeightDiff + rHeightDiff) / 2.0f;

            Matrix worldMatrix = rotatedModelWorld * Matrix.CreateTranslation(new Vector3(0, -finalHeightDiff, 0));

            return worldMatrix;
        }

        public float GetExactHeightAt(float x, float z, int heightMapID)
        {
            ComponentManager cm = ComponentManager.GetInstance();

            Texture2D terrain = cm.GetComponentForEntity<HeightMapComponent>(heightMapID).Texture;

            float[,] heightData = GetHeightData(terrain);

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


        public float[,] GetHeightData(Texture2D heightmap)
        {
            float minimumHeight = 255;
            float maximumHeight = 0;

            int width = heightmap.Width;
            int height = heightmap.Height;

            Color[] heightMapColors = new Color[width * height];
            heightmap.GetData<Color>(heightMapColors);

            float[,] heightData = new float[width, height];

            // kan vara fel här, kanske ta bort / lägga till vingar
            for(int i = 0; i < width; i++)

                for (int j = 0; j < height; j++)
                {
                    heightData[i, j] = heightMapColors[i + j * width].R;
                    if (heightData[i, j] < minimumHeight) minimumHeight = heightData[i, j];
                    if (heightData[i, j] > maximumHeight) maximumHeight = heightData[i, j];
                }
            
            
            for(int i = 0; i < width; i++)
                for(int j = 0; j < height; j++)
                {
                    heightData[i, j] = (heightData[i, j] - minimumHeight) / 
                        (maximumHeight - minimumHeight) * 30.0f;
                }

            return heightData;
            
        }
    }
}
