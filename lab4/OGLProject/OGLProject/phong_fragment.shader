#version 330 core

in vec3 Normal;
in vec3 FragPos;

uniform vec3 viewPos;
uniform vec3 objectColor;
uniform int specularPower;


uniform vec3 pointLightPos;
uniform vec3 pointLightColor;

uniform vec3 reflectorLightPos;
uniform vec3 reflectorLightColor;

out vec4 FragColor;

void main()
{
	/// ---------------- Ambient lighting (WHITE)
	float ambientStrength = 0.2;
	vec3 ambientLightColor = vec3(1.0);
	vec3 ambient = ambientStrength * ambientLightColor;
	/// ----------------------------------------
	
	


	/// ---------------- Diffused ligthing
	float diffuseStrength = 0.6;
	vec3 norm = normalize(Normal);
	
	// -------- Point light
	vec3 pointLightDir = normalize(pointLightPos - FragPos);
	float pointAngleFactor = max(dot(norm, pointLightDir), 0.0);
	vec3 pointDiff = pointAngleFactor * pointLightColor;
	
	// -------- Reflector light
	//
	//
	vec3 reflectorDiff = vec3(0.0);

	vec3 diffuse = diffuseStrength * (pointDiff + reflectorDiff);
	/// ----------------------------------------




	/// ---------------- Specular lighting (WHITE)
	float specularStrength = 0.2;
	vec3 specularColor = vec3(1.0);
	vec3 viewDir = normalize(viewPos - FragPos);

	// -------- Point light
	vec3 pointReflectDir = reflect(-pointLightDir, norm);
	pointAngleFactor = pow(max(dot(viewDir, pointReflectDir), 0.0), specularPower);
	vec3 pointSpec = pointAngleFactor * specularColor;

	// -------- Reflector light
	//
	//
	vec3 reflectorSpec = vec3(0.0);
	
	vec3 specular = specularStrength * (pointSpec + reflectorSpec);
	/// ----------------------------------------





	vec3 result = (ambient + diffuse + specular) * objectColor;
	FragColor = vec4(result, 1.0);
}
