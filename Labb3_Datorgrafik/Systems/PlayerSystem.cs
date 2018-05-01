using Labb3_Datorgrafik.Components;
using Labb3_Datorgrafik.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace Labb3_Datorgrafik.Systems
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

            foreach (var (id, nameComp, transComp) in cm.GetComponentsOfType<NameComponent, TransformComponent>())
            {
                if (nameComp.Name == "Body")
                {
                    double speedx = (Math.Sin(transComp.Rotation.X));
                    double speedz = (Math.Cos(transComp.Rotation.X));
                    float speedxdouble, speedzdouble;
                    speedxdouble = (float)speedx * 0.2f;
                    speedzdouble = (float)speedz * 0.2f;

                    AnimationComponent animComp = cm.GetComponentForEntity<AnimationComponent>(id);

                    animComp.Animate = false;

                    if (Keyboard.GetState().IsKeyDown(Keys.W))
                    {
                        transComp.Position += Vector3.Forward * speedzdouble;
                        transComp.Position += Vector3.Left * speedxdouble;
                        animComp.Animate = true;
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.S))
                    {
                        transComp.Position += Vector3.Backward * speedzdouble;
                        transComp.Position += Vector3.Right * speedxdouble;
                        animComp.Animate = true;
                    }

                    if (Keyboard.GetState().IsKeyDown(Keys.A))
                    {
                        transComp.Rotation.X += .03f;
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.D))
                    {
                        transComp.Rotation.X -= .03f;
                    }

                    if (Keyboard.GetState().IsKeyDown(Keys.Z))
                    {
                        transComp.Scale -= Vector3.One * 0.03f;
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.X))
                    {
                        transComp.Scale += Vector3.One * 0.03f;
                    }


                    TiltModelAccordingToTerrain(heightMapID, id, leftLegID);
                }
            }
        }

        private void TiltModelAccordingToTerrain(int heightMapID, int bodyID, int legID)
        {
            var bodyTransform = cm.GetComponentForEntity<TransformComponent>(bodyID);
            var heightmap = cm.GetComponentForEntity<HeightMapComponent>(heightMapID);
            var legTransform = cm.GetComponentForEntity<TransformComponent>(legID);
            var legRectangle = cm.GetComponentForEntity<RectangleComponent>(legID);

            Vector3 position = bodyTransform.Position;
            position.Y += 9.5f;
            
            Ray ray = new Ray(position, Vector3.Down);

            int? index = GetIntersectingBoxIndex(heightmap, ray);
            if (!index.HasValue)
                return;

            float? distance = GetIntersectingVertexDistance(heightmap, index.Value, ray);
            if (!distance.HasValue)
                return;

            Console.WriteLine(distance.Value);

            bodyTransform.Position.Y -= distance.Value;
            bodyTransform.Position.Y += 10.5f;
        }

        private int? GetIntersectingBoxIndex(HeightMapComponent heightmap, Ray ray)
        {
            for (int i = 0; i < heightmap.BoundingBoxes.Length; i++)
            {
                if (ray.Intersects(heightmap.BoundingBoxes[i]).HasValue)
                    return i;
            }
            return null;
        }

        private float? GetIntersectingVertexDistance(HeightMapComponent heightmap, int index, Ray ray)
        {
            Vector3[] vertices = heightmap.Vertices[index];
            int[] indices = heightmap.Indices[index];
            
            for (int i = 0; i < indices.Length; i += 3)
            {
                float? distance = ray.Intersects(vertices[indices[i]], vertices[indices[i + 1]], vertices[indices[i + 2]]);
                if (distance.HasValue)
                    return distance;
            }
            return null;
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
