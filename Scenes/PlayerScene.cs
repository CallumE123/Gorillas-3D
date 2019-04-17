using Gorillas3D.Managers;
using OpenTK;
using OpenTK.Input;
using System;
using OpenTK.Graphics.OpenGL;
using OpenTK.Audio;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gorillas3D.Objects;
using System.Drawing;
using System.IO;

namespace Gorillas3D.Scenes
{
    class PlayerScene : Scene
    {
        AudioContext audioContext;
        int Players, Input, InputTotal, NumOfRounds;
        string Player1, Player2, CurrentVal;

        public PlayerScene(SceneManager sceneManager) : base(sceneManager)
        {
            sceneManager.Title = "3D Gorillas";
            sceneManager.Keyboard.KeyDown += Keyboard_KeyDown;
            sceneManager.renderer = Render;
            sceneManager.updater = Update;
            GL.ClearColor(0f, 0f, 0f, 1f);

            audioContext = new AudioContext();

            Players = 0;
            Input = 0;
            InputTotal = 4;

            NumOfRounds = 0;
            Player1 = "";
            Player2 = "";
            CurrentVal = "0";

            Audio Intro = new Audio(ResourceManager.LoadAudio("Audio/intro_effect.wav"));
            Intro.UpdateEmitPos(sceneManager.cam.Position);
            Intro.Vol = 0.4f;
            Intro.Play();

            CompileInputs();
        }

        public override void Update(FrameEventArgs e)
        {
            float dt = (float)e.Time;
            if (Keyboard.GetState().IsKeyDown(Key.Escape))
                sceneManager.Exit();

            if (Input >= InputTotal)
            {
                if (Keyboard.GetState().IsKeyDown(Key.Space))
                    sceneManager.SceneChange(EScene.Scene_Play);
            }

            switch (Input)
            {
                case 0:
                    Players = int.Parse(CurrentVal);
                    if (Players > 2) { Players = 2; }
                    break;

                case 1:
                    Player1 = CurrentVal;
                    break;

                case 2:
                    Player2 = CurrentVal;
                    break;

                case 3:
                    NumOfRounds = int.Parse(CurrentVal);
                    break;
            }
        }

        public override void Render(FrameEventArgs e)
        {
            GL.Viewport(0, 0, sceneManager.Width, sceneManager.Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, sceneManager.Width, 0, sceneManager.Height, -1, 100);

            GUI.clearColour = Color.Black;


            //Title Display
            float width = sceneManager.Width,
                height = sceneManager.Height,
                titleSize = Math.Min(width, height) / 10f,
                subTitleSize = Math.Min(width, height) / 20f,
                fontSize = Math.Min(width, height) / 25f;
            int middleX = (int)(width / 2),
                middleY = (int)(height / 2);

            GUI.Label(new Rectangle(0, (int)(titleSize / 2f), (int)width, (int)(titleSize * 2f)), "3D Gorillas", (int)titleSize, StringAlignment.Center);
            GUI.Label(new Rectangle(0, (int)(middleY / 2.3f), (int)width, (int)(fontSize * 2.0f)), "1 or 2 players (Default = 1): " + Players, (int)(fontSize * 0.5f), StringAlignment.Center);
            if (Input > 0)
            {
                GUI.Label(new Rectangle(0, (int)(middleY / 2.0f), (int)width, (int)(fontSize * 2.0f)), "Name of Player 1 (Default = 'Player 1'): " + Player1, (int)(fontSize * 0.5f), StringAlignment.Center);
            }
            if (Input > 1)
            {
                GUI.Label(new Rectangle(0, (int)(middleY / 1.75f), (int)width, (int)(fontSize * 2.0f)), "Name of Player 2 (Default = 'Player 2'): " + Player2, (int)(fontSize * 0.5f), StringAlignment.Center);
            }
            if (Input > 2)
            {
                GUI.Label(new Rectangle(0, (int)(middleY / 1.55f), (int)width, (int)(fontSize * 2.0f)), "Play to how many total points (Default = 3)? " + NumOfRounds.ToString(), (int)(fontSize * 0.5f), StringAlignment.Center);
            }
            if (Input > 3)
            {
                GUI.Label(new Rectangle(0, (int)(middleY / 1.0f), (int)width, (int)(fontSize * 2.0f)), "----------------", (int)(fontSize * 0.5f), StringAlignment.Center);
                GUI.Label(new Rectangle(0, (int)(middleY / 0.85f), (int)width, (int)(fontSize * 2.0f)), "Space = Play Game ", (int)(fontSize * 0.5f), StringAlignment.Center);
                GUI.Label(new Rectangle(0, (int)(middleY / 0.75f), (int)width, (int)(fontSize * 2.0f)), "Your Choice? ", (int)(fontSize * 0.5f), StringAlignment.Center);
            }
            GUI.Render();
        }

        void CompileInputs()
        {
            File.WriteAllText("Gorillas3D.txt", string.Empty);
            StreamWriter w = new StreamWriter("Gorillas3D.txt");
            //If empty inputs
            if(Players == 0) { Players = 1; }
            if(Player1 == "") { Player1 = "Player 1"; }
            if(Player2 == "") { Player2 = "Player 2"; }
            if(NumOfRounds == 0){ NumOfRounds = 3; }
            //Writes the player settings to a file for use in game scene
            w.WriteLine("Players = " + Players);
            w.WriteLine("Player 1 = " + Player1);
            w.WriteLine("Player 2 = " + Player2);
            w.WriteLine("Rounds =  " + NumOfRounds);
            w.WriteLine("Score   = 0-0");
            w.Close();

            if (Input >= InputTotal)
            {
                if (Keyboard.GetState().IsKeyDown(Key.Space))
                    
                    sceneManager.SceneChange(EScene.Scene_Play);
            }
        }

        public void Keyboard_KeyDown(object sender, KeyboardKeyEventArgs e)
        {
            string temp = "";
            // Cycle through switch statement to avoid rare key value (e.g. 1 key = Number1 but need 1)
            // This also makes it alphanumeric only which is what is needed
            switch (e.Key)
            {
                // Letters
                case Key.A:case Key.B:case Key.C:case Key.D:case Key.E:case Key.F:case Key.G:
                case Key.H:case Key.I:case Key.J:case Key.K:case Key.L:case Key.M:case Key.N:
                case Key.O:case Key.P:case Key.Q:case Key.R:case Key.S:case Key.T:case Key.U:
                case Key.V:case Key.W:case Key.X:case Key.Y:case Key.Z:
                    if (Input < 3)
                    {
                        if (e.Shift) { temp += e.Key.ToString(); }
                        else { temp += e.Key.ToString().ToLower(); }
                    }
                    break;
                // Numbers
                case Key.Number0:
                    temp += "0";
                    break;
                case Key.Number1:
                    temp += "1";
                    break;
                case Key.Number2:
                    temp += "2";
                    break;
                case Key.Number3:
                    temp += "3";
                    break;
                case Key.Number4:
                    temp += "4";
                    break;
                case Key.Number5:
                    temp += "5";
                    break;
                case Key.Number6:
                    temp += "6";
                    break;
                case Key.Number7:
                    temp += "7";
                    break;
                case Key.Number8:
                    temp += "8";
                    break;
                case Key.Number9:
                    temp += "9";
                    break;
                // Format related keys
                case Key.BackSpace:
                    if (CurrentVal.Length <= 0) { break; }
                    CurrentVal = CurrentVal.Substring(0, CurrentVal.Length - 1);
                    break;
                case Key.Period:
                    temp += ".";
                    break;
                // Functions
                case Key.Enter:
                    Input++;
                    if (Input == 0 || Input > 2) { CurrentVal = "0"; }
                    else { CurrentVal = ""; }
                    break;
                case Key.Space:
                    if (Input >= InputTotal) { CompileInputs(); }
                    break;
            }

            CurrentVal += temp;
        }
        public override void Close()
        {
            audioContext.Dispose();
            sceneManager.Keyboard.KeyDown -= Keyboard_KeyDown;
        }
    }
}
