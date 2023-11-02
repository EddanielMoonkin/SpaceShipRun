using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor;

public partial class CameraRenderer 
{
    void DrawGizmos() 
    {
        if (!Handles.ShouldRenderGizmos())
        {
            return;
        }
        _context.DrawGizmos(_camera, GizmoSubset.PreImageEffects);
        _context.DrawGizmos(_camera, GizmoSubset.PostImageEffects);
    }

    partial void ShowUI();

    partial void DrawUnsupportedShaders();
#if UNITY_EDITOR

    partial void ShowUI() //lssn8 task1
    {
        if (_camera.cameraType == CameraType.SceneView)
            ScriptableRenderContext.EmitWorldGeometryForSceneView(_camera);
    }

    private static readonly ShaderTagId[] _legacyShaderTagIds =
    {
        new ShaderTagId("Always"),
        new ShaderTagId("ForwardBase"),
        new ShaderTagId("PrepassBase"),
        new ShaderTagId("Vertex"),
        new ShaderTagId("VertexLMRGBM"),
        new ShaderTagId("VertexLM")
    };

    private static Material _errorMaterial = new Material(Shader.Find("Hidden/InternalErrorShader"));

    partial void DrawUnsupportedShaders()
    {
        var drawingSettings = new DrawingSettings(_legacyShaderTagIds[0], new SortingSettings(_camera))
        {
            overrideMaterial = _errorMaterial,
        };
        for (var i = 1; i < _legacyShaderTagIds.Length; i++)
        {
            drawingSettings.SetShaderPassName(i, _legacyShaderTagIds[i]);
        }
        var filteringSettings = FilteringSettings.defaultValue;

        _context.DrawRenderers(_cullingResult, ref drawingSettings, ref filteringSettings);
    }
#endif
}
