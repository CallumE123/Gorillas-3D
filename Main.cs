using System;
using Gorillas3D.Managers;
using System.Collections.Generic;
using System.Linq;

namespace Gorillas3D
{
    public static class MainPoint
    {
        [STAThread]
        static void Main()
        {
            using (var game = new SceneManager()) game.Run(60);
        }
    }
}
