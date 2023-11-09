using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


public class CustomRenderPipeline : RenderPipeline 
{
	CameraRenderer renderer = new CameraRenderer();

	bool useDynamicBatching, useGPUInstancing;

	ShadowSettings shadowSettings;

	public CustomRenderPipeline(bool useDynamicBatching, bool useGPUInstancing, bool useSRPBatcher, ShadowSettings shadowSettings)
    {
		this.shadowSettings = shadowSettings;	
		this.useDynamicBatching = useDynamicBatching;
        this.useGPUInstancing = useGPUInstancing;
        GraphicsSettings.useScriptableRenderPipelineBatching = useSRPBatcher;
        GraphicsSettings.lightsUseLinearIntensity = true;
	}

	protected override void Render(ScriptableRenderContext context, Camera[] cameras)
	{
		foreach (var camera in cameras)
        {
			renderer.Render(context, camera, useDynamicBatching, useGPUInstancing, shadowSettings);
		}			
	}

	/*protected override void Render (ScriptableRenderContext context, List<Camera> cameras) 
	{
		for (int i = 0; i < cameras.Count; i++)
		{
			renderer.Render(context, cameras[i], useDynamicBatching, useGPUInstancing, shadowSettings);
		}
	}*/
}