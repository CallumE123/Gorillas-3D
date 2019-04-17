using System;

namespace Gorillas3D.Components
{
    [FlagsAttribute]
    enum ComponentTypes
    {
        COMPONENT_NONE = 0,
        COMPONENT_GEOMETRY = 1 << 0,
        COMPONENT_POSITION = 1 << 1,
        COMPONENT_TEXTURE = 1 << 2,
        COMPONENT_TRANSFORM = 1 << 3,
        COMPONENT_COLLISION = 1 << 4,
        COMPONENT_VELOCITY = 1 << 5,
        COMPONENT_DIRECTION = 1 << 6,
        COMPONENT_SKY = 1 << 7,
        COMPONENT_AUDIO = 1 << 8
    }
    
    interface IComponent
    {
        ComponentTypes ComponentType
        {
            get;
        }
    }
}
