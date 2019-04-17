using OpenTK;

namespace Gorillas3D.Components
{
    class ComponentTransform : IComponent
    {
        Matrix4 transform;

        public ComponentTransform(Vector3 pScale, Vector3 pRotation)
        {
            transform = Matrix4.CreateRotationX(pRotation.X);
            transform *= Matrix4.CreateRotationY(pRotation.Y);
            transform *= Matrix4.CreateRotationZ(pRotation.Z);
            transform *= Matrix4.CreateScale(pScale);
        }

        public ComponentTransform(Matrix4 trans)
        {
            transform = trans;
        }

        public Matrix4 Transform
        {
            get { return transform; }
            set { transform = value; }
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_TRANSFORM; }
        }
    }
}
