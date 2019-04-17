using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Drawing.Imaging;

namespace Gorillas3D.Scenes
{
    static class GUI
    {
        static private Bitmap textBMP;
        static Bitmap TextBMP
        {
            get { return textBMP; }
            set { }
        }

        static private int textTexture;
        static private Graphics textGPH;

        static private int gWidth, gHeight;
        static public Vector2 guiPos = Vector2.Zero;

        static public Color clearColour = Color.Black;

        public static void SetUpGUI(int width, int height)
        {
            gWidth = width;
            gHeight = height;

            textBMP = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            textGPH = Graphics.FromImage(textBMP);
            textGPH.Clear(clearColour);

            if (textTexture > 0)
            {
                GL.DeleteTexture(textTexture);
                textTexture = 0;
            }
            textTexture = GL.GenTexture();
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, textTexture);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.Linear);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, textBMP.Width, textBMP.Height,
                0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, IntPtr.Zero);
            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.Disable(EnableCap.Texture2D);
        }

        static public void Label(Rectangle rect, string text)
        {
            Label(rect, text, 20, StringAlignment.Near);
        }
        static public void Label(Rectangle rect, string text, StringAlignment sa)
        {
            Label(rect, text, 20, sa);
        }
        static public void Label(Rectangle rect, string text, int fontSize)
        {
            Label(rect, text, fontSize, StringAlignment.Near);
        }

        static public void Label(Rectangle rect, string text, int fontSize, StringAlignment sa)
        {
            Label(rect, text, fontSize, sa, Color.White);
        }

        static public void Label(Rectangle rect, string text, int fontSize, StringAlignment sa, Color color)
        {
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = sa;
            stringFormat.LineAlignment = sa;

            SolidBrush brush = new SolidBrush(color);

            textGPH.DrawString(text, new Font("Forte", fontSize), brush, rect, stringFormat);
        }

        static public void Render()
        {
            // Enable the texture
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.One, BlendingFactorDest.OneMinusSrcAlpha);

            GL.Enable(EnableCap.Texture2D);
            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);

            GL.BindTexture(TextureTarget.Texture2D, textTexture);

            BitmapData data = textBMP.LockBits(new Rectangle(0, 0, textBMP.Width, textBMP.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, (int)textBMP.Width, (int)textBMP.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            textBMP.UnlockBits(data);

            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0f, 1f); GL.Vertex2(guiPos.X, guiPos.Y);
            GL.TexCoord2(1f, 1f); GL.Vertex2(guiPos.X + gWidth, guiPos.Y);
            GL.TexCoord2(1f, 0f); GL.Vertex2(guiPos.X + gWidth, guiPos.Y + gHeight);
            GL.TexCoord2(0f, 0f); GL.Vertex2(guiPos.X, guiPos.Y + gHeight);
            GL.End();

            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.Disable(EnableCap.Texture2D);

            textGPH.Clear(clearColour);
        }
    }
}
