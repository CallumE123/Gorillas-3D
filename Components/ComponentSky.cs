using Gorillas3D.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gorillas3D.Components
{
    class ComponentSky : IComponent
    {
        int texture;

        public ComponentSky(string[] textureName)
        {
            texture = ResourceManager.LoadSkyBox(textureName);
        }

        public int Texture
        {
            get { return texture; }
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_SKY; }
        }
    }
}
