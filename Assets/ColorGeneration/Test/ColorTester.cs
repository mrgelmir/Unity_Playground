using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ColorGeneration.Test
{
	public class ColorTester : MonoBehaviour
	{
		[SerializeField]
		private RectTransform imageParent;

		[SerializeField]
		private int colorCount = 3;

		[SerializeField]
		private ColorGenerator.GeneratorSettings colorSettings;

		private ColorGenerator cg;


		public void GenerateColors()
		{
			// Clear images
			for (int i = 0; i < imageParent.childCount; i++)
			{
				Destroy(imageParent.GetChild(i).gameObject);
			}

			// Generate new images
			ICollection<Color> colors = cg.GetColorSet(3);
			foreach (Color color in colors)
			{
				AddImage(color);
			}
		}

		private void AddImage(Color color)
		{
			Image img = new GameObject().AddComponent<Image>();
			img.color = color;
			img.transform.SetParent(imageParent, false);
		}

	}
}