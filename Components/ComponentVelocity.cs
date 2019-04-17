using OpenTK;

namespace Gorillas3D.Components
{
    class ComponentVelocity : IComponent
    {
        float velocity;
        
        public ComponentVelocity(float vel)
        {
            velocity = vel;
        }

        public float Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_VELOCITY; }
        }
    }
}
