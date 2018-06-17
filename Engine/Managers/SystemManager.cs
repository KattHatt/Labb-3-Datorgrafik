using Engine.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Engine.Managers
{
    public class SystemManager
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

        public void Load(ContentManager content)
        {
            foreach (var system in systems.Values)
            {
                if (system is ILoad)
                    ((ILoad)system).Load(content);
            }
        }

        public void Init<T>(GraphicsDevice gd)
        {
            object system;
            systems.TryGetValue(typeof(T), out system);
            ((IInit)system)?.Init(gd);
        }

        public void Init(GraphicsDevice gd)
        {
            foreach (var system in systems.Values)
            {
                if (system is IInit)
                    ((IInit)system).Init(gd);
            }
        }

        public void Update<T>(GameTime gameTime)
        {
            object system;
            systems.TryGetValue(typeof(T), out system);
            ((ISystem)system)?.Update(gameTime);
        }

        public void Update(GameTime gameTime)
        {
            foreach (var system in systems.Values)
            {
                if (system is ISystem)
                    ((ISystem)system).Update(gameTime);
            }
        }

        public void Render<T>(GraphicsDevice gd, Effect e, string technique)
        {
            object system;
            systems.TryGetValue(typeof(T), out system);
            ((IRender)system)?.Render(gd, e, technique);
        }

        public void Render(GraphicsDevice gd, Effect e, string technique)
        {
            foreach (var system in systems.Values)
            {
                if (system is IRender)
                    ((IRender)system).Render(gd, e, technique);
            }
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
