using Labb3_Datorgrafik.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Labb3_Datorgrafik.Managers
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

        public IEnumerable<(int, T)> GetComponentsOfType<T>()
        {
            componentGroups.TryGetValue(typeof(T), out Dictionary<int, IComponent> components);
            if (components == null)
                yield break;

            foreach (var component in components)
            {
                yield return (component.Key, (T)component.Value);
            }
        }

        public IEnumerable<(int, T1, T2)> GetComponentsOfType<T1, T2>()
        {
            componentGroups.TryGetValue(typeof(T1), out Dictionary<int, IComponent> components1);
            if (components1 == null)
                yield break;
            componentGroups.TryGetValue(typeof(T2), out Dictionary<int, IComponent> components2);
            if (components2 == null)
                yield break;

            int[] keys1 = new int[components1.Keys.Count];
            components1.Keys.CopyTo(keys1, 0);

            int[] keys2 = new int[components2.Keys.Count];
            components2.Keys.CopyTo(keys2, 0);

            int[] keys = keys1.Intersect(keys2).ToArray();

            foreach (var entity in keys)
            {
                var components = GetComponentsForEntity(entity);
                components.TryGetValue(typeof(T1), out IComponent component1);
                components.TryGetValue(typeof(T2), out IComponent component2);
                yield return (entity, (T1)component1, (T2)component2);
            }
        }

        public IEnumerable<(int, T1, T2, T3)> GetComponentsOfType<T1, T2, T3>()
        {
            componentGroups.TryGetValue(typeof(T1), out Dictionary<int, IComponent> components1);
            if (components1 == null)
                yield break;
            componentGroups.TryGetValue(typeof(T2), out Dictionary<int, IComponent> components2);
            if (components2 == null)
                yield break;
            componentGroups.TryGetValue(typeof(T3), out Dictionary<int, IComponent> components3);
            if (components3 == null)
                yield break;

            int[] keys1 = new int[components1.Keys.Count];
            components1.Keys.CopyTo(keys1, 0);

            int[] keys2 = new int[components2.Keys.Count];
            components2.Keys.CopyTo(keys2, 0);

            int[] keys3 = new int[components3.Keys.Count];
            components3.Keys.CopyTo(keys3, 0);

            int[] keys = keys1.Intersect(keys2).Intersect(keys3).ToArray();

            foreach (var entity in keys)
            {
                var components = GetComponentsForEntity(entity);
                components.TryGetValue(typeof(T1), out IComponent component1);
                components.TryGetValue(typeof(T2), out IComponent component2);
                components.TryGetValue(typeof(T3), out IComponent component3);
                yield return (entity, (T1)component1, (T2)component2, (T3)component3);
            }
        }

        public Dictionary<Type, IComponent> GetComponentsForEntity(int entity)
        {
            if (entityComponents.ContainsKey(entity))
                return entityComponents[entity];
            return null;
        }
    }
}
