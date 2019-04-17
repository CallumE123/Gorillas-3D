#version 330
 
in vec2 v_TexCoord;
uniform sampler2D s_Tex;

out vec4 Color;
 
void main()
{
    Color = texture2D(s_Tex, v_TexCoord);
}