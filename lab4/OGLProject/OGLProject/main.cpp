#include <stdio.h>
#include <stdlib.h>

#include <GL\glew.h>
#include <GLFW\glfw3.h>

#include <glm\glm.hpp>
#include <glm\gtc\matrix_transform.hpp>

#include "shdloader.hpp"

void keyPressCallback(GLFWwindow* window, int key, int scancode, int action, int mods);
glm::vec3 getCameraPosition(int cameraId);

// PLANE
//---------------------------------
static const GLfloat vertices[] = {
	-1.0f,  0.0f, -1.0f,		1.0f, 0.0f, 0.0f,
	 1.0f,  0.0f, -1.0f,		1.0f, 1.0f, 0.0f,
	 0.0f,  0.0f,  1.0f,		1.0f, 1.0f, 0.0f,

	 //0.0f,  0.0f,   1.0f, 	
	  0.0f,  -0.33f, -1.0f,		1.0f, 0.5f, 0.5f,
	 -0.33f,  0.0f,  -1.0f,		1.0f, 0.5f, 0.5f,

	 //0.0f,  0.0f, 1.0f,  
	 0.0f, -0.33f, -1.0f,		1.0f, 0.5f, 0.5f,
	 0.33f, 0.0f, -1.0f,			1.0f, 0.5f, 0.5f,

	 0.0f, -0.33f, -1.0f,		1.0f, 0.5f, 0.5f
	 //-0.33f, -1.0f,  0.0f,
	 //0.33f, -1.0f,  0.0f
};
static GLuint indices[] = {
	0, 1, 2,
	2, 3, 4,
	2, 5, 6,
	7, 4, 6
};

// CUBE
//-----------------------------------
static const GLfloat vertices2[] = {
	-0.5f, -0.5f, -0.5f,
	 0.5f, -0.5f, -0.5f,
	 0.5f,  0.5f, -0.5f,
	 0.5f,  0.5f, -0.5f,
	-0.5f,  0.5f, -0.5f,
	-0.5f, -0.5f, -0.5f,

	-0.5f, -0.5f,  0.5f,
	 0.5f, -0.5f,  0.5f,
	 0.5f,  0.5f,  0.5f,
	 0.5f,  0.5f,  0.5f,
	-0.5f,  0.5f,  0.5f,
	-0.5f, -0.5f,  0.5f,

	-0.5f,  0.5f,  0.5f,
	-0.5f,  0.5f, -0.5f,
	-0.5f, -0.5f, -0.5f,
	-0.5f, -0.5f, -0.5f,
	-0.5f, -0.5f,  0.5f,
	-0.5f,  0.5f,  0.5f,

	 0.5f,  0.5f,  0.5f,
	 0.5f,  0.5f, -0.5f,
	 0.5f, -0.5f, -0.5f,
	 0.5f, -0.5f, -0.5f,
	 0.5f, -0.5f,  0.5f,
	 0.5f,  0.5f,  0.5f,

	-0.5f, -0.5f, -0.5f,
	 0.5f, -0.5f, -0.5f,
	 0.5f, -0.5f,  0.5f,
	 0.5f, -0.5f,  0.5f,
	-0.5f, -0.5f,  0.5f,
	-0.5f, -0.5f, -0.5f,

	-0.5f,  0.5f, -0.5f,
	 0.5f,  0.5f, -0.5f,
	 0.5f,  0.5f,  0.5f,
	 0.5f,  0.5f,  0.5f,
	-0.5f,  0.5f,  0.5f,
	-0.5f,  0.5f, -0.5f
};


int cameraId = 1;

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


	// Resize content on window resize
	//-------------------------------------------------
	glfwSetFramebufferSizeCallback(window, [](GLFWwindow* window, int width, int height) {
		glViewport(0, 0, width, height);
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
	GLuint VBOs[2], VAOs[2], EBO;
	glGenVertexArrays(2, VAOs); // Buffer array object
	glGenBuffers(2, VBOs);	// Vertex buffer object
	glGenBuffers(1, &EBO); // Element buffer object - keeps data about indices


	// Plane model
	glBindVertexArray(VAOs[0]); // Set active

	glBindBuffer(GL_ARRAY_BUFFER, VBOs[0]); // Set active
	glBufferData(GL_ARRAY_BUFFER, sizeof(vertices), vertices, GL_STATIC_DRAW); // Copy data

	glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, EBO); // Set active
	glBufferData(GL_ELEMENT_ARRAY_BUFFER, sizeof(indices), indices, GL_STATIC_DRAW); // Copy data

	glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 6 * sizeof(float), (void*)0); // Set the interpretation of Vertex Buffer data
	glEnableVertexAttribArray(0);
	glVertexAttribPointer(1, 3, GL_FLOAT, GL_FALSE, 6 * sizeof(float), (void*)(3 * sizeof(float))); // Set the interpretation of Vertex Buffer data
	glEnableVertexAttribArray(1);

	// Cube model
	glBindVertexArray(VAOs[1]);

	glBindBuffer(GL_ARRAY_BUFFER, VBOs[1]); // Set active
	glBufferData(GL_ARRAY_BUFFER, sizeof(vertices2), vertices2, GL_STATIC_DRAW); // Copy data

	glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 3 * sizeof(float), (void*)0);
	glEnableVertexAttribArray(0);



	// Complie and use shader
	//-------------------------------------------------
	GLuint programID = LoadShaders("vertex.shader", "fragment.shader");
	glUseProgram(programID);


	// Set background color
	//-------------------------------------------------
	glClearColor(0.2f, 0.2f, 0.4f, 0.0f);


	// Set wireframe mode
	//-------------------------------------------------
	glPolygonMode(GL_FRONT_AND_BACK, GL_LINE);


	// Enable depth testing
	//-------------------------------------------------
	glEnable(GL_DEPTH_TEST);


	// Calculate projection matrix and send it to shader
	//-------------------------------------------------
	glm::mat4 projection = glm::perspective(glm::radians(80.0f), (float)mode->width / (float)mode->height, 0.1f, 100.0f);
	unsigned int projLoc = glGetUniformLocation(programID, "projection");
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

		float radius = 15.0f;
		float orbitX = sin(currentTime) * radius;
		float orbitZ = cos(currentTime) * radius;
		float camY = (sin(currentTime / 4) + 2) * 3;


		// Get shader variable handles
		//-------------------------------------------------
		unsigned int viewLoc = glGetUniformLocation(programID, "view");
		unsigned int modelLoc = glGetUniformLocation(programID, "model");


		// Set camera
		//-------------------------------------------------
		glm::vec3 cameraPos = getCameraPosition(cameraId);
		glm::vec3 cameraTarget = glm::vec3(0.0f, 0.0f, 0.0f);
		glm::vec3 up = glm::vec3(0.0f, 1.0f, 0.0f);

		glm::vec3 cameraDirection = glm::normalize(cameraPos - cameraTarget); // camera's Z-axis
		glm::vec3 cameraRight = glm::normalize(glm::cross(up, cameraDirection)); // camera's X-axis 
		glm::vec3 cameraUp = glm::cross(cameraDirection, cameraRight); // camera's Y-axis


		glm::mat4 view = glm::lookAt(cameraPos, cameraTarget, cameraUp);
		glUniformMatrix4fv(viewLoc, 1, GL_FALSE, &view[0][0]);

		//fprintf(stderr, "(%f, %f, %f)\n", cameraUp.x, cameraUp.y, cameraUp.z);

		// Draw plane
		//-------------------------------------------------
		glm::mat4 modelPlane = glm::translate(glm::mat4()/*glm::lookAt(glm::vec3(yawAngle, -3.0f, orbitZ), glm::vec3(0.0f, 0.0f, 0.0f), up)*/, glm::vec3(orbitX, 3.0f, orbitZ));
		glUniformMatrix4fv(modelLoc, 1, GL_FALSE, &modelPlane[0][0]); // Send model matrix to shader
		glBindVertexArray(VAOs[0]);
		glDrawElements(GL_TRIANGLES, 12, GL_UNSIGNED_INT, 0);


		// Draw cubes
		//-------------------------------------------------
		glBindVertexArray(VAOs[1]);
		int amount = 50;
		for (int x = -amount; x <= amount; x++)
			for (int z = -amount; z <= amount; z++)
			{
				glm::mat4 modelCube = glm::translate(glm::mat4(), glm::vec3((float)x, 0.5f, (float)z));
				glUniformMatrix4fv(modelLoc, 1, GL_FALSE, &modelCube[0][0]); // Send model matrix to shader
				glDrawArrays(GL_TRIANGLES, 0, 108);
			}


		glfwSwapBuffers(window);
		glfwPollEvents();
	}


	// Destructors
	//--------------------------------------------------
	glDeleteVertexArrays(2, VAOs);
	glDeleteBuffers(2, VBOs);
	glDeleteBuffers(1, &EBO);
	glfwDestroyWindow(window);
	glfwTerminate();
	return 0;
}

void keyPressCallback(GLFWwindow* window, int key, int scancode, int action, int mods)
{
	switch (key)
	{
	case GLFW_KEY_1:
		cameraId = 1;
		break;
	case GLFW_KEY_2:
		cameraId = 2;
		break;
	case GLFW_KEY_3:
		cameraId = 3;
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
		return glm::vec3(20.0f, 10.0f, 20.0f);
	case 2:
		return glm::vec3(0.0f, 10.0f, 20.0f);
	case 3:
		return glm::vec3(-3.0f, 10.0f, -20.0f);
	}
}