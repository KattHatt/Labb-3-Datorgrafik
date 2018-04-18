using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Labb2_Datorgrafik
{
    public static class Extensions
    {
        public static void Deconstruct<TKey, TValue>(this KeyValuePair<TKey, TValue> source, out TKey key, out TValue value)
        {
            key = source.Key;
            value = source.Value;
        }

        /**
         * Möller-Trumbore intersection algorithm, taken from http://xbox.create.msdn.com/en-US/education/catalog/sample/picking_triangle
         */
        public static float? Intersects(this Ray ray, Vector3 vertex1, Vector3 vertex2, Vector3 vertex3)
        {
            // Compute vectors along two edges of the triangle.
            Vector3 edge1, edge2;

            Vector3.Subtract(ref vertex2, ref vertex1, out edge1);
            Vector3.Subtract(ref vertex3, ref vertex1, out edge2);

            // Compute the determinant.
            Vector3 directionCrossEdge2;
            Vector3.Cross(ref ray.Direction, ref edge2, out directionCrossEdge2);

            float determinant;
            Vector3.Dot(ref edge1, ref directionCrossEdge2, out determinant);

            // If the ray is parallel to the triangle plane, there is no collision.
            if (determinant > -float.Epsilon && determinant < float.Epsilon)
                return null;

            float inverseDeterminant = 1.0f / determinant;

            // Calculate the U parameter of the intersection point.
            Vector3 distanceVector;
            Vector3.Subtract(ref ray.Position, ref vertex1, out distanceVector);

            float u;
            Vector3.Dot(ref distanceVector, ref directionCrossEdge2, out u);
            u *= inverseDeterminant;

            // Make sure it is inside the triangle.
            if (u < 0 || u > 1)
                return null;

            // Calculate the V parameter of the intersection point.
            Vector3 distanceCrossEdge1;
            Vector3.Cross(ref distanceVector, ref edge1, out distanceCrossEdge1);

            float v;
            Vector3.Dot(ref ray.Direction, ref distanceCrossEdge1, out v);
            v *= inverseDeterminant;

            // Make sure it is inside the triangle.
            if (v < 0 || u + v > 1)
                return null;

            // Compute the distance along the ray to the triangle.
            float rayDistance;
            Vector3.Dot(ref edge2, ref distanceCrossEdge1, out rayDistance);
            rayDistance *= inverseDeterminant;

            // Is the triangle behind the ray origin?
            if (rayDistance < 0)
                return null;

            return rayDistance;
        }
    }
}