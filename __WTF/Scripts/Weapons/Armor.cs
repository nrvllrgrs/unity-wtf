﻿using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

#if UNITY_EDITOR
using UnityEditor;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
#endif

namespace UnityEngine.Workshop
{
	[ExecuteInEditMode]
	public class Armor : SerializedMonoBehaviour
	{
		#region Variables

		[OdinSerialize]
		private Dictionary<string, ArmorInfo> m_armors;

		#endregion

		#region Methods

		public ArmorInfo GetArmorInfo(string armorKey)
		{
			return armorKey != null && m_armors.TryGetValue(armorKey, out ArmorInfo armorInfo)
				? armorInfo
				: null;
		}

#if UNITY_EDITOR

		private void Update()
		{
			if (!ArmorManager.Ready)
				return;

			if (m_armors == null)
			{
				m_armors = new Dictionary<string, ArmorInfo>();
				foreach (var key in ArmorManager.Instance.GetDamageTypes())
				{
					m_armors.Add(key, new ArmorInfo());
				}
			}
			else
			{
				var armors = new Dictionary<string, ArmorInfo>();
				foreach (var key in ArmorManager.Instance.GetDamageTypes())
				{
					if (m_armors.TryGetValue(key, out ArmorInfo armorInfo))
					{
						armors.Add(key, armorInfo);
					}
					else
					{
						armors.Add(key, new ArmorInfo());
					}
				}

				m_armors = armors;
			}
		}

#endif

		#endregion

		#region Editor Methods
#if UNITY_EDITOR

#endif
		#endregion

		#region Structures

		[System.Serializable]
		public class ArmorInfo
		{
			[Tooltip("Subtractive damage reduction that prevents a fixed amount of damage.")]
			public float absorption;

			[Tooltip("Percentage damage reduction that lowers damage by a fixed fraction of the original damage -- stacking multiplicatively.")]
			public float resistance;

			[Tooltip("Threshold damage reduction prevents all damage above a given threshold, while having no effect on damage below that threshold.")]
			public float threshold;
		}

		#endregion
	}

#if UNITY_EDITOR

	public class ArmorDrawer : OdinValueDrawer<Dictionary<string, Armor.ArmorInfo>>
	{
		#region Variables

		private Dictionary<string, bool> m_foldout = new Dictionary<string, bool>();

		#endregion

		protected override void DrawPropertyLayout(IPropertyValueEntry<Dictionary<string, Armor.ArmorInfo>> entry, GUIContent label)
		{
			var value = entry.SmartValue;
			foreach (var pair in value)
			{
				if (!m_foldout.ContainsKey(pair.Key))
				{
					m_foldout.Add(pair.Key, false);
				}

				m_foldout[pair.Key] = EditorGUILayout.Foldout(m_foldout[pair.Key], pair.Key, true, SirenixGUIStyles.Foldout);
				if (m_foldout[pair.Key])
				{
					pair.Value.absorption = EditorGUILayout.FloatField("Absorption", pair.Value.absorption);
					pair.Value.resistance = Mathf.Min(EditorGUILayout.FloatField("Resistance", pair.Value.resistance), 1f);
					pair.Value.threshold = Mathf.Max(EditorGUILayout.FloatField("Threshold", pair.Value.threshold), 0f);
				}
			}
		}
	}

#endif
}