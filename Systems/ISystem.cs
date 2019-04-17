using Gorillas3D.Objects;

namespace Gorillas3D.Systems
{
    interface ISystem
    {
        void OnAction(Entity entity);

        string Name
        {
            get;
        }
    }
}
