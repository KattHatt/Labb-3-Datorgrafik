using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Labb1_Datorgrafik.Managers;

namespace Labb1_Datorgrafik.Systems
{
    public class TrackingCameraSystem : ISystem, IRender
    {
        ComponentManager cm = ComponentManager.GetInstance();

        public void Load(ContentManager content)
        {
        }

        public void Render(GraphicsDevice gd, BasicEffect be)
        {
        }

        public void Update(GameTime gametime)
        {
        }
    }
}
