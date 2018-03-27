using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Labb1_Datorgrafik
{
    public interface IRender
    {
        void Render(GraphicsDevice gd, BasicEffect be);
    }
}
