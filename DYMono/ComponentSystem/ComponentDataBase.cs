using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DYMono.ComponentSystem
{
    /// <summary>
    /// The base class of any components, has no event functions
    /// </summary>
    public abstract class ComponentDataBase
    {
        public Entity Entity { get; private set; } = null;

        public bool IsActive { get; set; } = true;

        public bool IsDestroyed { get; private set; } = false;

        public void SetEntity(Entity entity)
        {
            Entity = entity;
        }

        public void Destroy()
        {
            IsDestroyed = true;
        }

        public virtual void OnDrawGizmos() { }

        /// <summary>
        /// Called at the end of Entity.AddComponent
        /// </summary>
        public virtual void PostAddComponent() { }
    }
}
