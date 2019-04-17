using System.Collections.Generic;
using Gorillas3D.Systems;
using Gorillas3D.Objects;

namespace Gorillas3D.Managers
{
    class SystemManager
    {
        List<ISystem> systemList = new List<ISystem>();

        public SystemManager()
        {

        }

        //Loops through systems for each entity passing into it
        public void ActionSystem(EntityManager entityManager)
        {
            List<Entity> entityList = entityManager.Entities();
            foreach(ISystem system in systemList)
            {
                foreach(Entity entity in entityList)
                {
                    system.OnAction(entity);
                }
            }
        }

        //Adds a new system to the list of systems in game scene
        public void AddSystem(ISystem system)
        {
            ISystem result = FindSystem(system.Name);
            systemList.Add(system);
        }


        //Returns system upon search
        public ISystem FindSystem(string name)
        {
            return systemList.Find(delegate (ISystem system)
            {
                return system.Name == name;
            });
        }

        //Clears the list of systems
        public void ClearSystems()
        {
            systemList = new List<ISystem>();
        }
    }
}
