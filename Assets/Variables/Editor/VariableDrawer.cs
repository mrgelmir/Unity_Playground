using UnityEditor;
using UnityEngine;

namespace SD.Variables.Editor
{

	// This is not an optimal nor elegant solution, for each variable type a new line needs to be added 
	[CustomPropertyDrawer(typeof(Reference<float>))]
	[CustomPropertyDrawer(typeof(Reference<bool>))]
	public class VariableDrawer : PropertyDrawer
	{

		private readonly string[] popupOptions =
			{ "Use constant", "Use Variable" };

		private GUIStyle popupStyle;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			if (popupStyle == null)
			{
				popupStyle = new GUIStyle(GUI.skin.GetStyle("PaneOptions"));
				popupStyle.imagePosition = ImagePosition.ImageOnly;
			}

			label = EditorGUI.BeginProperty(position, label, property);
			position = EditorGUI.PrefixLabel(position, label);

			EditorGUI.BeginChangeCheck();
			


			if (EditorGUI.EndChangeCheck())
				property.serializedObject.ApplyModifiedProperties();

			EditorGUI.EndProperty();
		}
	}
}