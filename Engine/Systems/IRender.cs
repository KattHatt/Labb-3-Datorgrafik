using Microsoft.Xna.Framework.Graphics;

namespace Engine.Systems
{
    public interface IRender
    {
        void Render(GraphicsDevice gd, BasicEffect be);
    }
}
