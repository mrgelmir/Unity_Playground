using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorGeneration
{
	public class ColorGenerator
	{

		// Static part

		public static ICollection<Color> GetRandomColors(int amount)
		{
			ICollection<Color> col = new List<Color>();

			for (int i = 0; i < amount; i++)
			{
				col.Add(GetRandomColor());
			}

			return col;
		}

		public static Color GetRandomColor()
		{
			return Random.ColorHSV(0f, 1f, 0f, 1f, 0f, 1f);
		}

		public static Color GetColor(Range hue, Range saturation, Range value)
		{
			return Random.ColorHSV(hue.Min, hue.Max, saturation.Min, saturation.Max, value.Min, value.Max);
		}

		public static Color GetColor(float hue, Range saturation, Range value)
		{
			return Random.ColorHSV(hue, hue, saturation.Min, saturation.Max, value.Min, value.Max);
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
			// Sanity checking
			if(amount <= 0)
			{
				throw new System.ArgumentOutOfRangeException("amount", "Amount should be above zero");
			}
			if ((settings.MinAngle * (amount - 1)) > 1f)
			{
				throw new System.ArgumentException(string.Format("A minimum angle of {0} cannot be obtained with {1} elements", settings.MinAngle, amount));
			}

			List<Color> colors = new List<Color>(amount);

			// Create first color
			Color c = GetRandomColor();
			colors.Add(c);

			if (amount > 1)
			{
				// Get hue
				float h, s, v;
				Color.RGBToHSV(c, out h, out s, out v);

				// Calculate hue variations using minimum angle, rotating right as a default
				

			}

			return colors;
		}


		[System.Serializable]
		public class GeneratorSettings
		{
			[Range(0f, .5f)]
			public float MinAngle = 0f;
			//[Range(.5f, 1f)]
			//public float MaxAngle = 0f;
			public Range Saturation = Range.Full;
			public Range Value = Range.Full;
		}

		[System.Serializable]
		public class Range
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