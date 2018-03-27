using System;
using System.Collections.Generic;

namespace Labb1_Datorgrafik.Managers
{
    class ComponentManager
    {
        private Dictionary<int, Dictionary<Type, IComponent>> entityComponents;
        private Dictionary<Type, Dictionary<int, IComponent>> componentGroups;
        private static ComponentManager componentManagerInstance;

        static ComponentManager()
        {
            componentManagerInstance = new ComponentManager();
        }

        private ComponentManager()
        {
            entityComponents = new Dictionary<int, Dictionary<Type, IComponent>>();
            componentGroups = new Dictionary<Type, Dictionary<int, IComponent>>();
        }

        public static ComponentManager GetInstance()
        {
            return componentManagerInstance;
        }

        public int AddEntityWithComponents(params IComponent[] components)
        {
            int entity = GetEntityId();
            AddComponentsToEntity(entity, components);
            return entity;
        }

        public T GetComponentForEntity<T>(int entity) where T : IComponent
        {
            if (entityComponents.ContainsKey(entity))
            {
                var components = entityComponents[entity];
                IComponent component;
                components.TryGetValue(typeof(T), out component);
                return (T)component;
            }
            return default(T);
        }

        static int nextId;

        public static int GetEntityId()
        {
            return nextId++;
        }

        public void AddComponentsToEntity(int entity, params IComponent[] components)
        {
            if (!entityComponents.ContainsKey(entity) || entityComponents[entity] == null)
                entityComponents[entity] = new Dictionary<Type, IComponent>();

            foreach (IComponent component in components)
            {
                if (!componentGroups.ContainsKey(component.GetType()) || componentGroups[component.GetType()] == null)
                    componentGroups[component.GetType()] = new Dictionary<int, IComponent>();

                entityComponents[entity].Add(component.GetType(), component);
                componentGroups[component.GetType()][entity] = component;
            }
        }

        public Dictionary<int, IComponent> GetComponentsOfType<T>()
        {
            Dictionary<int, IComponent> components;
            componentGroups.TryGetValue(typeof(T), out components);

            return components ?? new Dictionary<int, IComponent>();
        }

        public Dictionary<Type, IComponent> GetComponentsForEntity(int entity)
        {
            if (entityComponents.ContainsKey(entity))
                return entityComponents[entity];
            return null;
        }
    }
}
