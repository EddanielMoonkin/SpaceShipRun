using UnityEngine;

public class RangeAttribute : PropertyAttribute
{
    public readonly float Min;
    public readonly float Max;
    public RangeAttribute(float min, float max)
    {
        Min = min;
        Max = max;
    }
}

