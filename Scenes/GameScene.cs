using Gorillas3D.Components;
using Gorillas3D.Managers;
using Gorillas3D.Objects;
using Gorillas3D.Systems;
using OpenTK;
using OpenTK.Audio;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Drawing;
using System.IO;

namespace Gorillas3D.Scenes
{
    class GameScene : Scene
    {
        EntityManager entityManager;
        SystemManager systemManager;
        AudioContext audioContext;
        Audio SoundEffect;

        int InputNum, Rounds, Score1, Score2;
        string Input, Angle, Power, Player1, Player2;
        bool isPlayer1;
        public Matrix4 view, projection;
        public static float dt = 0;
        float ProjTime;

        public static GameScene instance;

        public GameScene(SceneManager sceneManager): base(sceneManager)
        {
            instance = this;
            entityManager = new EntityManager();
            systemManager = new SystemManager();
            audioContext = new AudioContext();

            sceneManager.Title = "3D Gorillas";
            sceneManager.Keyboard.KeyDown += Keyboard_KeyDown;
            sceneManager.renderer = Render;
            sceneManager.updater = Update;

            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
            view = sceneManager.cam.GetViewMatrix();
            projection = sceneManager.cam.GetViewMatrix() * Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45), sceneManager.cam.width / (float)sceneManager.cam.height, 10f, 1000);

            //Sets the camera's position for the frame of play.
            sceneManager.cam.Position = new Vector3(-8.0f, 30.0f, -8.0f);
            sceneManager.cam.Rotation(10.2f, -0.3f);
            
            //Loads the game scenes entities & systems.
            CreateArena();
            CreateSystems();

            //Determines which players turn it is: true = player 1, false = player 2.
            isPlayer1 = true;

            //Variables to allow the players to input their choices.
            Input = ""; Angle = ""; Power = "";

            //Identifies which input the game is currently on
            InputNum = 0;

            //Handle for the length of time the banana is in motion.
            ProjTime = 0.0f;

            LoadGameState();
            LoadAudFiles();
        }

        private void CreateArena()
        {
            float startX = 0.0f;
            float startZ = 0.0f;
            Random rnd = new Random();
            Entity newEntity;

            newEntity = new Entity("SkyBox");
            newEntity.AddComponent(new ComponentGeometry("Geometry/CubeGeometry.txt"));
            newEntity.AddComponent(new ComponentPosition(0.0f, 0.0f, 0.0f));
            newEntity.AddComponent(new ComponentTransform(new Vector3(100f), new Vector3()));
            string[] SkyBox = new string[] { "Textures/ft.jpg", "Textures/bk.jpg", "Textures/up.jpg", "Textures/dn.jpg", "Textures/rt.jpg", "Textures/lf.jpg" };
            newEntity.AddComponent(new ComponentSky(SkyBox));
            entityManager.AddEntity(newEntity);

            for (int i = 0; i < 4; i++)
            {
                int randomHeight = rnd.Next(19, 22);
                int roof = randomHeight + 1;

                for (int y = 0; y < randomHeight; y++)
                {
                    for (int x = 0; x < 10; x++)
                    {
                        for (int z = 0; z < 10; z++)
                        {
                            newEntity = new Entity("Building");
                            newEntity.AddComponent(new ComponentPosition(startX + x, y,startZ + z));
                            newEntity.AddComponent(new ComponentGeometry("Geometry/CubeGeometry.txt"));
                            newEntity.AddComponent(new ComponentTexture("Textures/Building.jpg"));
                            //newEntity.AddComponent(new ComponentCollision(true));
                            newEntity.AddComponent(new ComponentTransform(new Vector3(0.5f, 0.5f, 0.5f), new Vector3()));
                            entityManager.AddEntity(newEntity);

                            newEntity = new Entity("Roof");
                            newEntity.AddComponent(new ComponentPosition(startX + x, roof - 1.25f, startZ + z));
                            newEntity.AddComponent(new ComponentGeometry("Geometry/CubeGeometry.txt"));
                            newEntity.AddComponent(new ComponentTexture("Textures/Building_2.jpg"));
                            //newEntity.AddComponent(new ComponentCollision(true));
                            newEntity.AddComponent(new ComponentTransform(new Vector3(0.5f, 0.25f, 0.5f), new Vector3()));
                            entityManager.AddEntity(newEntity);
                        }
                    }
                }
                startZ += 11.0f;
                if(startZ >= 22)
                {
                    startX += 11.0f;
                    startZ = 0.0f;
                }
                
                if(i == 0)
                {
                    newEntity = new Entity("Gorillas 1");
                    newEntity.AddComponent(new ComponentPosition(3.0f, roof, 3.0f));
                    newEntity.AddComponent(new ComponentGeometry("Geometry/CubeGeometry.txt"));
                    newEntity.AddComponent(new ComponentTexture("Textures/Gorilla.jpg"));
                    //newEntity.AddComponent(new ComponentCollision(true));
                    newEntity.AddComponent(new ComponentTransform(new Vector3(0.25f, 0.5f, 0.25f), new Vector3()));
                    entityManager.AddEntity(newEntity);

                    newEntity = new Entity("Banana");
                    newEntity.AddComponent(new ComponentPosition(3.0f, roof, 3.0f));
                    newEntity.AddComponent(new ComponentGeometry("Geometry/CubeGeometry.txt"));
                    newEntity.AddComponent(new ComponentTexture("Textures/Banana.jpg"));
                    newEntity.AddComponent(new ComponentTransform(new Vector3(0.25f, 0.25f, 0.25f), new Vector3(0.0f)));
                    entityManager.AddEntity(newEntity);
                }

                if(i == 3)
                {
                    newEntity = new Entity("Gorillas 2");
                    newEntity.AddComponent(new ComponentPosition(16.0f, roof, 16.0f));
                    newEntity.AddComponent(new ComponentGeometry("Geometry/CubeGeometry.txt"));
                    newEntity.AddComponent(new ComponentTexture("Textures/Gorilla.jpg"));
                    newEntity.AddComponent(new ComponentCollision(true));
                    newEntity.AddComponent(new ComponentTransform(new Vector3(0.25f, 0.5f, 0.25f), new Vector3()));
                    entityManager.AddEntity(newEntity);
                }

            }
        }
        
        public override void Update(FrameEventArgs e)
        {
            dt = (float)e.Time;
            var keyboardState = Keyboard.GetState();
            view = sceneManager.cam.GetViewMatrix();

            if (keyboardState.IsKeyDown(Key.Escape))
                sceneManager.Exit();
            
            //Trajectory calculation
            if (InputNum >= 2)
            {

                ComponentPosition pos;
                if (ProjTime <= 0.0f)
                {
                    SoundEffect.UpdateEmitPos(sceneManager.cam.Position);
                    SoundEffect.Vol = 1f;
                    SoundEffect.Play();
                }

                SystemCollision collider = (SystemCollision)systemManager.FindSystem("SystemCollision");
                pos = (ComponentPosition)entityManager.FindEntity("Banana").FindComponent(ComponentTypes.COMPONENT_POSITION);

                float power, angle;

                if(Angle == "") { angle = 0f; }
                else { angle = (float)((Math.PI / 180) * float.Parse(Angle)); }
                if(Power == "") { power = 0f; }
                else { power = float.Parse(Power) / 25f; }
                // x = X+V*COS(A)*T
				// z = Z+V*COS(A)*T
                // y = Y+V*SIN(A)*T-G*T*T/2
                float x = pos.Position.X + power * (float)Math.Cos(angle) * ProjTime;
                float z = pos.Position.Z + power * (float)Math.Cos(angle) * ProjTime;
                float y = pos.Position.Y + power * (float)Math.Sin(angle) * ProjTime - (9.807f * 0.25f) * ProjTime * (ProjTime / 2);
                pos.Position = new Vector3(x, y, z);
                collider.UpdatePosition(pos.Position);
                ProjTime += dt;
            }
        }

        private void CreateSystems()
        {
            ISystem newSystem;

            ComponentPosition pos = (ComponentPosition)entityManager.FindEntity("Banana").FindComponent(ComponentTypes.COMPONENT_POSITION);

            newSystem = new SystemRender();
            systemManager.AddSystem(newSystem);
            newSystem = new SystemCollision(pos.Position);
            systemManager.AddSystem(newSystem);
            newSystem = new SystemPhysics(ref sceneManager.cam);
            systemManager.AddSystem(newSystem);
            newSystem = new SystemSkyBox(ref sceneManager.cam);
            systemManager.AddSystem(newSystem);

        }

        public override void Render(FrameEventArgs e)
        {
            GL.Viewport(0, 0, sceneManager.Width, sceneManager.Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            systemManager.ActionSystem(entityManager);
            GUI.clearColour = Color.Transparent;

            //Depending on inputNum the game shows what to input for the player
            if (InputNum == 0)
            {
                Angle = Input;
            }
            else
            if(InputNum == 1)
            {
                Power = Input;
            }

            float width = sceneManager.Width,
                height = sceneManager.Height,
                fontSize = Math.Min(width, height) / 20f;
            int middleX = (int)(width / 2),
                middleY = (int)(height / 2);

            //Draws Scoreboard and the Player's chosen names.
            GUI.Label(new Rectangle(0, 0 - (int)(fontSize / 4.0f), (int)width, (int)(fontSize * 2f)), Score1 + " > Score < " + Score2, (int)(fontSize * 0.4f), StringAlignment.Center);
            GUI.Label(new Rectangle(-middleX + (Player1.Length * 10) + (int)(width * 0.01f), 0 - (int)(fontSize / 4.0f), (int)width, (int)(fontSize * 2f)), Player1, (int)(fontSize * 0.5f), StringAlignment.Center);
            GUI.Label(new Rectangle(middleX - (Player2.Length * 10) - (int)(width * 0.01f), 0 - (int)(fontSize / 4.0f), (int)width, (int)(fontSize * 2f)), Player2, (int)(fontSize * 0.5f), StringAlignment.Center);

            //Draws Player1 UI
            if (isPlayer1)
            {
                GUI.Label(new Rectangle(-middleX + (Player1.Length * 10) + (int)(width * 0.01f), 0 + (int)(fontSize / 1.5f), (int)width, (int)(fontSize * 2f)), "Angle: " + Angle, (int)(fontSize * 0.5f), StringAlignment.Center);
                if (InputNum >= 1)
                {
                    GUI.Label(new Rectangle(-middleX + (Player1.Length * 10) + (int)(width * 0.01f), 0 + (int)(fontSize * 1.5f), (int)width, (int)(fontSize * 2f)), "Power: " + Power, (int)(fontSize * 0.5f), StringAlignment.Center);
                }
            }

            // Draws Player2 UI
            if (!isPlayer1)
            {
                GUI.Label(new Rectangle(middleX - (Player2.Length * 10) - (int)(width * 0.01f), 0 + (int)(fontSize / 1.5f), (int)width, (int)(fontSize * 2f)), "Angle: " + Angle, (int)(fontSize * 0.5f), StringAlignment.Center);
                if (InputNum >= 1)
                {
                    GUI.Label(new Rectangle(middleX - (Player2.Length * 10) - (int)(width * 0.01f), 0 + (int)(fontSize * 1.5f), (int)width, (int)(fontSize * 2f)), "Power: " + Power, (int)(fontSize * 0.5f), StringAlignment.Center);
                }
            }

            GUI.Render();
        }
        
        private void LoadAudFiles()
        {
            SoundEffect = new Audio(ResourceManager.LoadAudio("Audio/throw.wav"));
        }

        private void LoadGameState()
        {
            StreamReader r = new StreamReader("Gorillas3D.txt");
            string[] line = r.ReadToEnd().Split('\n');
            Player1 = line[1].Substring(10, line[1].ToString().Length - 11);
            Player2 = line[2].Substring(10, line[2].ToString().Length - 11);
            Rounds = int.Parse(line[3].Substring(10, line[3].ToString().Length - 11));

            string[] score = line[4].Substring(10, line[4].ToString().Length - 11).Split('-');
            Score1 = int.Parse(score[0]);
            Score2 = int.Parse(score[1]);
            r.Close();
        }

        private void WriteGameState()
        {
            StreamReader r = new StreamReader("Gorillas3D.txt");
            string[] line = r.ReadToEnd().Split('\n');
            string nRound = line[3].Substring(0, 10) + (Rounds - 1);
            string nScore = line[4].Substring(0, 10) + Score1 + "-" + Score2;
            line[3] = nRound;
            line[4] = nScore;
            r.Close();
            File.WriteAllText("Gorillas3D.txt", string.Empty);
            StreamWriter w = new StreamWriter("Gorillas3D.txt");
            for (int i = 0; i < line.Length; i++)
            {
                w.WriteLine(line[i]);
            }
            w.Close();
        }

        private void Keyboard_KeyDown(object sender, KeyboardKeyEventArgs e)
        {
            string num = "";
            if (isPlayer1)
            {
                switch (e.Key)
                {
                    // Handles input of numbers
                    case Key.Number0:
                        num += "0";
                        break;
                    case Key.Number1:
                        num += "1";
                        break;
                    case Key.Number2:
                        num += "2";
                        break;
                    case Key.Number3:
                        num += "3";
                        break;
                    case Key.Number4:
                        num += "4";
                        break;
                    case Key.Number5:
                        num += "5";
                        break;
                    case Key.Number6:
                        num += "6";
                        break;
                    case Key.Number7:
                        num += "7";
                        break;
                    case Key.Number8:
                        num += "8";
                        break;
                    case Key.Number9:
                        num += "9";
                        break;
						
                    case Key.Enter:
                        switch (InputNum)
                        {
                            case 0:
                                Angle = Input;
                                Input = "";                 
                                InputNum++;                  
                                break;
                            case 1:
                                Power = Input;
                                Input = "";                 
                                InputNum++;
                                break;
                        }
                        break;
                }
            }

            Input += num;
        }


        public override void Close()
        {
            entityManager.DeleteEntities();
            sceneManager.Keyboard.KeyDown -= Keyboard_KeyDown;
        }
    }
}