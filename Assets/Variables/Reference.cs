using UnityEngine;

namespace SD.Variables
{
	[System.Serializable]
	public class Reference<T>
	{
		[SerializeField]
		private bool useConstant = true;
		[SerializeField]
		private T constantValue;
		[SerializeField]
		private Variable<T> variableValue;

		public T Value
		{
			get { return useConstant ? constantValue : variableValue.Value; }
		}
	}
}
