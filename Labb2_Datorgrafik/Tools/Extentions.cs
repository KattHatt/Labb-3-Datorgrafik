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

        public static float? Intersects(this Ray ray, Vector3 v0, Vector3 v1, Vector3 v2)
        {


            return null;
        }
    }
}