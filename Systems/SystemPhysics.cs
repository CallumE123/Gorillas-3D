using System.Collections.Generic;
using Gorillas3D.Components;
using Gorillas3D.Objects;
using Gorillas3D.Scenes;

namespace Gorillas3D.Systems
{
    class SystemPhysics : ISystem
    {
        const ComponentTypes MASK = (ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_VELOCITY | ComponentTypes.COMPONENT_DIRECTION);
        Camera mCamera;

        public string Name
        {
            get { return "SystemPhysics"; }
        }

        public SystemPhysics(ref Camera pCam)
        {
            mCamera = pCam;
        }

        public void OnAction(Entity entity)
        {
            if ((entity.Mask & MASK) == MASK)
            {
                List<IComponent> components = entity.Components;

                IComponent velocityComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_VELOCITY;
                });

                IComponent positionComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_POSITION;
                });

                IComponent directionComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_DIRECTION;
                });

                Motion((ComponentPosition)positionComponent, (ComponentVelocity)velocityComponent, (ComponentDirection)directionComponent);
            }
        }

        public void Motion(ComponentPosition pPos, ComponentVelocity pVel, ComponentDirection pDir)
        {
            pPos.Position += pDir.Direction * (pVel.Velocity * GameScene.dt);
        }
    }
}
