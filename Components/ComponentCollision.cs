namespace Gorillas3D.Components
{
    class ComponentCollision : IComponent
    {
        bool collide;

        public ComponentCollision(bool col)
        {
            collide = col;
        }

        public bool isCollidable
        {
            get { return collide; }
            set { collide = value; }
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_COLLISION; }
        }

    }
}
