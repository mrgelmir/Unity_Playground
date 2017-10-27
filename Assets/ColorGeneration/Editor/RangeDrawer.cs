
using UnityEditor;
using UnityEngine;
using ColorGeneration;

[CustomPropertyDrawer(typeof(ColorGenerator.Range))]
public class RangeDrawer : PropertyDrawer
{

	private SerializedProperty Min, Max;
	private string name;
	private bool cache = false;

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		if (!cache)
		{
			//get the name before it's gone
			name = property.displayName;

			//get the X and Y values
			property.Next(true);
			Min = property.Copy();
			property.Next(true);
			Max = property.Copy();

			cache = true;
		}

		Rect contentPosition = EditorGUI.PrefixLabel(position, new GUIContent(name));

		//Check if there is enough space to put the name on the same line (to save space)
		if (position.height > EditorGUIUtility.singleLineHeight * 2f)
		{
			position.height = EditorGUIUtility.singleLineHeight * 2f;
			EditorGUI.indentLevel += 1;
			contentPosition = EditorGUI.IndentedRect(position);
			contentPosition.y += 18f;
		}

		float half = contentPosition.width / 2f;
		GUI.skin.label.padding = new RectOffset(3, 3, 6, 6);

		// TODO fix positioning
		Rect sliderPos = contentPosition;
		sliderPos.height *= .5f;
		sliderPos.y += sliderPos.height;

		//show the Min and Max
		EditorGUIUtility.labelWidth = 30f;
		contentPosition.width *= .5f;
		contentPosition.height *= .5f;
		EditorGUI.indentLevel = 0;

		// Begin/end property & change check make each field
		// behave correctly when multi-object editing.
		EditorGUI.BeginProperty(contentPosition, label, Min);
		{
			EditorGUI.BeginChangeCheck();
			float newVal = EditorGUI.FloatField(contentPosition, new GUIContent("Min"), Min.floatValue);
			if (EditorGUI.EndChangeCheck())
				Min.floatValue = newVal;
		}
		EditorGUI.EndProperty();

		contentPosition.x += half;

		EditorGUI.BeginProperty(contentPosition, label, Max);
		{
			EditorGUI.BeginChangeCheck();
			float newVal = EditorGUI.FloatField(contentPosition, new GUIContent("Max"), Max.floatValue);
			if (EditorGUI.EndChangeCheck())
				Max.floatValue = newVal;
		}
		EditorGUI.EndProperty();

		// Fancy slider
		float min = Min.floatValue;
		float max = Max.floatValue;
		EditorGUI.BeginChangeCheck();
		EditorGUI.MinMaxSlider(sliderPos, GUIContent.none, ref min, ref max, 0f, 1f);
		if (EditorGUI.EndChangeCheck())
		{
			Min.floatValue = min;
			Max.floatValue = max;
		}


	}

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		// Create extra height for title on narrow inspectors
		return Screen.width < 333 ? (EditorGUIUtility.singleLineHeight * 2f + 18f) : EditorGUIUtility.singleLineHeight * 2f;
	}
}
