#version 330 core

layout(location = 0) in vec3 aPos;
layout(location = 1) in vec3 aNormal;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;
uniform mat4 normalMatrix;

uniform vec3 viewPos;

uniform vec3 materialAmbient;
uniform vec3 materialDiffuse;
uniform vec3 materialSpecular;
uniform float shininess;

uniform vec3 pointLightPos;
uniform vec3 pointLightColor;

uniform vec3 spotLightPos;
uniform vec3 spotLightColor;
uniform vec3 spotLightAim;
uniform float spotLightCosCutoff;
uniform float spotLightExp;

out vec4 FragColor;

void main()
{
	vec3 FragPos = vec3(model * vec4(aPos, 1.0));

	gl_Position = projection * view * model * vec4(aPos, 1.0);

	vec3 norm = normalize(vec3(normalMatrix * vec4(aNormal, 1.0f)));
	vec3 viewDir = normalize(viewPos - FragPos);
	vec3 specularColor = vec3(1.0);


	/// ---------------- Ambient lighting (WHITE)
	vec3 ambientLightColor = vec3(1.0f);
	vec3 ambient = materialAmbient * ambientLightColor;
	/// ----------------------------------------



	/// ---------------- Point light
	// -------- Diffuse
	vec3 pointLightDir = normalize(pointLightPos - FragPos);
	float pointAngleFactor = max(dot(norm, pointLightDir), 0.0);
	vec3 pointDiff = pointAngleFactor * pointLightColor;

	// -------- Specular
	vec3 pointSpecDir = reflect(-pointLightDir, norm);
	pointAngleFactor = pow(max(dot(viewDir, pointSpecDir), 0.0), shininess);
	vec3 pointSpec = pointAngleFactor * specularColor;

	vec3 pointResult = materialDiffuse * pointDiff + materialSpecular * pointSpec;
	/// ----------------------------------------



	/// ---------------- Spotlight
	// -------- Diffuse
	vec3 spotLightDir = normalize(spotLightPos - FragPos);
	float spotAngleFactor = max(dot(norm, spotLightDir), 0.0);
	vec3 spotDiff = spotAngleFactor * spotLightColor;

	float cutoff = dot(normalize(spotLightAim), -spotLightDir);
	float effectFactor = pow(cutoff, spotLightExp);
	spotDiff *= effectFactor;

	// -------- Specular
	vec3 spotSpecDir = reflect(-spotLightDir, norm);
	spotAngleFactor = pow(max(dot(viewDir, spotSpecDir), 0.0), shininess);
	vec3 spotSpec = spotAngleFactor * specularColor;

	if (cutoff < spotLightCosCutoff) {
	spotDiff = vec3(0.0f);
	spotSpec = vec3(0.0f);
	}

	vec3 spotResult = materialDiffuse * spotDiff + materialSpecular * spotSpec;
	/// ----------------------------------------


	vec3 result = (ambient + pointResult + spotResult);
	FragColor = vec4(result, 1.0);
}