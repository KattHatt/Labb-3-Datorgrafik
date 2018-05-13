using Microsoft.Xna.Framework.Graphics;

namespace Engine.Systems
{
    public interface IRender
    {
        void Render(GraphicsDevice gd);
        void RenderShadow(GraphicsDevice gd, Effect e);
    }
}
