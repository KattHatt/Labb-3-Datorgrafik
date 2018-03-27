using Microsoft.Xna.Framework;

namespace Labb1_Datorgrafik.Components
{
    public class WorldComponent : IComponent
    {
        public Matrix WorldMatrix { get; set; }

        public WorldComponent(Matrix world)
        {
            WorldMatrix = world;
        }
    }
}
