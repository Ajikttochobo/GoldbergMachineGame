using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Ability))]
public class AbilityDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // testEnum 프로퍼티
        SerializedProperty testEnumProp = property.FindPropertyRelative("test");
        SerializedProperty abilityNameProp = property.FindPropertyRelative("abilityName");

        // 그리기 위치 분할
        Rect enumRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        Rect nameRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + 2, position.width, EditorGUIUtility.singleLineHeight);

        EditorGUI.PropertyField(enumRect, testEnumProp);
        EditorGUI.PropertyField(nameRect, abilityNameProp);

        // enum 값에 따른 다른 필드 표시
        Ability.testEnum testValue = (Ability.testEnum)testEnumProp.enumValueIndex;

        float lineHeight = EditorGUIUtility.singleLineHeight + 2;
        Rect dataRect = new Rect(position.x, position.y + lineHeight * 2, position.width, lineHeight);

        switch (testValue)
        {
            case Ability.testEnum.Fire:
                EditorGUI.PropertyField(dataRect, property.FindPropertyRelative("fireData"), true);
                break;
            case Ability.testEnum.Water:
                EditorGUI.PropertyField(dataRect, property.FindPropertyRelative("waterData"), true);
                break;
            case Ability.testEnum.Wood:
                EditorGUI.PropertyField(dataRect, property.FindPropertyRelative("woodData"), true);
                break;
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        Ability.testEnum testValue = (Ability.testEnum)property.FindPropertyRelative("test").enumValueIndex;
        float lineHeight = EditorGUIUtility.singleLineHeight + 2;

        switch (testValue)
        {
            case Ability.testEnum.Fire:
                return lineHeight * 4; // enum + name + fireData(2 lines 예상) 필요한 줄 수 만큼 꼭 넣어줘야함!
            case Ability.testEnum.Water:
                return lineHeight * 4;
            case Ability.testEnum.Wood:
                return lineHeight * 4;
            default:
                return lineHeight * 2; // enum + name
        }
    }
}