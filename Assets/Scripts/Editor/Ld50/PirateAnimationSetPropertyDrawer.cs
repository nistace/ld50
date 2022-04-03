using Ld50;
using UnityEditor;
using UnityEngine;

namespace Ld50Editor {
	[CustomPropertyDrawer(typeof(PirateAnimationSet))]
	public class PirateAnimationSetPropertyDrawer : PropertyDrawer {
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) =>
			(Mathf.Max(1, property.FindPropertyRelative("_animationFrames").arraySize) + 1) * EditorGUIUtility.singleLineHeight;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			EditorGUI.BeginProperty(position, label, property);

			var indent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;

			while (property.FindPropertyRelative("_animationFrames").arraySize < EnumUtils.SizeOf<PirateAnimationSet.Animation>()) {
				property.FindPropertyRelative("_animationFrames").InsertArrayElementAtIndex(property.FindPropertyRelative("_animationFrames").arraySize);
				property.FindPropertyRelative("_animationFrames").GetArrayElementAtIndex(property.FindPropertyRelative("_animationFrames").arraySize - 1).vector2IntValue = new Vector2Int(0, 0);
			}

			while (property.FindPropertyRelative("_animationSpeeds").arraySize < EnumUtils.SizeOf<PirateAnimationSet.Animation>()) {
				property.FindPropertyRelative("_animationSpeeds").InsertArrayElementAtIndex(property.FindPropertyRelative("_animationSpeeds").arraySize);
				property.FindPropertyRelative("_animationSpeeds").GetArrayElementAtIndex(property.FindPropertyRelative("_animationSpeeds").arraySize - 1).floatValue = .1f;
			}

			GUI.Label(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), "Animation Frames");
			for (var i = 0; i < property.FindPropertyRelative("_animationFrames").arraySize; ++i) {
				EditorGUI.PropertyField(new Rect(position.x + 20, position.y + (i + 1) * EditorGUIUtility.singleLineHeight, position.width - 80, EditorGUIUtility.singleLineHeight),
					property.FindPropertyRelative("_animationFrames").GetArrayElementAtIndex(i), new GUIContent($"{(PirateAnimationSet.Animation)i}"));
				EditorGUI.PropertyField(new Rect(position.x + position.width - 60, position.y + (i + 1) * EditorGUIUtility.singleLineHeight, 60, EditorGUIUtility.singleLineHeight),
					property.FindPropertyRelative("_animationSpeeds").GetArrayElementAtIndex(i), GUIContent.none);
			}

			EditorGUI.indentLevel = indent;
			EditorGUI.EndProperty();
		}
	}
}