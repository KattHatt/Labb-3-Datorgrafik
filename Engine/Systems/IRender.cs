using Microsoft.Xna.Framework.Graphics;

namespace Engine.Systems
{
    public interface IRender
    {
        void Render(GraphicsDevice gd, Effect e, string technique);
    }
}
