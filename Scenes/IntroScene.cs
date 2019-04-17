using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using Gorillas3D.Managers;
using OpenTK.Input;

namespace Gorillas3D.Scenes
{
    class IntroScene : Scene
    {
        public IntroScene(SceneManager sceneManager) : base(sceneManager)
        {
            sceneManager.Title = "3D Gorillas Intro";
            sceneManager.renderer = Render;
            sceneManager.updater = Update;
            
        }

        public override void Update(FrameEventArgs e)
        {
            if (Keyboard.GetState().IsKeyDown(Key.Escape))
                sceneManager.Exit();

            if (Keyboard.GetState().IsKeyDown(Key.Space))
            {
                sceneManager.SceneChange(EScene.Scene_Player);
            }
        }

        public override void Render(FrameEventArgs e)
        {
            GL.Viewport(0, 0, sceneManager.Width, sceneManager.Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, sceneManager.Width, 0, sceneManager.Height, -1, 1);

            GUI.clearColour = Color.Black;

            //Title Display
            float width = sceneManager.Width,
                height = sceneManager.Height,
                titleSize = Math.Min(width, height) / 10f,
                subTitleSize = Math.Min(width, height) / 20f,
                fontSize = Math.Min(width, height) / 35f;

            GUI.Label(new Rectangle(0, (int)(titleSize / 2f), (int)width, (int)(titleSize * 2f)), "3D Gorillas", (int)titleSize, StringAlignment.Center);
            GUI.Label(new Rectangle(0, (int)(fontSize / 2f), (int)width, (int)(fontSize * 20f)), 
                "Your mission is to hit your opponent with the exploding banana by varying the angle and power of your throw, taking " +
                "into account the city skyline as an obstacle.", (int)fontSize, StringAlignment.Center);
            GUI.Label(new Rectangle(0, (int)(fontSize / 2f), (int)width, (int)(fontSize * 40f)), "Press the SPACE key to continue", (int)fontSize, StringAlignment.Center);


            GUI.Render();
        }

        public override void Close()
        {

        }
    }
}
