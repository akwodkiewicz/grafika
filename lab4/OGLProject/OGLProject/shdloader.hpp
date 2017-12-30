#pragma once
#include <stdio.h>
#include <string>
#include <vector>
#include <iostream>
#include <fstream>
#include <algorithm>
#include <sstream>
#include <stdlib.h>
#include <string.h>

#include <GL\glew.h>

GLuint LoadShaders(const char * vertexShaderFilePath, const char * fragmentShaderFilePath);
std::string ReadShaderCode(const char * filePath);
bool CompileShader(int shaderId, const char * code);
