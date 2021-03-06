﻿using UnityEngine;

namespace UnityEngine.Workshop.Events
{
	[System.Serializable]
	public abstract class PropertyChangedEventArgs<T> : System.EventArgs
	{
		#region Properties

		public T oldValue { get; protected set; }
		public T newValue { get; protected set; }

		#endregion

		#region Constructors

		public PropertyChangedEventArgs(T oldValue, T newValue)
		{
			this.oldValue = oldValue;
			this.newValue = newValue;
		}

		#endregion
	}

	public class FloatChangedEventArgs : PropertyChangedEventArgs<float>
	{
		public FloatChangedEventArgs(float oldValue, float newValue)
			: base(oldValue, newValue)
		{ }
	}

	public class StringChangedEventArgs : PropertyChangedEventArgs<string>
	{
		public StringChangedEventArgs(string oldValue, string newValue)
			: base(oldValue, newValue)
		{ }
	}
}
