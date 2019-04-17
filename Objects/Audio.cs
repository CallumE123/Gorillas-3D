using OpenTK;
using OpenTK.Audio.OpenAL;

namespace Gorillas3D.Objects
{
    class Audio
    {
        private Vector3 pEmitterPos;
        private int mBuffer, mSource;
        private bool PlayAuto;
        private bool PlayLoop;
        private float PlayVol;

        public Audio(int pBuffer)
        {
            mBuffer = pBuffer;
            PlayAuto = false;
            PlayLoop = false;
            PlayVol = 0.5f;
            if (PlayAuto)
            {
                Play();
            }
        }

        //Plays audio Object
        public void Play()
        {
            //Creates a source for the sound
            mSource = AL.GenSource();
            AL.Source(mSource, ALSourcei.Buffer, mBuffer);
            AL.Source(mSource, ALSourceb.Looping, PlayLoop);
            AL.Source(mSource, ALSourcef.Gain, PlayVol);
            AL.Source(mSource, ALSource3f.Position, ref pEmitterPos);
            AL.SourcePlay(mSource);
        }

        //Updates the position of an entity's audio
        public void UpdateEmitPos(Vector3 pPos)
        {
            pEmitterPos = pPos;
            AL.Source(mSource, ALSource3f.Position, ref pEmitterPos);
        }

        //When a scene is reloaded use stored audio without needing original
        public void ReloadAudio()
        {
            if (PlayAuto)
            {
                Play();
            }
        }

        public void Close()
        {
            AL.SourceStop(mSource);
            AL.DeleteSource(mSource);
        }


        //boolians for option of auto playing or loop playing
        public bool AutoPlay
        {
            set { PlayAuto = value; }
        }

        public bool LoopPlay
        {
            set { PlayLoop = value; }
        }

        //set volume of object
        public float Vol
        {
            set { PlayVol = value; }
        }
    }
}
