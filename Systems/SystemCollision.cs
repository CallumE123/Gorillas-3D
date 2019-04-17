using System;
using System.Collections.Generic;
using Gorillas3D.Components;
using Gorillas3D.Objects;
using Gorillas3D.Managers;
using OpenTK;

namespace Gorillas3D.Systems
{
    class SystemCollision : ISystem
    {
        const ComponentTypes MASK = (ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_COLLISION | ComponentTypes.COMPONENT_TRANSFORM);
        Vector3 mObject;

        public string Name
        {
            get { return "SystemCollision"; }
        }

        public SystemCollision(Vector3 pVec)
        {
            mObject = pVec;
        }

        public void OnAction(Entity entity)
        {
            if ((entity.Mask & MASK) == MASK)
            {
                List<IComponent> components = entity.Components;

                IComponent positionComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_POSITION;
                });

                IComponent collisionComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_COLLISION;
                });

                IComponent transformComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_TRANSFORM;
                });

                Collide((ComponentPosition)positionComponent, (ComponentTransform)transformComponent);
            }
        }

        public void Collide(ComponentPosition pPos, ComponentTransform transform)
        {
            float xOffset = pPos.Position.X + (transform.Transform.ExtractScale().X / 2);
            float sxOffset = pPos.Position.X - (transform.Transform.ExtractScale().X / 2);
            float zOffset = pPos.Position.Z + (transform.Transform.ExtractScale().Z / 2);
            float szOffset = pPos.Position.Z - (transform.Transform.ExtractScale().Z / 2);

            if (mObject.X >= sxOffset && mObject.X <= xOffset)
            {
                if (mObject.Z > szOffset && mObject.Z < zOffset)
                {
                    Console.WriteLine("Collision Detected");
                }
            }
        }

        public void UpdatePosition(Vector3 pPos)
        {
            mObject = pPos;
        }
    }
}
