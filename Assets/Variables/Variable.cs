using UnityEngine;

namespace SD.Variables
{
	public class Variable<T>:ScriptableObject
	{
		// The value that will be serialized and persist in editor
		[SerializeField]
		private T value;

		// The variable used in a session
		private T currentValue;

		/// <summary>
		/// The value this instance represents
		/// </summary>
		public T Value
		{
			get { return currentValue; }
			set { currentValue = value; }
		}

		protected void OnEnable()
		{
			// Make sure this object does not get unloaded when not in use
			// Otherwise, it might lose state during play 
			hideFlags = HideFlags.DontUnloadUnusedAsset;

			// Set the value we will use througout the session
			currentValue = value;
		}

		protected void OnDisable()
		{

		}
	}
}
