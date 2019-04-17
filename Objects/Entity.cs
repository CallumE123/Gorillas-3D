using System;
using System.Collections.Generic;
using System.Diagnostics;
using Gorillas3D.Components;

namespace Gorillas3D.Objects
{
    class Entity
    {
        string name;
        List<IComponent> componentList = new List<IComponent>();
        ComponentTypes mask;

        public Entity(string name)
        {
            this.name = name;
        }

        public void AddComponent(IComponent component)
        {
            Debug.Assert(component != null, "Component can't be null");
            componentList.Add(component);
            mask |= component.ComponentType;
        }

        public IComponent FindComponent(ComponentTypes type)
        {
            return componentList.Find(delegate (IComponent c)
            {
                return c.ComponentType == type;
            });
        }

        public string Name
        {
            get { return name; }
        }

        public ComponentTypes Mask
        {
            get { return mask; }
        }

        public List<IComponent> Components
        {
            get { return componentList; }
        }
    }
}
