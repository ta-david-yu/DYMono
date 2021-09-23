using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using Microsoft.Xna.Framework;
using DYMono.Math;

namespace DYMono.ComponentSystem
{
    /// <summary>
    /// The component holds the transform data of an entity
    /// </summary>
    public class Transform : ComponentDataBase
    {
        private Transform m_Parent = null;
        public Transform Parent 
        { 
            get { return m_Parent; }
            private set { m_Parent = value; }
        }

        private float m_LocalRotation = 0;
        /// <summary>
        /// Local rotation in angle degree
        /// </summary>
        public float LocalRotation 
        { 
            get { return m_LocalRotation; } 
            set { m_LocalRotation = value; }
        }

        private Vector2 m_LocalPosition = Vector2.Zero;
        public Vector2 LocalPosition 
        { 
            get { return m_LocalPosition; }
            set { m_LocalPosition = value; }
        }

        public Vector2 WorldPosition
        {
            get
            {
                /// Lazy recursive transformation for now
                var pos = LocalPosition;
                if (m_Parent != null &&
                    !m_Parent.IsDestroyed)
                {
                    pos.Rotate((float) m_Parent.WorldRotation);
                    pos += m_Parent.WorldPosition;
                }
                return pos;
            }
        }

        public float WorldRotation
        {
            get
            {
                /// Lazy recursive transformation for now
                var rot = LocalRotation;
                
                if (m_Parent != null &&
                    !m_Parent.IsDestroyed)
                {
                    rot += m_Parent.WorldRotation;
                }
                
                return rot;
            }
        }

        public Vector2 Right
        {
            get { return Vector2Helper.Rotate(Vector2.UnitX, WorldRotation); }
            set 
            {
                Vector2 parentRight = (Parent != null && !Parent.IsDestroyed) ? Parent.Right : Vector2.UnitX;
                LocalRotation = Vector2Helper.SignedAngle(parentRight, value);
            }
        }

        public Vector2 Up
        {
            get { return Vector2Helper.Rotate(Vector2.UnitY, WorldRotation); }
            set
            {
                Vector2 parentUp = (Parent != null && !Parent.IsDestroyed) ? Parent.Up : Vector2.UnitY;
                LocalRotation = Vector2Helper.SignedAngle(parentUp, value);
            }
        }

        private List<Transform> m_Children = new List<Transform>();
        public ReadOnlyCollection<Transform> Children { get { return m_Children.AsReadOnly(); } }

        public void SetParent(Transform newParent)
        {
            if (m_Parent != null && !m_Parent.IsDestroyed)
            {
                m_Parent.m_Children.Remove(this);
            }

            m_Parent = newParent;

            if (m_Parent != null && !m_Parent.IsDestroyed)
            {
                m_Parent.m_Children.Add(this);
            }
        }

        public override void OnDrawGizmos()
        {
            float radius = 5.0f;
            /*
            Rendering.RenderCommand.Instance.ResetWorldTransform();
            Rendering.RenderCommand.Instance.SetWorldTransform(WorldPosition, 0);
            
            Rendering.RenderCommand.Instance.DrawWireCircle(Color.Blue, radius);

            Rendering.RenderCommand.Instance.DrawLineInWorldSpace(Color.Blue, WorldPosition - Right * radius, WorldPosition + Right * radius);
            Rendering.RenderCommand.Instance.DrawLineInWorldSpace(Color.Blue, WorldPosition - Up * radius, WorldPosition + Up * radius);
            */
        }
    }
}
