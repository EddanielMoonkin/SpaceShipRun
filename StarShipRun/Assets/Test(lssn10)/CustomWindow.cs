using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CustomWindow : EditorWindow
{
    private string _myString = "Hello World";
    private bool _groupEnabled;
    private bool _myBool = true;
    private float _myFloat = 1.23f;

    [MenuItem("Window/Custom Window")] //путь вызова
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(CustomWindow)); //вызов окна
    }
    private void OnGUI() // Код реализации UI интерфейса окна
    {
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        _myString = EditorGUILayout.TextField("Text Field", _myString);
        _groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", _groupEnabled);
        _myBool = EditorGUILayout.Toggle("Toggle", _myBool);
        _myFloat = EditorGUILayout.Slider("Slider", _myFloat, -3, 3);
        EditorGUILayout.EndToggleGroup();
    }

}
