using System.Collections.Generic;
using UnityEngine;

namespace ColorGeneration
{
	public class ColorGenerator
	{
		// Static part

		/// <summary>
		/// Get some random colors
		/// </summary>
		/// <param name="amount">The desired amount of colors</param>
		/// <returns>A list of ugly colors</returns>
		public static ICollection<Color> GetRandomColors(int amount)
		{
			ICollection<Color> col = new List<Color>();

			for (int i = 0; i < amount; i++)
			{
				col.Add(GetRandomColor());
			}

			return col;
		}

		/// <summary>
		/// Get a fully random color
		/// </summary>
		/// <returns>Probably an ugly color</returns>
		public static Color GetRandomColor()
		{
			return Random.ColorHSV(0f, 1f, 0f, 1f, 0f, 1f);
		}

		/// <summary>
		/// Get a Random color within specified ranges
		/// </summary>
		/// <param name="hue">The range of the output hue [0,1]</param>
		/// <param name="saturation">The range of the output saturation [0,1]</param>
		/// <param name="value">The range of the output value [0,1]</param>
		/// <returns>A random color within provided ranges</returns>
		public static Color GetColor(Range hue, Range saturation, Range value)
		{
			return Color.HSVToRGB(hue.GetValue(), saturation.GetValue(), value.GetValue());
		}

		/// <summary>
		/// Get a random color with a fixed hue
		/// </summary>
		/// <param name="hue">The hue of the output color</param>
		/// <param name="saturation">The range of the output saturation</param>
		/// <param name="value">The range of the output value</param>
		/// <returns>A random color with provided hue, and saturation and value between provided ranges</returns>
		public static Color GetColor(float hue, Range saturation, Range value)
		{
			return Color.HSVToRGB(hue, saturation.GetValue(), value.GetValue());
		}

		// Instance part

		private GeneratorSettings settings;

		public GeneratorSettings Settings
		{
			get { return settings; }
			set { settings = value; }
		}

		public ColorGenerator(GeneratorSettings settings)
		{
			this.Settings = settings;
		}

		public Color NextColor()
		{
			return GetColor(Range.Full, Settings.Saturation, Settings.Value);
		}


		public ICollection<Color> GetColorSet(int amount)
		{
			return GetColorSet(amount, GetColor(Random.value, settings.Saturation, settings.Value));
		}

		public ICollection<Color> GetColorSet(int amount, Color startColor)
		{
			// Sanity checking
			if (amount <= 0)
			{
				throw new System.ArgumentOutOfRangeException("amount", "Amount should be above zero");
			}
			if ((settings.Angle.Min * amount) > 1f)
			{
				throw new System.ArgumentException(string.Format("A minimum angle of {0} cannot be obtained with {1} elements", settings.Angle.Min, amount));
			}

			List<Color> colors = new List<Color>(amount);

			// Create first color
			colors.Add(startColor);

			if (amount > 1)
			{
				// Get hue
				float h, s, v;
				Color.RGBToHSV(startColor, out h, out s, out v);

				// Calculate hue variations using minimum angle, rotating right as a default
				float maxAngle = Mathf.Min(settings.Angle.Max, 1f / amount);
				float currentAngle = 0f;
				for (int i = 0; i < amount - 1; i++)
				{
					currentAngle += Random.Range(settings.Angle.Min, maxAngle);

					if (h + currentAngle >= 1f)
					{
						currentAngle -= 1f;
					}

					colors.Add(GetColor(h + currentAngle, settings.Saturation, settings.Value));
				}
			}

			//// Debugging
			//System.Text.StringBuilder sb = new System.Text.StringBuilder();
			//foreach (Color col in colors)
			//{
			//	sb.AppendLine(col.ToString());
			//}

			//Debug.Log(sb.ToString());

			return colors;
		}

		/// <summary>
		/// Get a color set in which the <b>first</b> color will be separated from the <paramref name="exclusiveColor"/> by the angle values in the settings
		/// </summary>
		/// <param name="amount">Desired amount of colors</param>
		/// <param name="exclusiveColor">The color that will not overlap with the first in the returned collection</param>
		/// <returns>A collection of colors, in which the first differs from <paramref name="exclusiveColor"/></returns>
		public ICollection<Color> GetColorSetExclusive(int amount, Color exclusiveColor)
		{
			// Generate a color seperate from the exclusiveColor
			// Get hue
			float h, s, v;
			Color.RGBToHSV(exclusiveColor, out h, out s, out v);

			// Rotate the color
			float newHue = h + settings.Angle.GetValue();
			if (newHue >= 1f)
			{
				newHue -= 1f;
			}
			
			Color startColor = GetColor(newHue, settings.Saturation, settings.Value);

			return GetColorSet(amount, startColor);
		}

		[System.Serializable]
		public class GeneratorSettings
		{
			public Range Angle = Range.Full;
			public Range Saturation = Range.Full;
			public Range Value = Range.Full;
		}

		[System.Serializable]
		public struct Range
		{
			public static Range Full = new Range() { Min = 0, Max = 1 };
			public static Range Create(float min, float max)
			{
				return new Range() { Min = min, Max = max };
			}

			public float Min;
			public float Max;

			public float GetValue()
			{
				return Random.Range(Min, Max);
			}
		}
	}
}