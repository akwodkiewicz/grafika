#version 330 core
out vec4 FragColor;

uniform vec3 oneColor;
void main()
{
    FragColor = vec4(oneColor, 1.0);
}