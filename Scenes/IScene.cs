using OpenTK;

namespace Gorillas3D.Scenes
{
    enum EScene
    {
        Scene_Start,
        Scene_Player,
        Scene_Play,
        Scene_GameOver
    }
    
    interface IScene
    {
        void Render(FrameEventArgs e);
        void Update(FrameEventArgs e);
        void Close();
    }
}
