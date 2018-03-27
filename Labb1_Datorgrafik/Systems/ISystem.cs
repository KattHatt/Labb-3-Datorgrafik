using Microsoft.Xna.Framework;

namespace Labb1_Datorgrafik.Systems
{
    public interface ISystem
    {
        void Start();
        void Update(GameTime gametime);
    }
}
