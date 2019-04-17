using System;
using OpenTK;
using OpenTK.Input;

namespace Gorillas3D.Objects
{
    class Camera
    {
        public Vector3 Position = Vector3.Zero;
        public Vector3 Orientation = new Vector3((float)Math.PI, 0f, 0f);
        public float Velocity = 0.1f;
        public float MouseSensitivity = 1f;
        public int width = 1600, height = 900;

        public Matrix4 GetViewMatrix()
        {
            Vector3 lookat = new Vector3();

            lookat.X = (float)(Math.Sin((float)Orientation.X) * Math.Cos((float)Orientation.Y));
            lookat.Y = (float)Math.Sin((float)Orientation.Y);
            lookat.Z = (float)(Math.Cos((float)Orientation.X) * Math.Cos((float)Orientation.Y));

            return Matrix4.LookAt(Position, Position + lookat, Vector3.UnitY);
        }

        public void Move(float x, float y, float z)
        {
            Vector3 offset = new Vector3();

            Vector3 forward = new Vector3((float)Math.Sin((float)Orientation.X),
                0, (float)Math.Cos((float)Orientation.X));
            Vector3 right = new Vector3(-forward.Z, 0, forward.X);

            offset += x * right;
            offset += y * forward;
            offset.Y += z;

            offset.NormalizeFast();
            offset = Vector3.Multiply(offset, Velocity);

            Position += offset;
        }

        public void Rotation(float x, float y)
        {
            x = x * MouseSensitivity;
            y = y * MouseSensitivity;

            Orientation.X = (Orientation.X + x) % ((float)Math.PI * 2.0f);
            Orientation.Y = Math.Max(Math.Min(Orientation.Y + y,
                (float)Math.PI / 2.0f - 0.1f),
                (float)-Math.PI / 2.0f + 0.1f);
        }

        public void ReloadCamera()
        {
            Position = Vector3.Zero;
            Orientation = new Vector3((float)Math.PI, 0f, 0f);
        }

        public Vector3 Positon {
            get { return this.Position; }
            set { this.Position = value; }
        }


    }
}
