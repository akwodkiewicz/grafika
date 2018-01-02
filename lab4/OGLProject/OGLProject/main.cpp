#include <stdio.h>
#include <stdlib.h>

#include <GL\glew.h>
#include <GLFW\glfw3.h>

#include <glm\glm.hpp>
#include <glm\gtc\matrix_transform.hpp>
#include <glm\ext.hpp>

#include "shdloader.hpp"

void keyPressCallback(GLFWwindow* window, int key, int scancode, int action, int mods);
glm::vec3 getCameraPosition(int cameraId);
glm::mat4 createViewMatrix(glm::vec3 cameraPos, glm::vec3 cameraTarget, glm::vec3 up = glm::vec3(0.0f, 1.0f, 0.0f));
glm::mat4 createProjectionMatrix(float near, float far, float fov_rad, float aspect_ratio);

// PLANE
//---------------------------------
static const GLfloat vertices[] = {
	-1.0f,  0.0f, -1.0f,		0.0f, 1.0f, 0.0f,
	 1.0f,  0.0f, -1.0f,		0.0f, 1.0f, 0.0f,
	 0.0f,  0.0f,  1.0f,		0.0f, 1.0f, 0.0f,

	 -1.0f,  0.0f, -1.0f,		0.0f, -1.0f, 0.0f,
	 -0.33f, -0.02f, -1.0f,		0.0f, -1.0f, 0.0f,
	 0.0f,  0.0f,  1.0f,		0.0f, -1.0f, 0.0f,

	 1.0f,  0.0f, -1.0f,		0.0f, -1.0f, 0.0f,
	 0.33f, -0.02f, -1.0f,		0.0f, -1.0f, 0.0f,
	 0.0f,  0.0f,  1.0f,		0.0f, -1.0f, 0.0f,

	 0.0f,  0.0f,  1.0f, 		-1.0f, 0.0f, 0.0f,
	 0.0f,  -0.33f, -1.0f,		-1.0f, 0.0f, 0.0f,
	-0.33f,  0.0f,  -1.0f,		-1.0f, 0.0f, 0.0f,

	 0.0f,  0.0f, 1.0f,			1.0f, 0.0f, 0.0f,
	 0.0f, -0.33f, -1.0f,		1.0f, 0.0f, 0.0f,
	 0.33f, 0.0f, -1.0f,		1.0f, 0.0f, 0.0f,

	 0.0f, -0.33f, -1.0f,		0.0f, 0.0f, -1.0f,
	-0.33f, 0.0f,  -1.0f,		0.0f, 0.0f, -1.0f,
	 0.33f, 0.0f,  -1.0f,		0.0f, 0.0f, -1.0f
};

// CUBE
//-----------------------------------
static const GLfloat vertices2[] = {
	-0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
	0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
	0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
	0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
	-0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
	-0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,

	-0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
	0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
	0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
	0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
	-0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
	-0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,

	-0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f,
	-0.5f,  0.5f, -0.5f, -1.0f,  0.0f,  0.0f,
	-0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,
	-0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,
	-0.5f, -0.5f,  0.5f, -1.0f,  0.0f,  0.0f,
	-0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f,

	0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f,
	0.5f,  0.5f, -0.5f,  1.0f,  0.0f,  0.0f,
	0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,
	0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,
	0.5f, -0.5f,  0.5f,  1.0f,  0.0f,  0.0f,
	0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f,

	-0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,
	0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,
	0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,
	0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,
	-0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,
	-0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,

	-0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,
	0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,
	0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,
	0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,
	-0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,
	-0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f
};

static const glm::vec3 LIGHT_POSITION(0.0f, 7.0f, 0.0f);
static const glm::vec3 LIGHT_COLOR(1.0f, 1.0f, 1.0f);
static const glm::vec3 PLANE_COLOR(0.04f, 0.3f, 0.5f);
static const glm::vec3 GROUND_COLOR(0.34f, 0.31f, 0.34f);

int cameraId = 1;
int aspectRatioChanged = 1;
int currentWidth;
int currentHeight;
int specularPower = 32;
int main()
{
	// Initialize GLFW
	//-------------------------------------------------
	if (!glfwInit())
	{
		fprintf(stderr, "Failed to initialize GLFW\n");
		getchar();
		return -1;
	}


	// Create window
	//-------------------------------------------------
	GLFWmonitor* monitor = glfwGetPrimaryMonitor();
	const GLFWvidmode* mode = glfwGetVideoMode(monitor);
	GLFWwindow* window = glfwCreateWindow(mode->width / 1.2, mode->height / 1.2, "Project 4", NULL, NULL);
	if (!window)
	{
		fprintf(stderr, "Failed to open GLFW window\n");
		getchar();
		glfwTerminate();
		return -1;
	}
	glfwMakeContextCurrent(window); // Needed for GLEW initializer
	currentHeight = mode->height;
	currentWidth = mode->width;


	// Resize content on window resize
	//-------------------------------------------------
	glfwSetFramebufferSizeCallback(window, [](GLFWwindow* window, int width, int height) {
		glViewport(0, 0, width, height);
		currentWidth = width;
		currentHeight = height;
		aspectRatioChanged = 1;
	});


	// Initialize GLEW
	//-------------------------------------------------
	glewExperimental = true; // Needed for core profile
	if (glewInit() != GLEW_OK) {
		fprintf(stderr, "Failed to initialize GLEW\n");
		getchar();
		glfwTerminate();
		return -1;
	}


	// Prepare all buffers
	//-------------------------------------------------
	GLuint VAOs[3], VBOs[2], EBO;
	glGenVertexArrays(3, VAOs); // Vertex array object
	glGenBuffers(2, VBOs);	// Vertex buffer object


	// Plane model
	glBindVertexArray(VAOs[0]); // Set active

	glBindBuffer(GL_ARRAY_BUFFER, VBOs[0]); // Bind VBO to VAO
	glBufferData(GL_ARRAY_BUFFER, sizeof(vertices), vertices, GL_STATIC_DRAW); // Copy data

	glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 6 * sizeof(float), (void*)0); // Set the interpretation of Vertex Buffer data
	glEnableVertexAttribArray(0);
	glVertexAttribPointer(1, 3, GL_FLOAT, GL_FALSE, 6 * sizeof(float), (void*)(3 * sizeof(float))); // Set the interpretation of Vertex Buffer data
	glEnableVertexAttribArray(1);

	// Cube model
	glBindVertexArray(VAOs[1]);

	glBindBuffer(GL_ARRAY_BUFFER, VBOs[1]); // Bind VBO to VAO
	glBufferData(GL_ARRAY_BUFFER, sizeof(vertices2), vertices2, GL_STATIC_DRAW); // Copy data

	glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 6 * sizeof(float), (void*)0); // Set the interpretation of Vertex Buffer data
	glEnableVertexAttribArray(0);
	glVertexAttribPointer(1, 3, GL_FLOAT, GL_FALSE, 6 * sizeof(float), (void*)(3 * sizeof(float))); // Set the interpretation of Vertex Buffer data
	glEnableVertexAttribArray(1);


	// Light model
	glBindVertexArray(VAOs[2]);
	glBindBuffer(GL_ARRAY_BUFFER, VBOs[1]); // Bind VBO to VAO
	// Data is already copied
	glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 6 * sizeof(float), (void*)0); // Set the interpretation of Vertex Buffer data
	glEnableVertexAttribArray(0);

	// Complie and use shaders
	//-------------------------------------------------
	GLuint programID = LoadShaders("vertex.shader", "fragment.shader");
	glUseProgram(programID);

	GLuint program2ID = LoadShaders("vertex.shader", "fragment_onecolor.shader");


	// Get shader variable handles
	//-------------------------------------------------
	unsigned int modelLoc = glGetUniformLocation(programID, "model");
	unsigned int viewLoc = glGetUniformLocation(programID, "view");
	unsigned int projLoc = glGetUniformLocation(programID, "projection");
	unsigned int objectColorLoc = glGetUniformLocation(programID, "objectColor");
	unsigned int lightColorLoc = glGetUniformLocation(programID, "lightColor");
	unsigned int lightPosLoc = glGetUniformLocation(programID, "lightPos");
	unsigned int viewPosLoc = glGetUniformLocation(programID, "viewPos");
	unsigned int specularPowerLoc = glGetUniformLocation(programID, "specularPower");


	unsigned int modelLoc2 = glGetUniformLocation(program2ID, "model");
	unsigned int viewLoc2 = glGetUniformLocation(program2ID, "view");
	unsigned int projLoc2 = glGetUniformLocation(program2ID, "projection");

	// Set background color
	//-------------------------------------------------
	glClearColor(0.2f, 0.2f, 0.4f, 0.0f);


	// Set light color and position
	//-------------------------------------------------
	glUniform3fv(lightColorLoc, 1, glm::value_ptr(LIGHT_COLOR));
	glUniform3fv(lightPosLoc, 1, glm::value_ptr(LIGHT_POSITION));



	// Set wireframe mode
	//-------------------------------------------------
	//glPolygonMode(GL_FRONT_AND_BACK, GL_LINE);


	// Enable depth testing
	//-------------------------------------------------
	glEnable(GL_DEPTH_TEST);


	// Calculate projection matrix and send it to shader
	//-------------------------------------------------
	glm::mat4 projection = createProjectionMatrix(1.0f, 100.0f, glm::radians(45.0f), (float)mode->height/(float)mode->width);
	glUniformMatrix4fv(projLoc, 1, GL_FALSE, &projection[0][0]);


	// Register callbacks for keypresses
	//-------------------------------------------------
	glfwSetKeyCallback(window, keyPressCallback);


	// Event loop
	//-------------------------------------------------
	while (!glfwWindowShouldClose(window))
	{
		glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
		float currentTime = glfwGetTime();

		float radius = 10.0f;
		float orbitX = sin(currentTime) * radius;
		float orbitZ = cos(currentTime) * radius;
		float camY = (sin(currentTime / 4)) * 3;

		glUseProgram(programID);

		// Calculate new projection matrix has the aspect ratio changed
		//-------------------------------------------------
		if (aspectRatioChanged)
		{
			projection = createProjectionMatrix(1.0f, 100.0f, glm::radians(95.0f), (float)currentHeight / (float)currentWidth);
			glUniformMatrix4fv(projLoc, 1, GL_FALSE, &projection[0][0]);
			aspectRatioChanged = 0;
		}

		// Set specular light power
		//-------------------------------------------------
		glUniform1i(specularPowerLoc, specularPower);


		// Set camera (calculate `view` matrix)
		//-------------------------------------------------
		glm::vec3 cameraPos = getCameraPosition(cameraId);
		cameraPos.y += camY;
		glUniform3fv(viewPosLoc, 1, glm::value_ptr(cameraPos));

		glm::vec3 cameraTarget;
		if (cameraId == 3)
			cameraTarget = glm::vec3(orbitX, 3.0f, orbitZ);
		else
			cameraTarget = glm::vec3(0.0f, 0.5f, 0.5f);
		glm::vec3 up = glm::vec3(0.0f, 1.0f, 0.0f);
		glm::mat4 view = createViewMatrix(cameraPos, cameraTarget, up);
		glUniformMatrix4fv(viewLoc, 1, GL_FALSE, &view[0][0]);



		// Draw plane
		//-------------------------------------------------
		glBindVertexArray(VAOs[0]);

		glm::mat4 modelPlane = glm::translate(glm::mat4(),/*glm::lookAt(glm::vec3(yawAngle, -3.0f, orbitZ), glm::vec3(0.0f, 0.0f, 0.0f), up)*/glm::vec3(orbitX, 3.0f, orbitZ));
		glUniform3fv(objectColorLoc, 1, glm::value_ptr(PLANE_COLOR));
		glUniformMatrix4fv(modelLoc, 1, GL_FALSE, &modelPlane[0][0]); // Send model matrix to shader
		glDrawArrays(GL_TRIANGLES, 0, 54);


		// Draw ground
		//-------------------------------------------------
		glBindVertexArray(VAOs[1]);

		glm::mat4 modelCube;
		glUniform3fv(objectColorLoc, 1, glm::value_ptr(GROUND_COLOR));	

		modelCube = glm::translate(glm::scale(glm::mat4(), glm::vec3(50.0f, 0.5f, 50.0f)), glm::vec3(0.0f, -0.5f, 0.0f));
		glUniformMatrix4fv(modelLoc, 1, GL_FALSE, &modelCube[0][0]); // Send model matrix to shader
		glDrawArrays(GL_TRIANGLES, 0, 36);


		// Draw light cube
		//--------------------------------------------------
		glUseProgram(program2ID);
		glBindVertexArray(VAOs[2]);

		modelCube =  glm::translate(glm::mat4(), LIGHT_POSITION);

		glUniformMatrix4fv(projLoc2, 1, GL_FALSE, &projection[0][0]);
		glUniformMatrix4fv(viewLoc2, 1, GL_FALSE, &view[0][0]);
		glUniformMatrix4fv(modelLoc2, 1, GL_FALSE, &modelCube[0][0]);
		glDrawArrays(GL_TRIANGLES, 0, 36);


		glfwSwapBuffers(window);
		glfwPollEvents();
	}


	// Destructors
	//--------------------------------------------------
	glDeleteVertexArrays(3, VAOs);
	glDeleteBuffers(2, VBOs);
	glfwDestroyWindow(window);
	glfwTerminate();
	return 0;
}

void keyPressCallback(GLFWwindow* window, int key, int scancode, int action, int mods)
{
	if (action != GLFW_PRESS)
		return;
	switch (key)
	{
	case GLFW_KEY_1:
		cameraId = 1;
		std::cout << "Camera 1" << std::endl;
		break;
	case GLFW_KEY_2:
		cameraId = 2;
		std::cout << "Camera 2" << std::endl;
		break;
	case GLFW_KEY_3:
		cameraId = 3;
		std::cout << "Camera 3 (tracking)" << std::endl;
		break;
	case GLFW_KEY_EQUAL:
		if (specularPower != INT_MAX)
			specularPower <<= 1;
		std::cout << "Phong specular power = " << specularPower << std::endl;
		break;
	case GLFW_KEY_MINUS:
		if (specularPower != 2)
			specularPower >>= 1;
		std::cout << "Phong specular power = " << specularPower << std::endl;
		break;
	default:
		break;
	}
}

glm::vec3 getCameraPosition(int cameraId)
{
	switch (cameraId)
	{
	case 1:
		return glm::vec3(15.0f, 10.0f, 20.0f);
	case 2:
		return glm::vec3(0.0f, 30.0f, 20.0f);
	case 3:
		return glm::vec3(3.0f, 4.5f, -0.5f);
	}
}


glm::mat4 createViewMatrix(glm::vec3 cameraPos, glm::vec3 cameraTarget, glm::vec3 up)
{
	//glm::vec3 leftHandedCameraPos = glm::vec3(cameraPos.x, cameraPos.y, cameraPos.z);
	glm::vec3 zAxis = glm::normalize(cameraPos - cameraTarget);
	glm::vec3 xAxis = glm::normalize(glm::cross(up, zAxis));
	glm::vec3 yAxis = glm::cross(zAxis, xAxis);

	//glm::mat4 view_inverse = glm::mat4(
	//	xAxis.x, yAxis.x, zAxis.x, cameraPos.x,
	//	xAxis.y, yAxis.y, zAxis.y, cameraPos.y,
	//	xAxis.z, yAxis.z, zAxis.z, cameraPos.z,
	//		  0,       0,		0,			 1
	//);
	glm::mat3 t_1 = glm::mat3(
		xAxis.x, yAxis.x, zAxis.x, // first column
		xAxis.y, yAxis.y, zAxis.y, // second column
		xAxis.z, yAxis.z, zAxis.z //third column
	);
	glm::vec3 v = -t_1*cameraPos;

	glm::mat4 view = glm::mat4(t_1);
	view[3].x = v.x;
	view[3].y = v.y;
	view[3].z = v.z;

	return view;
}

glm::mat4 createProjectionMatrix(float near, float far, float fov_rad, float aspect_ratio)
{
	float e = 1 / (tan(fov_rad / 2));
	glm::mat4 projection;
	projection[0][0] = e;
	projection[1][1] = e / aspect_ratio;
	projection[2][2] = -((far + near) / (far - near));
	projection[2][3] = -1;
	projection[3][2] = -((2 * far*near) / (far - near));
	return projection;
}
