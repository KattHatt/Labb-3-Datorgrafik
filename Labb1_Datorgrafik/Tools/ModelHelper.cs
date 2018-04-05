using Microsoft.Xna.Framework;

namespace Labb1_Datorgrafik.Tools
{
    public class ModelHelper
    {
        // Rotates a model on the given axis the number of degrees
        public Matrix rotateModel(Matrix toRotate, Vector3 axis, float degrees)
        {
            Matrix old = toRotate;

            return Matrix.CreateTranslation(Vector3.Zero) * Matrix.CreateFromAxisAngle(axis, degrees) * old;
        }
    }
}
