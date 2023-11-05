using UnityEditor;
using UnityEngine;


[CustomPropertyDrawer(typeof(Ingredient))]
public class IngredientDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property,
    GUIContent label)
    {
        // Использование BeginProperty / EndProperty определяет блок кода,который будет применим только для текущего SerializedProperty
        EditorGUI.BeginProperty(position, label, property);
        
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label); // Название переменной
        
        var indent = EditorGUI.indentLevel; // Настройка отступа для дочерних объектов
        EditorGUI.indentLevel = 0;
       
        var amountRect = new Rect(position.x, position.y, 30, position.height);  // Расчет поля размеров границ поля визуализации
        var unitRect = new Rect(position.x + 35, position.y, 50, position.height);
        var nameRect = new Rect(position.x + 90, position.y, position.width - 90, position.height);
        
        EditorGUI.PropertyField(amountRect, property.FindPropertyRelative("Amount"), GUIContent.none); // Визуализация полей исходного класса
        EditorGUI.PropertyField(unitRect, property.FindPropertyRelative("Unit"), GUIContent.none);
        EditorGUI.PropertyField(nameRect, property.FindPropertyRelative("Name"), GUIContent.none);
       
        EditorGUI.indentLevel = indent;  // Возвращение отступа к исходному значению
        EditorGUI.EndProperty();
    }
}
