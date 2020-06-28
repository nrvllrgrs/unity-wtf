using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;

namespace UnityEngine.Workshop
{
	public abstract class StatusEffectFactory : ScriptableObject
	{
		public abstract string key { get; }
		public abstract StatusEffect GetStatusEffect(GameObject target);
	}

	public abstract class StatusEffectFactory<T, K> : StatusEffectFactory
		where T : StatusEffectData
		where K : StatusEffect<T>, new()
	{
		#region Variables

		[HideLabel]
		public T data;

		#endregion

		#region Properties

		public override string key => data.key;

		#endregion

		#region Methods

		public override StatusEffect GetStatusEffect(GameObject target)
		{
			return new K()
			{
				data = data,
				target = target
			};
		}

		#endregion
	}

	[System.Serializable]
	public abstract class StatusEffectData
	{
		#region Variables

		[Header("General Settings")]

		public GameObject attachmentObject;

		#endregion

		#region Properties

		public abstract string key { get; }

		#endregion
	}

	public abstract class StatusEffect
	{
		public bool isRunning { get; protected set; }
		public abstract void Apply(float duration = 0);
		public abstract void Unapply();
	}

	public abstract class StatusEffect<T> : StatusEffect
		where T : StatusEffectData
	{
		#region Variables

		public T data;
		public GameObject target;

		protected Coroutine m_tickThread;
		protected float m_duration;

		private StatusEffectControl m_control;

		#endregion

		#region Properties

		public StatusEffectControl control => target?.GetComponent(ref m_control);

		#endregion

		#region Constructors

		public StatusEffect() {}

		#endregion

		#region Methods

		public override void Apply(float duration = 0)
		{
			if (control == null)
				return;

			if (duration > 0f)
			{
				if (!control.IsStatusEffectRunning(data.key))
				{
					CustomApply();
					m_tickThread = control.StartCoroutine(Tick(duration));
				}
				else
				{
					m_duration += duration;
				}
			}

			// Start running after applying
			// Need to know whether was previously running
			isRunning = true;
		}

		protected virtual void CustomApply() { }

		protected virtual IEnumerator Tick(float duration)
		{
			m_duration = duration;

			while (m_duration > 0)
			{
				float timeSlice = m_duration; 
				yield return new WaitForSeconds(m_duration);

				m_duration -= timeSlice;
				Debug.LogFormat("Remaining Time = {0}", m_duration);
			}

			Unapply();
		}

		public override void Unapply()
		{
			if (control == null)
				return;

			control.StopAndClearCoroutine(m_tickThread);

			if (control.IsStatusEffectRunning(data.key))
			{
				CustomUnapply();

				// Notify unapply has occurred
				control.InvokeUnapplyStatusEffect(data.key);
			}

			// All done
			isRunning = false;
		}

		protected virtual void CustomUnapply() { }

		#endregion
	}
}
