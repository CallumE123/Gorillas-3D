using System;
using System.Collections.Generic;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Gorillas3D.Components;
using Gorillas3D.Objects;
using Gorillas3D.Scenes;

namespace Gorillas3D.Systems
{
    class SystemRender : ISystem
    {
        const ComponentTypes MASK = (ComponentTypes.COMPONENT_GEOMETRY | ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_TEXTURE);

        protected int progID;
        protected int vertID;
        protected int fragID;
        protected int attribute_vtex;
        protected int attribute_vpos;
        protected int uniform_stex;
        protected int uniform_mview;
        protected int uniform_mtransform;

        public SystemRender()
        {
            progID = GL.CreateProgram();
            LoadShader("Shaders/verts.glsl", ShaderType.VertexShader, progID, out vertID);
            LoadShader("Shaders/frags.glsl", ShaderType.FragmentShader, progID, out fragID);
            GL.LinkProgram(progID);
            Console.WriteLine(GL.GetProgramInfoLog(progID));

            attribute_vpos = GL.GetAttribLocation(progID, "a_Pos");
            attribute_vtex = GL.GetAttribLocation(progID, "a_TexCoord");
            uniform_mview = GL.GetUniformLocation(progID, "WorldViewProj");
            uniform_stex = GL.GetUniformLocation(progID, "s_Tex");
            uniform_mtransform = GL.GetUniformLocation(progID, "Transform");

            if (attribute_vpos == -1 || attribute_vtex == -1 || uniform_stex == -1 || uniform_mview == -1|| uniform_mtransform == -1)
            {
                Console.WriteLine("Error binding attributes");
            }
        }

        void LoadShader(string filename, ShaderType type, int prog, out int address)
        {
            address = GL.CreateShader(type);
            using (StreamReader sr = new StreamReader(filename))
            {
                GL.ShaderSource(address, sr.ReadToEnd());
            }
            GL.CompileShader(address);
            GL.AttachShader(prog, address);
            Console.WriteLine(GL.GetShaderInfoLog(address));
        }

        public string Name
        {
            get { return "SystemRender"; }
        }

        public void OnAction(Entity entity)
        {
            if ((entity.Mask & MASK) == MASK)
            {
                List<IComponent> components = entity.Components;

                IComponent geometryComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_GEOMETRY;
                });
                Geometry geometry = ((ComponentGeometry)geometryComponent).Geometry();

                IComponent positionComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_POSITION;
                });
                Vector3 position = ((ComponentPosition)positionComponent).Position;
                Matrix4 world = Matrix4.CreateTranslation(position);

                IComponent textureComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_TEXTURE;
                });
                int texture = ((ComponentTexture)textureComponent).Texture;

                IComponent transformComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_TRANSFORM;
                });
                Matrix4 transform = ((ComponentTransform)transformComponent).Transform;

                Draw(world, geometry, texture, transform);
            }
        }

        public void Draw(Matrix4 world, Geometry geometry, int texture, Matrix4 transform)
        {
            GL.UseProgram(progID);

            GL.Uniform1(uniform_stex, 0);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, texture);
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);

            Matrix4 worldviewProjection = world * GameScene.instance.view * GameScene.instance.projection;
            GL.UniformMatrix4(uniform_mview, false, ref worldviewProjection);
            GL.UniformMatrix4(uniform_mtransform, true, ref transform);
            geometry.Render();

            GL.Disable(EnableCap.CullFace);
            GL.Disable(EnableCap.Texture2D);
            GL.BindVertexArray(0);
            GL.UseProgram(0);
        }
    }
}
