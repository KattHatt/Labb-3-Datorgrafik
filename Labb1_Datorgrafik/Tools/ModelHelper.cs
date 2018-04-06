using Microsoft.Xna.Framework;

namespace Labb1_Datorgrafik.Tools
{
    public static class ModelHelper
    {
        // Rotates a model on the given axis the number of degrees
        public static Matrix rotateModel(Matrix toRotate, Vector3 axis, float degrees)
        {
            Matrix old = toRotate;

            return Matrix.CreateTranslation(Vector3.Zero) * Matrix.CreateFromAxisAngle(axis, degrees) * old;
        }

        public static Matrix rotateAroundPoint(Matrix toRotate, Matrix pointOfRotation, Vector3 axis, float degrees)
        {
            return Matrix.CreateTranslation(Vector3.Zero) * Matrix.CreateFromAxisAngle(axis, degrees) * pointOfRotation;
        }
    }
}
