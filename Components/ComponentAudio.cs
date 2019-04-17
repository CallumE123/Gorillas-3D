using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gorillas3D.Managers;
using Gorillas3D.Objects;
using OpenTK.Graphics;
using OpenTK.Audio;

namespace Gorillas3D.Components
{
    class ComponentAudio : IComponent
    {
        Audio audio;

        public ComponentAudio(string audioName, float vol)
        {
            audio = new Audio(ResourceManager.LoadAudio(audioName));
            audio.Vol = vol;
        }
        
        public ComponentAudio(string audioName, float vol, bool auto)
        {
            audio = new Audio(ResourceManager.LoadAudio(audioName));
            audio.AutoPlay = auto;
            audio.Vol = vol;
        }

        public ComponentAudio(string audioName, float vol, bool auto, bool loop)
        {
            audio = new Audio(ResourceManager.LoadAudio(audioName));
            audio.AutoPlay = auto;
            audio.LoopPlay = loop;
            audio.Vol = vol;
        }

        public Audio AudioObject
        {
            get { return audio; }
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_AUDIO; }   
        }

        public void CloseAudio()
        {
            audio.Close();
        }
    }
}
