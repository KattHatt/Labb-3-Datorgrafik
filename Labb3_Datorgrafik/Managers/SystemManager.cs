using Labb3_Datorgrafik.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Labb3_Datorgrafik.Managers
{
    class SystemManager
    {
        private Dictionary<Type, object> systems;
        private static SystemManager systemManagerInstance;

        static SystemManager()
        {
            systemManagerInstance = new SystemManager();
        }

        private SystemManager()
        {
            systems = new Dictionary<Type, object>();
        }

        public static SystemManager GetInstance()
        {
            return systemManagerInstance;
        }

        public T GetSystem<T>()
        {
            object system;
            systems.TryGetValue(typeof(T), out system);
            return (T)system;
        }

        public void Load<T>(ContentManager content)
        {
            object system;
            systems.TryGetValue(typeof(T), out system);
            ((ILoad)system)?.Load(content);
        }

        public void Update<T>(GameTime gameTime)
        {
            object system;
            systems.TryGetValue(typeof(T), out system);
            ((ISystem)system)?.Update(gameTime);
        }

        public void Render<T>(GraphicsDevice gd, BasicEffect be)
        {
            object system;
            systems.TryGetValue(typeof(T), out system);
            ((IRender)system)?.Render(gd, be);
        }

        public void AddSystem(object system)
        {
            systems.Add(system.GetType(), system);
        }

        public void RemoveSystem(object system)
        {
            if (systems.ContainsKey(system.GetType()))
                systems.Remove(system.GetType());
        }
    }
}
