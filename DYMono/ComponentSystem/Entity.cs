using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace DYMono.ComponentSystem
{
    public class Entity
    {
        private string m_Name;
        public string Name 
        { 
            get { return m_Name; } 
            private set { m_Name = value; } 
        }

        private int m_ID = -1;
        public int ID 
        {
            get { return m_ID; }
            private set { m_ID = value; }
        }

        public bool IsSelfActive { get; set; } = true;
        public bool IsActiveInHierarchy
        {
            get
            {
                /// Lazy recursive is active for now
                if (Transform.Parent != null &&
                    !Transform.Parent.IsDestroyed)
                {
                    return IsSelfActive && Transform.Parent.Entity.IsActiveInHierarchy;
                }
                return IsSelfActive;
            }
        }

        public bool IsDestroyed { get; private set; } = false;

        public Transform Transform { get; private set; } = null;

        private List<ComponentDataBase> m_Components = new List<ComponentDataBase>();
        public ReadOnlyCollection<ComponentDataBase> Components { get { return m_Components.AsReadOnly(); } }

        private Entity(int id, string name)
        {
            m_ID = id;
            m_Name = name;
        }

        public T AddComponent<T>() where T : ComponentDataBase, new()
        {
            var comp = new T();
            comp.SetEntity(this);

            m_Components.Add(comp);
            comp.PostAddComponent();
            return comp;
        }

        public ComponentDataBase AddComponent(Type type)
        {
            var comp = Activator.CreateInstance(type) as ComponentDataBase;
            comp.SetEntity(this);

            m_Components.Add(comp);
            comp.PostAddComponent();
            return comp;
        }

        public T GetComponent<T>() where T : ComponentDataBase
        {
            for (int i = 0; i < m_Components.Count; i++)
            {
                var comp = m_Components[i];
                if (comp is T)
                {
                    return comp as T;
                }
            }
            return null;
        }

        public ComponentDataBase GetComponent(Type type)
        {
            for (int i = 0; i < m_Components.Count; i++)
            {
                var comp = m_Components[i];
                if (type.IsAssignableFrom(comp.GetType()))
                {
                    return comp;
                }
            }
            return null;
        }

        public class Factory
        {
            private static Factory s_Instance = null;
            public static Factory Instance
            {
                get
                {
                    if (s_Instance == null)
                    {
                        s_Instance = new Factory();
                    }
                    return s_Instance;
                }
            }


            private int m_IDCounter = -1;

            /// <summary>
            /// ID to EntityInstance
            /// </summary>
            public Dictionary<int, Entity> Entities = new Dictionary<int, Entity>();

            /// <summary>
            /// Destory all entities and their components
            /// </summary>
            public void CleanUpAllEntities()
            {
                foreach (var entityPair in Entities)
                {
                    Entity entity = entityPair.Value;

                    entity.IsDestroyed = true;

                    // Destroy all components
                    for (int i = 0; i < entity.Components.Count; i++)
                    {
                        var component = entity.Components[i];
                        component.Destroy();
                    }
                }

                Entities = new Dictionary<int, Entity>();
                m_IDCounter = -1;
            }

            public Entity CreateEntity(string name = "New Entity")
            {
                m_IDCounter += 1;
                Entity newEnt = new Entity(m_IDCounter, name);
                newEnt.Transform = newEnt.AddComponent<Transform>();

                Entities.Add(m_IDCounter, newEnt);
                return newEnt;
            }

            public void DestoryEntity(Entity entity)
            {
                entity.IsDestroyed = true;

                // Destroy child objects if there is a transform component
                Transform transform = entity.GetComponent<Transform>();
                if (transform != null && 
                    !transform.IsDestroyed)
                {
                    for (int i = 0; i < transform.Children.Count; i++)
                    {
                        var childTransform = transform.Children[i];
                        DestoryEntity(childTransform.Entity);
                    }
                }

                // Destroy all components
                for (int i = 0; i < entity.Components.Count; i++)
                {
                    var component = entity.Components[i];
                    component.Destroy();
                }

                Entities.Remove(entity.ID);
            }

            public Entity GetEntity(int id)
            {
                Entity entity;
                if (!Entities.TryGetValue(id, out entity))
                {
                    return null;
                }
                return entity;
            }
        }
    }
}
