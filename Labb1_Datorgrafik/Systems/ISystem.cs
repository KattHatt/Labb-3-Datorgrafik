using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb1_Datorgrafik
{
    public interface ISystem
    {
        void Start();
        void Update(GameTime gametime);
    }
}
