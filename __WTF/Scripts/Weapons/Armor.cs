using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
	public class Armor : MonoBehaviour
	{
		#region Variables

		[SerializeField]
		private ArmorCollection m_armors = new ArmorCollection();

		#endregion

		#region Methods

		public ArmorInfo GetArmorInfo(string key)
		{
			return m_armors.GetArmorInfo(key);
		}

#if UNITY_EDITOR

		private void Update()
		{
			if (m_armors == null)
			{
				m_armors = new ArmorCollection();
			}
			else
			{
				m_armors.Refresh();
			}
		}

#endif
		#endregion

		#region Structures

		[System.Serializable]
		public class ArmorCollection : IEnumerable<ArmorInfo>
		{
			#region Variables

			private List<ArmorInfo> m_armors;

			#endregion

			#region Constructors

			public ArmorCollection()
			{
				m_armors = new List<ArmorInfo>();
			}

			#endregion

			#region Methods

			public void Refresh()
			{
				if (!ArmorManager.Ready)
					return;

				if (m_armors == null)
				{
					m_armors = new List<ArmorInfo>();
				}

				var damageTypes = ArmorManager.Instance.GetDamageTypes().ToList();
				//var damageTypes = new List<string>()
				//{
				//	"Slashing", "Bludgeoning", "Piercing", "Fire", "Cold", "Lightning", "Magic"
				//};
				foreach (var key in damageTypes)
				{
					var item = m_armors.SingleOrDefault(x => x.key == key);
					if (item == null)
					{
						m_armors.Add(new ArmorInfo(key));
					}
				}

				// Remove all invalid keys
				for (int i = m_armors.Count - 1; i >= 0; --i)
				{
					if (!damageTypes.Contains(m_armors[i].key))
					{
						m_armors.RemoveAt(i);
					}
				}
			}

			public ArmorInfo GetArmorInfo(string key)
			{
				if (string.IsNullOrWhiteSpace(key))
					return null;

				return m_armors.SingleOrDefault(x => x.key == key);
			}

			IEnumerator<ArmorInfo> IEnumerable<ArmorInfo>.GetEnumerator()
			{
				return m_armors.GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return m_armors.GetEnumerator();
			}

			#endregion
		}

		[System.Serializable]
		public class ArmorInfo
		{
			#region Variables

			[SerializeField]
			private readonly string m_key;

			[SerializeField, Tooltip("Subtractive damage reduction that prevents a fixed amount of damage.")]
			public float absorption;

			[SerializeField, Tooltip("Percentage damage reduction that lowers damage by a fixed fraction of the original damage -- stacking multiplicatively.")]
			public float resistance;

			[SerializeField, Tooltip("Threshold damage reduction prevents all damage above a given threshold, while having no effect on damage below that threshold.")]
			public float threshold;

			#endregion

			#region Properties

			public string key => m_key;

			#endregion

			#region Constructors

			public ArmorInfo(string key)
			{
				m_key = key;
			}

			#endregion
		}

		#endregion
	}

#if UNITY_EDITOR

	public class ArmorDrawer : OdinValueDrawer<Armor.ArmorCollection>
	{
		#region Variables

		private Dictionary<string, bool> m_foldout = new Dictionary<string, bool>();

		#endregion

		protected override void DrawPropertyLayout(GUIContent label)
		{
			bool dirty = false;

			var value = ValueEntry.SmartValue;
			foreach (var info in value)
			{
				if (string.IsNullOrWhiteSpace(info.key))
					continue;

				if (!m_foldout.ContainsKey(info.key))
				{
					m_foldout.Add(info.key, false);
				}

				//TODO: Show this as a FoldoutGroup...somehow

				m_foldout[info.key] = EditorGUILayout.Foldout(m_foldout[info.key], info.key, true, SirenixGUIStyles.Foldout);
				if (m_foldout[info.key])
				{
					++EditorGUI.indentLevel;

					SetValue(ref info.absorption, EditorGUILayout.FloatField("Absorption", info.absorption), ref dirty);
					SetValue(ref info.resistance, Mathf.Min(EditorGUILayout.FloatField("Resistance", info.resistance), 1f), ref dirty);
					SetValue(ref info.threshold, Mathf.Max(EditorGUILayout.FloatField("Threshold", info.threshold), 0f), ref dirty);

					--EditorGUI.indentLevel;
				}
			}

			if (dirty)
			{
				//EditorUtility.SetDirty(value);
			}

			ValueEntry.SmartValue = value;
		}

		private void SetValue<T>(ref T target, T value, ref bool dirty)
		{
			if (!Equals(target, value))
			{
				target = value;
				dirty = true;
			}
		}
	}

#endif
}