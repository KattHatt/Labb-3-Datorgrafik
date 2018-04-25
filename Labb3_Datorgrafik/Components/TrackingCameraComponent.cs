using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb3_Datorgrafik.Components
{
    public class TrackingCameraComponent : IComponent
    {
        public int Target;
        public Vector3 Offset;

        public TrackingCameraComponent(int target, Vector3 offset)
        {
            Target = target;
            Offset = offset;
        }
    }
}
