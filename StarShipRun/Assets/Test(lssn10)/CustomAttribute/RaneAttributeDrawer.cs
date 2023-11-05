using UnityEditor;
using UnityEngine;
[CustomPropertyDrawer(typeof(RangeAttribute))]
public class RangeAttributeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property,
    GUIContent label)
    {
        var range = (RangeAttribute)attribute;
        if (property.propertyType == SerializedPropertyType.Float)
        {
            EditorGUI.Slider(position, property, range.Min, range.Max, label);
        }
        else if (property.propertyType == SerializedPropertyType.Integer)
        {
            EditorGUI.IntSlider(position, property, (int)range.Min, (int)range.Max, label);
        }
        else
        {
            EditorGUI.LabelField(position, label.text, "Use RangeAttribute with float or int.");
        } 
    } 
}