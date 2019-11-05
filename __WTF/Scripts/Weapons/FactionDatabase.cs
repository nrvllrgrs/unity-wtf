using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;

#if UNITY_EDITOR
using UnityEditor;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
#endif

namespace UnityEngine.Workshop
{
	public class FactionDatabase : SerializedScriptableObject
	{
		#region Variables

		[SerializeField]
		private FactionInfo[] m_factions;

		[Header("Damage Factor Settings")]

		[SerializeField]
		private Dictionary<Faction.RelationshipType, RelationshipMapInfo> m_relationshipMap;

		[SerializeField, HideLabel]
		private RelationshipMapInfo m_playerFactionMap;

		[Header("Relationship Settings")]

		[SerializeField]
		private Dictionary<string, Dictionary<string, RelationshipInfo>> m_relationships;

		#endregion

		#region Properties

		#endregion

		#region Methods

		public string[] GetFactionNames() => m_factions.Select(x => x.name).ToArray();

		public FactionInfo GetFaction(string key)
		{
			return m_factions.SingleOrDefault(x => x.name == key);
		}

		public Faction.RelationshipType GetRelationship(Faction a, Faction b)
		{
			if (a == null || b == null)
			{
				return Faction.RelationshipType.Neutral;
			}

			// On the same team, obviously friends
			if (a.key == b.key)
			{
				return Faction.RelationshipType.Friend;
			}

			// Sort in alphanumeric lookup
			string m, n;
			if (a.key.CompareTo(b.key) < 0)
			{
				m = a.key;
				n = b.key;
			}
			else
			{
				m = b.key;
				n = a.key;
			}

			return m_relationships[m][n].relationship;
		}

		public float GetDamageFactor(Faction.RelationshipType relationship)
		{
			return m_relationshipMap.TryGetValue(relationship, out RelationshipMapInfo value)
				? value.damageFactor
				: 1f;
		}

		public float GetPlayerFactionDamageFactor() => m_playerFactionMap.damageFactor;

		#endregion

		#region Editor Methods
#if UNITY_EDITOR

		[MenuItem("Assets/Create/Workshop/Faction Database")]
		private static void CreateAsset()
		{
			var data = UnityUtil.CreateAsset<FactionDatabase>();
		}

		public void Refresh()
		{
			var relationshipMap = new Dictionary<Faction.RelationshipType, RelationshipMapInfo>();
			EnumUtil.Iterate<Faction.RelationshipType>((relationship) =>
			{
				GetRelationshipMap(relationship, relationshipMap);
			});

			// Assign relationship map
			m_relationshipMap = relationshipMap;

			if (m_factions != null)
			{
				// Get factions to alphanumeric order
				var sortedFactions = GetFactionNames().OrderBy(x => x).ToArray();

				// Add faction-faction pair
				for (int i = 0; i < sortedFactions.Length - 1; ++i)
				{
					for (int j = i + 1; j < sortedFactions.Length; ++j)
					{
						GetRelationship(sortedFactions[i], sortedFactions[j], true);
					}
				}

				var deadFactions = m_relationships.Keys.Except(GetFactionNames());
				foreach (var deadFaction in deadFactions)
				{
					m_relationships.Remove(deadFaction);
				}

				foreach (var pair in m_relationships)
				{
					foreach (var deadFaction in pair.Value.Keys.Except(GetFactionNames()))
					{
						pair.Value.Remove(deadFaction);
					}
				}
			}
		}

		private void GetRelationshipMap(Faction.RelationshipType relationship, Dictionary<Faction.RelationshipType, RelationshipMapInfo> map)
		{
			if (m_relationshipMap != null && m_relationshipMap.ContainsKey(relationship))
			{
				map.Add(relationship, m_relationshipMap[relationship]);
			}
			else
			{
				map.Add(relationship, new RelationshipMapInfo()
				{
					damageFactor = 1f
				});
			}
		}

		private RelationshipInfo GetRelationship(string key, string otherKey, bool initialize = false)
		{
			RelationshipInfo relationship;
			if (m_relationships == null || !m_relationships.ContainsKey(key))
			{
				if (!initialize)
					return null;

				// Setup return value
				relationship = new RelationshipInfo();

				var dict = new Dictionary<string, RelationshipInfo>();
				dict.Add(otherKey, relationship);

				if (m_relationships == null)
				{
					m_relationships = new Dictionary<string, Dictionary<string, RelationshipInfo>>();
				}
				m_relationships.Add(key, dict);

				return relationship;
			}
			else
			{
				var dict = m_relationships[key];
				if (!dict.TryGetValue(otherKey, out relationship) && initialize)
				{
					relationship = new RelationshipInfo();
					dict.Add(otherKey, relationship);
				}

				return relationship;
			}
		}

#endif
		#endregion

		#region Structures

		[System.Serializable]
		public class FactionInfo
		{
			public string name;
			public bool isPlayerFaction;
		}

		[System.Serializable]
		public class RelationshipMapInfo
		{
			[Range(0f, 1f)]
			public float damageFactor = 1f;
		}

		[System.Serializable]
		public class RelationshipInfo
		{
			public Faction.RelationshipType relationship = Faction.RelationshipType.Enemy;
		}

		#endregion
	}

#if UNITY_EDITOR

	public class RelationshipMapDrawer : OdinValueDrawer<Dictionary<Faction.RelationshipType, FactionDatabase.RelationshipMapInfo>>
	{
		#region Methods

		protected override void DrawPropertyLayout(IPropertyValueEntry<Dictionary<Faction.RelationshipType, FactionDatabase.RelationshipMapInfo>> entry, GUIContent label)
		{
			var value = entry.SmartValue;
			foreach (var pair in value)
			{
				pair.Value.damageFactor = EditorGUILayout.Slider(pair.Key.ToString(), pair.Value.damageFactor, 0f, 1f);
			}
		}

		#endregion
	}

	public class RelationshipDrawer : OdinValueDrawer<Dictionary<string, Dictionary<string, FactionDatabase.RelationshipInfo>>>
	{
		#region Variables

		private Dictionary<string, bool> m_foldout = new Dictionary<string, bool>();
		#endregion

		#region Methods

		protected override void DrawPropertyLayout(IPropertyValueEntry<Dictionary<string, Dictionary<string, FactionDatabase.RelationshipInfo>>> entry, GUIContent label)
		{
			var value = entry.SmartValue;
			foreach (var p in value)
			{
				if (!p.Value.Any())
					continue;

				if (!m_foldout.ContainsKey(p.Key))
				{
					m_foldout.Add(p.Key, false);
				}

				m_foldout[p.Key] = EditorGUILayout.Foldout(m_foldout[p.Key], p.Key, true, SirenixGUIStyles.Foldout);
				if (m_foldout[p.Key])
				{
					++EditorGUI.indentLevel;
					foreach (var q in p.Value)
					{
						q.Value.relationship = (Faction.RelationshipType)EditorGUILayout.EnumPopup(q.Key, q.Value.relationship);
					}

					--EditorGUI.indentLevel;
				}
			}
		}

		#endregion
	}

#endif
}