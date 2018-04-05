using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb1_Datorgrafik.Components
{
    public class TrackingCameraComponent : IComponent
    {
        public int Target;
        public Vector3 Offset;
        public float MaxDegreeSpeed;
        public float Fov;
        public float nearClipping;
        public float farClipping;

        public TrackingCameraComponent(int target, Vector3 offset)
        {
            Target = target;
            Offset = offset;
        }
    }
}
