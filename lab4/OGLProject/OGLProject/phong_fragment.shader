#version 330 core

in vec3 Normal;
in vec3 FragPos;

uniform vec3 viewPos;
uniform vec3 objectColor;
uniform int specularPower;


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
	float ambientStrength = 0.32f;
	float diffuseStrength = 0.8f;
	float specularStrength = 0.4f;

	vec3 norm = normalize(Normal);
	vec3 viewDir = normalize(viewPos - FragPos);
	vec3 specularColor = vec3(1.0);


	/// ---------------- Ambient lighting (WHITE)
	vec3 ambientLightColor = vec3(1.0);
	vec3 ambient = ambientStrength * ambientLightColor;
	/// ----------------------------------------


	
	/// ---------------- Point light
	// -------- Diffuse
	vec3 pointLightDir = normalize(pointLightPos - FragPos);
	float pointAngleFactor = max(dot(norm, pointLightDir), 0.0);
	vec3 pointDiff = pointAngleFactor * pointLightColor;

	// -------- Specular
	vec3 pointSpotDir = reflect(-pointLightDir, norm);
	pointAngleFactor = pow(max(dot(viewDir, pointSpotDir), 0.0), specularPower);
	vec3 pointSpec = pointAngleFactor * specularColor;

	vec3 pointResult = pointDiff * diffuseStrength + pointSpec * specularStrength;
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
	vec3 spotSpec = vec3(0.0f);
	

	if (cutoff < spotLightCosCutoff) {
		spotDiff = vec3(0.0f);
		spotSpec = vec3(0.0f);
	}
	vec3 spotResult = diffuseStrength * spotDiff + specularStrength * spotSpec;
	/// ----------------------------------------


	vec3 result = (ambient + pointResult + spotResult) * objectColor;
	FragColor = vec4(result, 1.0);
}
