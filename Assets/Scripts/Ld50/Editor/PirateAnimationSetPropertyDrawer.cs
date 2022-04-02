using UnityEditor;
using UnityEngine;

namespace Ld50.Editor {
	[CustomPropertyDrawer(typeof(PirateAnimationSet))]
	public class PirateAnimationSetPropertyDrawer : PropertyDrawer {
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) =>
			(Mathf.Max(1, property.FindPropertyRelative("_animationFrames").arraySize) + 1) * EditorGUIUtility.singleLineHeight;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			EditorGUI.BeginProperty(position, label, property);

			var indent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;

			while (property.FindPropertyRelative("_animationFrames").arraySize < 3) {
				property.FindPropertyRelative("_animationFrames").InsertArrayElementAtIndex(property.FindPropertyRelative("_animationFrames").arraySize);
				property.FindPropertyRelative("_animationFrames").GetArrayElementAtIndex(property.FindPropertyRelative("_animationFrames").arraySize - 1).vector2IntValue = new Vector2Int(0, 0);
			}

			GUI.Label(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), "Animation Frames");
			for (var i = 0; i < property.FindPropertyRelative("_animationFrames").arraySize; ++i) {
				EditorGUI.PropertyField(new Rect(position.x + 40, position.y + (i + 1) * EditorGUIUtility.singleLineHeight, position.width - 40, EditorGUIUtility.singleLineHeight),
					property.FindPropertyRelative("_animationFrames").GetArrayElementAtIndex(i), new GUIContent($"{(PirateAnimationSet.Animation)i}"));
			}

			EditorGUI.indentLevel = indent;
			EditorGUI.EndProperty();
		}
	}
}