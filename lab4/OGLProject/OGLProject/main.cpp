#include <stdio.h>
#include <stdlib.h>

#include <GL\glew.h>
#include <GLFW\glfw3.h>

#include <glm\glm.hpp>
#include <glm\gtc\matrix_transform.hpp>

#include "shdloader.hpp"


// MODEL
//---------------------------------
static const GLfloat vertices[] = {
	-1.0f, -1.0f,  0.0f,
	1.0f, -1.0f,  0.0f,
	0.0f,  1.0f,  0.0f,

	//0.0f,  1.0f,  0.0f,
	0.0f, -1.0f, -0.33f,
	-0.33f, -1.0f,  0.0f,

	//0.0f,  1.0f,  0.0f,
	0.0f, -1.0f, -0.33f,
	0.33f, -1.0f,  0.0f,

	0.0f, -1.0f, -0.33f,
	//-0.33f, -1.0f,  0.0f,
	//0.33f, -1.0f,  0.0f
};
static GLuint indices[] = {
	0, 1, 2,
	2, 3, 4,
	2, 5, 6,
	7, 4, 6
};



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
	GLFWwindow* window = glfwCreateWindow(mode->width/1.2, mode->height/1.2, "Project 4", NULL, NULL);
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

	// Complie shader
	//-------------------------------------------------
	GLuint programID = LoadShaders("vertex.shader", "fragment.shader");


	// Prepare all buffers
	//-------------------------------------------------
	GLuint VBO, VAO, EBO;

	glGenVertexArrays(1, &VAO); // Buffer array object
	glBindVertexArray(VAO); // Set active
	
	glGenBuffers(1, &VBO);	// Vertex buffer object
	glBindBuffer(GL_ARRAY_BUFFER, VBO); // Set active
	glBufferData(GL_ARRAY_BUFFER, sizeof(vertices), vertices, GL_STATIC_DRAW); // Copy data
	
	glGenBuffers(1, &EBO); // Element buffer object - keeps data about indices
	glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, EBO); // Set active
	glBufferData(GL_ELEMENT_ARRAY_BUFFER, sizeof(indices), indices, GL_STATIC_DRAW); // Copy data


	// Set the interpretation of Vertex Buffer data
	//-------------------------------------------------
	glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 3 * sizeof(float), (void*)0);
	glEnableVertexAttribArray(0);


	// Set background color
	//-------------------------------------------------
	glClearColor(0.2f, 0.2f, 0.2f, 0.0f);


	// Set wireframe mode
	//-------------------------------------------------
	glPolygonMode(GL_FRONT_AND_BACK, GL_LINE);
	
	
	// Event loop
	//-------------------------------------------------
	while (!glfwWindowShouldClose(window))
	{
		glClear(GL_COLOR_BUFFER_BIT);
		glUseProgram(programID);

		glm::mat4 model;
		glm::mat4 view;
		model = glm::rotate(model, (float)glfwGetTime() * glm::radians(50.0f), glm::vec3(0.5f, 1.0f, 0.0f));
		view = glm::translate(view, glm::vec3(0.0f, 0.0f, -5.0f));
		glm::mat4 projection = glm::perspective(glm::radians(45.0f), (float)mode->width / (float)mode->height, 0.1f, 100.0f);

		// Get shader variable handles
		//-------------------------------------------------
		unsigned int modelLoc = glGetUniformLocation(programID, "model");
		unsigned int viewLoc = glGetUniformLocation(programID, "view");
		unsigned int projLoc = glGetUniformLocation(programID, "projection");
		
		// Send data to shader
		//-------------------------------------------------
		glUniformMatrix4fv(modelLoc, 1, GL_FALSE, &model[0][0]);
		glUniformMatrix4fv(viewLoc, 1, GL_FALSE, &view[0][0]);
		glUniformMatrix4fv(projLoc, 1, GL_FALSE, &projection[0][0]);

		glBindVertexArray(VAO);
		glDrawElements(GL_TRIANGLES, 12, GL_UNSIGNED_INT, 0);

		glfwSwapBuffers(window);
		glfwPollEvents();
	}
	

	// Destructors
	//--------------------------------------------------
	glDeleteVertexArrays(1, &VAO);
	glDeleteBuffers(1, &VBO);
	glfwDestroyWindow(window);
	glfwTerminate();
	return 0;
}
