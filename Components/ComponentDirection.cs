using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gorillas3D.Components
{
    class ComponentDirection : IComponent
    {

        Vector3 direction;

        public ComponentDirection(float x, float y, float z)
        {
            direction = new Vector3(x, y, z);
        }

        public ComponentDirection(Vector3 pos)
        {
            direction = pos;
        }

        public Vector3 Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_DIRECTION; }
        }
    }
}
