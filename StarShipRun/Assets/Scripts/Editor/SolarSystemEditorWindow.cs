using System;
using UnityEditor;
using UnityEngine;
using Mechanics;

namespace Main
{    
    public sealed class SolarSystemEditorWindow : EditorWindow
    {
        [SerializeField] private PlanetOrbit[] _planets;        
        private Editor MainWindow;

        [MenuItem("Window/Solar System Editor")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(SolarSystemEditorWindow));
        }

        private void OnEnable()
        {
            _planets = GameObject.FindObjectsOfType<PlanetOrbit>(true);
        }

        [Obsolete]
        private void OnGUI()
        {         
            for (int i = 0; i < _planets.Length; i++)
            {
                EditorGUILayout.BeginVertical("Box");
                GUILayout.Label($"{_planets[i].name}", EditorStyles.boldLabel);
                _planets[i].gameObject.active = EditorGUILayout.Toggle("isPlanetActive", _planets[i].gameObject.active);
                _planets[i].FullCircleTime = EditorGUILayout.FloatField("Full Time Circle: ", _planets[i].FullCircleTime);
                GUILayout.Label("Orbit Radius: ");
                _planets[i].OrbitRadius = EditorGUILayout.Slider(_planets[i].OrbitRadius, 0f, 1200f);                
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space();

            }
            
        }
    }
}