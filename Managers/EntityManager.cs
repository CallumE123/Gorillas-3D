using System.Collections.Generic;
using Gorillas3D.Objects;
using Gorillas3D.Components;

namespace Gorillas3D.Managers
{
    class EntityManager
    {
        List<Entity> entityList;

        public EntityManager()
        {
            entityList = new List<Entity>();
        }

        //List of entities
        public List<Entity> Entities()
        {
            return entityList;
        }

        //New entity to the games entity list
        public void AddEntity(Entity entity)
        {
            Entity result = FindEntity(entity.Name);
            entityList.Add(entity);
        }

        //Finds entity through name search
        public Entity FindEntity(string name)
        {
            return entityList.Find(delegate (Entity e)
            {
                return e.Name == name;
            });
        }

        //Remove entity from the games entity list
        public void RemoveEntity(string name)
        {
            DeleteEntity(FindEntity(name));
            entityList.Remove(FindEntity(name));
        }

        //Cleans up attached audio components on the closing of the scene
        public void DeleteEntities()
        {
            foreach(Entity entity in entityList)
            {
                
                ComponentTypes MASK = (ComponentTypes.COMPONENT_AUDIO);

                if ((entity.Mask & MASK) == MASK)
                {
                    List<IComponent> components = entity.Components;

                    IComponent audioComponent = components.Find(delegate (IComponent component)
                    {
                        return component.ComponentType == ComponentTypes.COMPONENT_AUDIO;
                    });

                    Audio audioOff = ((ComponentAudio)audioComponent).AudioObject;
                    audioOff.Close();
                } 
                
            }
        }

        //Cleans up the audio component from a specific deleted entity
        public void DeleteEntity(Entity entity)
        {
            ComponentTypes MASK = (ComponentTypes.COMPONENT_AUDIO);

            if ((entity.Mask & MASK) == MASK)
            {
                List<IComponent> components = entity.Components;

                IComponent audioComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_AUDIO;
                });

                Audio audioOff = ((ComponentAudio)audioComponent).AudioObject;
                audioOff.Close();
            }
        }

        //Clears the entity list
        public void ClearEntities()
        {
            entityList = new List<Entity>();
        }
    }
}
