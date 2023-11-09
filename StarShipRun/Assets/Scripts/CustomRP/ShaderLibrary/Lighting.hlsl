#ifndef CUSTOM_LIGHTING_INCLUDED
#define CUSTOM_LIGHTING_INCLUDED

float3 IncomingLight (Surface surface, Light light) {
	return saturate(dot(surface.normal, light.direction)* light.attenuation) * light.color;
}

float3 GetLighting (Surface surface, BRDF brdf, Light light) {
	//return IncomingLight(surface, light) * surface.color;
	 return IncomingLight(surface, light) * DirectBRDF(surface, brdf, light);
}

float3 GetLighting (Surface surfaceWS, BRDF brdf) {
	//return GetLighting(surface, GetDirectionalLight()); //for a single directionLight
	ShadowData shadowData = GetShadowData(surfaceWS);
	float3 color = 0.0;	
	for (int i = 0; i < GetDirectionalLightCount(); i++) // for multiple
	{
		Light light = GetDirectionalLight(i, surfaceWS, shadowData);
		color += GetLighting(surfaceWS, brdf, light);
	}
	return color;
}

#endif