using System;
using UnityEngine;

namespace Main
{
    public interface IPlanetData
    {
        PlanetNames Name { get; set; }
        float OrbitRadius { get; set; }
        float FullCircleTime { get; set; }
    }

    [Serializable]
    public sealed class PlanetData
    {
        public PlanetNames Name;
        [Range(1.0f, 50.0f)] public float OrbitRadius = 1.0f;
        [Range(0.001f, 1.0f)] public float FullCircleTime = 0.005f;
    }
}