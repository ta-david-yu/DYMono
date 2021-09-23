using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DYMono.ComponentSystem
{
    /// <summary>
    /// System updates a set of component datas
    /// </summary>
    public abstract class SystemBase
    {
        public enum RegisterResultType
        {
            Success,
            FailedToAddAbstractComponent,
        }

        public struct RegisterResult
        {
            public RegisterResultType Type;
            public Type AbstractComponent;
        }

        /// <summary>
        /// Get the essential component types that are required by this system
        /// </summary>
        /// <returns></returns>
        public abstract Type[] GetEssentialTypeSet();

        protected List<ComponentDataBase[]> m_ComponentSets = new List<ComponentDataBase[]>();

        /// <summary>
        /// Clean up all the registered component sets
        /// </summary>
        public void CleanUpAllSets()
        {
            m_ComponentSets = new List<ComponentDataBase[]>();
        }

        public virtual void Update() { }

        /// <summary>
        /// Register an entity to this system. The system will try to add component automatically if one is missing.
        /// </summary>
        /// <param name="entity">The entity to be registered</param>
        /// <returns>true if the entity is successfully registered. Otherwise false.</returns>
        public bool TryRegisterEntityToSystem(Entity entity)
        {
            return TryRegisterEntityToSystem(entity, out _);
        }

        /// <summary>
        /// Register an entity to this system. The system will try to add component automatically if one is missing.
        /// </summary>
        /// <param name="entity">The entity to be registered</param>
        /// <param name="result">The result of the registry</param>
        /// <returns>true if the entity is successfully registered. Otherwise false. Check the result output for more information.</returns>
        public bool TryRegisterEntityToSystem(Entity entity, out RegisterResult result)
        {
            var typeSet = GetEssentialTypeSet();
            List<ComponentDataBase> componentSet = new List<ComponentDataBase>();
            for (int i = 0; i < typeSet.Length; i++)
            {
                Type compType = typeSet[i];
                ComponentDataBase component = entity.GetComponent(compType);

                // Already has a component of the given type
                if (component != null && !component.IsDestroyed)
                {
                    componentSet.Add(component);
                }
                // There hasn't been a component of the given type, add a new one
                else
                {
                    if (compType.IsAbstract)
                    {
                        result.Type = RegisterResultType.FailedToAddAbstractComponent;
                        result.AbstractComponent = compType;
                        return false;
                    }
                    else
                    {
                        componentSet.Add(entity.AddComponent(compType));
                    }
                }
            }

            // Register the set into the set list
            m_ComponentSets.Add(componentSet.ToArray());

            result.Type = RegisterResultType.Success;
            result.AbstractComponent = null;
            return true;
        }
    }
}
