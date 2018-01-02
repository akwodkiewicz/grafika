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


int cameraId = 3;
int aspectRatioChanged = 1;
int currentWidth;
int currentHeight;

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


	// Get shader variable handles
	//-------------------------------------------------
	unsigned int modelLoc = glGetUniformLocation(programID, "model");
	unsigned int viewLoc = glGetUniformLocation(programID, "view");
	unsigned int projLoc = glGetUniformLocation(programID, "projection");


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

		float radius = 15.0f;
		float orbitX = sin(currentTime) * radius;
		float orbitZ = cos(currentTime) * radius;
		//float camY = (sin(currentTime / 4) + 2) * 3;


		// Calculate new projection matrix has the aspect ratio changed
		//-------------------------------------------------
		if (aspectRatioChanged)
		{
			projection = createProjectionMatrix(1.0f, 100.0f, glm::radians(45.0f), (float)currentHeight / (float)currentWidth);
			glUniformMatrix4fv(projLoc, 1, GL_FALSE, &projection[0][0]);
			aspectRatioChanged = 0;
		}


		// Set camera (calculate `view` matrix)
		//-------------------------------------------------
		glm::vec3 cameraPos = getCameraPosition(cameraId);
		glm::vec3 cameraTarget = glm::vec3(0.0f, 0.5f, 0.5f);
		glm::vec3 up = glm::vec3(0.0f, 1.0f, 0.0f);
		glm::mat4 view = createViewMatrix(cameraPos, cameraTarget, up);
		glUniformMatrix4fv(viewLoc, 1, GL_FALSE, &view[0][0]);


		// Draw plane
		//-------------------------------------------------
		glm::mat4 modelPlane = glm::translate(glm::mat4()/*glm::lookAt(glm::vec3(yawAngle, -3.0f, orbitZ), glm::vec3(0.0f, 0.0f, 0.0f), up)*/, glm::vec3(orbitX, 3.0f, orbitZ));
		glUniformMatrix4fv(modelLoc, 1, GL_FALSE, &modelPlane[0][0]); // Send model matrix to shader
		glBindVertexArray(VAOs[0]);
		glDrawElements(GL_TRIANGLES, 12, GL_UNSIGNED_INT, 0);


		// Draw cubes
		//-------------------------------------------------
		glBindVertexArray(VAOs[1]);
		//int amount = 50;
		//for (int x = -amount; x <= amount; x++)
		//	for (int z = -amount; z <= amount; z++)
		//	{
		//		glm::mat4 modelCube = glm::translate(glm::mat4(), glm::vec3((float)x, 0.5f, (float)z));
		//		glUniformMatrix4fv(modelLoc, 1, GL_FALSE, &modelCube[0][0]); // Send model matrix to shader
		//		glDrawArrays(GL_TRIANGLES, 0, 108);
		//	}

		glm::mat4 modelCube = glm::translate(glm::mat4(), glm::vec3((float)0, 0.5f, (float)0));
		glUniformMatrix4fv(modelLoc, 1, GL_FALSE, &modelCube[0][0]); // Send model matrix to shader
		glDrawArrays(GL_TRIANGLES, 0, 108);
		modelCube = glm::translate(glm::mat4(), glm::vec3((float)0, -0.5f, (float)0));
		glUniformMatrix4fv(modelLoc, 1, GL_FALSE, &modelCube[0][0]); // Send model matrix to shader
		glDrawArrays(GL_TRIANGLES, 0, 108);
		modelCube = glm::translate(glm::mat4(), glm::vec3((float)0, 1.5f, (float)0));
		glUniformMatrix4fv(modelLoc, 1, GL_FALSE, &modelCube[0][0]); // Send model matrix to shader
		glDrawArrays(GL_TRIANGLES, 0, 108);
		modelCube = glm::translate(glm::mat4(), glm::vec3((float)1, 0.5f, (float)0));
		glUniformMatrix4fv(modelLoc, 1, GL_FALSE, &modelCube[0][0]); // Send model matrix to shader
		glDrawArrays(GL_TRIANGLES, 0, 108);
		 modelCube = glm::translate(glm::mat4(), glm::vec3((float)0, 0.5f, (float)1));
		glUniformMatrix4fv(modelLoc, 1, GL_FALSE, &modelCube[0][0]); // Send model matrix to shader
		glDrawArrays(GL_TRIANGLES, 0, 108);


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
		return glm::vec3(15.0f, 10.0f, 20.0f);
	case 2:
		return glm::vec3(0.0f, 10.0f, 20.0f);
	case 3:
		return glm::vec3(3.0f, 0.5f, -0.5f);
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
