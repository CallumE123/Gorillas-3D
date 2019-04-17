using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Gorillas3D.Scenes;
using Gorillas3D.Objects;
using OpenTK.Input;
using OpenTK.Audio.OpenAL;

namespace Gorillas3D.Managers
{
    class SceneManager : GameWindow
    {
        Scene scene;

        public static int width = 1600, height = 900;

        public delegate void SceneDelegate(FrameEventArgs e);
        public SceneDelegate renderer;
        public SceneDelegate updater;
        public Camera cam;

        private Vector3 ListenPos, ListenDir, ListenVirt;

        public SceneManager() : base(width, height, new OpenTK.Graphics.GraphicsMode(new OpenTK.Graphics.ColorFormat(8,8,8,8), 16))
        {
            cam = new Camera();
            ListenPos = cam.Position;
        }

        public void SceneChange(EScene eScene)
        {
            cam.ReloadCamera();
            switch (eScene)
            {
                case EScene.Scene_Start:
                    scene.Close();
                    StartIntro();
                    break;

                case EScene.Scene_Player:
                    scene.Close();
                    StartPlayer();
                    break;

                case EScene.Scene_Play:
                    scene.Close();
                    StartNewGame();
                    break;

               /* case EScene.Scene_GameOver:
                    scene.Close();
                    EndGame();
                    break;*/
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            CursorVisible = true;
            GL.Enable(EnableCap.DepthTest);

            //Loads the GUI
            GUI.SetUpGUI(width, height);

            StartIntro();
        }
        
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            ListenPos = cam.Position;
            AL.Listener(ALListener3f.Position, ref ListenPos);

            updater(e);
        }

        protected override void OnFocusedChanged(EventArgs e)
        {
            base.OnFocusedChanged(e);

            if (Focused)
            {

            }
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            renderer(e);

            GL.Flush();
            SwapBuffers();
        }

        public void StartIntro()
        {
            scene = new IntroScene(this);
        }

        public void StartPlayer()
        {
            scene = new PlayerScene(this);
        }

        public void StartNewGame()
        {
            scene = new GameScene(this);
        }

        /*public void EndGame()
        {
            scene = new GameOverScene(this);
        }*/

        public static int Window_Width
        {
            get { return width; }
        }

        public static int Window_Height
        {
            get { return height; }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);
            SceneManager.width = Width;
            SceneManager.height = Height;

            //Reloads the GUI to new scale
            GUI.SetUpGUI(Width, Height);
        }
    }
}
