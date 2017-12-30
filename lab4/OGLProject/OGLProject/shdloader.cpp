#include "shdloader.hpp"

GLuint LoadShaders(const char * vertexShaderFilePath, const char * fragmentShaderFilePath) {

	// Create the shaders
	GLuint vertexShaderID = glCreateShader(GL_VERTEX_SHADER);
	GLuint fragmentShaderID = glCreateShader(GL_FRAGMENT_SHADER);

	// Read the code from files
	std::string vStr = ReadShaderCode(vertexShaderFilePath);
	std::string fStr = ReadShaderCode(fragmentShaderFilePath);
	char const * vertexShaderCode = vStr.c_str();
	char const * fragmentShaderCode = fStr.c_str();


	// Compile Vertex Shader
	printf("Compiling shader : %s\n", vertexShaderFilePath);
	CompileShader(vertexShaderID, vertexShaderCode);

	// Compile Fragment Shader
	printf("Compiling shader : %s\n", fragmentShaderFilePath);
	CompileShader(fragmentShaderID, fragmentShaderCode);


	// Link
	printf("Linking program\n");
	GLuint programID = glCreateProgram();
	glAttachShader(programID, vertexShaderID);
	glAttachShader(programID, fragmentShaderID);
	glLinkProgram(programID);

	int infoLogLen = 0;
	// Check result
	glGetProgramiv(programID, GL_INFO_LOG_LENGTH, &infoLogLen);
	if (infoLogLen > 0) 
	{
		std::vector<char> errorMessage(infoLogLen + 1);
		glGetProgramInfoLog(programID, infoLogLen, NULL, &errorMessage[0]);
		printf("%s\n", &errorMessage[0]);
	}


	glDetachShader(programID, vertexShaderID);
	glDetachShader(programID, fragmentShaderID);

	glDeleteShader(vertexShaderID);
	glDeleteShader(fragmentShaderID);

	return programID;
}

std::string ReadShaderCode(const char * filePath)
{
	std::stringstream sstr;
	std::ifstream stream(filePath);
	sstr << stream.rdbuf();
	return sstr.str();
}

bool CompileShader(int shaderId, const char * code)
{
	int infoLogLen;
	GLint result = GL_FALSE;

	// Compile
	glShaderSource(shaderId, 1, &code, NULL);
	glCompileShader(shaderId);

	// Check compilation result
	glGetShaderiv(shaderId, GL_COMPILE_STATUS, &result);
	glGetShaderiv(shaderId, GL_INFO_LOG_LENGTH, &infoLogLen);
	if (result != GL_TRUE)
	{
		std::vector<char> errorMessage(infoLogLen + 1);
		glGetShaderInfoLog(shaderId, infoLogLen, NULL, &errorMessage[0]);
		printf("%s\n", &errorMessage[0]);
		return false;
	}
	return true;
}


