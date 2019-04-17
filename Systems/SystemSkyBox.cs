using Gorillas3D.Components;
using Gorillas3D.Objects;
using Gorillas3D.Scenes;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.IO;

namespace Gorillas3D.Systems
{
    class SystemSkyBox : ISystem
    {
        const ComponentTypes MASK = (ComponentTypes.COMPONENT_SKY | ComponentTypes.COMPONENT_GEOMETRY | ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_TRANSFORM);

        Camera mCamera;
        protected int pgmID;
        protected int vsID;
        protected int fsID;
        protected int attribute_vpos;
        protected int uniform_stex;
        protected int uniform_mview;
        protected int uniform_mtransform;

        public SystemSkyBox(ref Camera pCam)
        {
            pgmID = GL.CreateProgram();
            LoadShader("Shaders/skyboxVert.glsl", ShaderType.VertexShader, pgmID, out vsID);
            LoadShader("Shaders/skyboxFrag.glsl", ShaderType.FragmentShader, pgmID, out fsID);
            GL.LinkProgram(pgmID);
            Console.Write(GL.GetProgramInfoLog(pgmID));

            attribute_vpos = GL.GetAttribLocation(pgmID, "aPos");
            uniform_mview = GL.GetUniformLocation(pgmID, "WorldViewProj");
            uniform_stex = GL.GetUniformLocation(pgmID, "skybox");
            uniform_mtransform = GL.GetUniformLocation(pgmID, "Transform");
            mCamera = pCam;

            if (attribute_vpos == -1 || uniform_stex == -1 || uniform_mview == -1 || uniform_mtransform == -1)
            {
                Console.WriteLine("Error binding attributes");
            }
        }

        void LoadShader(String filename, ShaderType type, int program, out int address)
        {
            address = GL.CreateShader(type);
            using (StreamReader sr = new StreamReader(filename))
            {
                GL.ShaderSource(address, sr.ReadToEnd());
            }
            GL.CompileShader(address);
            GL.AttachShader(program, address);
            Console.WriteLine(GL.GetShaderInfoLog(address));
        }
        public string Name
        {
            get { return "SystemSkyBox"; }
        }
        public void OnAction(Entity entity)
        {
            if ((entity.Mask & MASK) == MASK)
            {
                List<IComponent> components = entity.Components;

                IComponent skyboxComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_SKY;
                });
                int texture = ((ComponentSky)skyboxComponent).Texture;

                IComponent positionComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_POSITION;
                });
                Vector3 position = ((ComponentPosition)positionComponent).Position = mCamera.Position;

                IComponent geometryComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_GEOMETRY;
                });
                Geometry geometry = ((ComponentGeometry)geometryComponent).Geometry();

                IComponent transformComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_TRANSFORM;
                });
                Matrix4 transform = ((ComponentTransform)transformComponent).Transform;


                Draw(texture, position, geometry, transform);
            }
        }
        public void Draw(int texture, Vector3 position, Geometry geometry, Matrix4 transform)
        {
            GL.UseProgram(pgmID);
            GL.Uniform1(uniform_stex, 0);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.TextureCubeMap, texture);
            GL.Enable(EnableCap.TextureCubeMap);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Front);
            Matrix4 world = Matrix4.CreateTranslation(position);
            Matrix4 worldViewProjection = world * GameScene.instance.view * GameScene.instance.projection;
            GL.UniformMatrix4(uniform_mview, false, ref worldViewProjection);
            GL.UniformMatrix4(uniform_mtransform, true, ref transform);

            geometry.Render();
            GL.Disable(EnableCap.CullFace);
            GL.Disable(EnableCap.TextureCubeMap);
            GL.BindVertexArray(0);
            GL.UseProgram(0);
        }
    }
}
