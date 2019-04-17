#version 330
layout (location = 0) in vec3 aPos;

out vec3 TexCoords;

uniform mat4 WorldViewProj;
uniform mat4 Transform;

void main()
{
    TexCoords = aPos;
    gl_Position = WorldViewProj * Transform * vec4(aPos, 1.0);
} 