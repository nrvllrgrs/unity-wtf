using System.IO;
using UnityEngine;
using Ludiq;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityEngine.Workshop
{
	public enum ActivationType
	{
		None,
		Awake,
		Enable,
	}

	[IncludeInSettings(true)]
	public static class UnityUtil
	{
		#region Methods

		public static float LinearMap(float value, float s0, float s1, float d0, float d1)
		{
			return d0 + (value - s0) * (d1 - d0) / (s1 - s0);
		}

		public static float GetPercent(float value, float min, float max)
		{
			return Mathf.Clamp01((value - min) / (max - min));
		}

		public static float Remap01(float value, float newMin, float newMax)
		{
			return newMin + (GetPercent(value, 0, 1) * (newMax - newMin));
		}

		public static float Remap(float value, float oldMin, float oldMax, float newMin, float newMax)
		{
			return newMin + (GetPercent(value, oldMin, oldMax) * (newMax - newMin));
		}

		public static Vector3 GetHeading(Vector3 forward)
		{
			return Vector3.ProjectOnPlane(forward, Vector3.up).normalized;
		}

		public static Quaternion GetHeadingRotation(Vector3 forward)
		{
			return Quaternion.LookRotation(GetHeading(forward));
		}

		public static void Swap<T>(ref T a, ref T b)
		{
			T t = a;
			a = b;
			b = t;
		}

		public static GameObject GetGameObject<T>(T a)
		{
			if (a == null)
				return null;

			Component component = a as Component;
			return component != null
				? component.gameObject
				: null;
		}

		public static bool InDeadzone(Vector2 axis, float majorAxis, float minorAxis)
		{
			return (axis.x * axis.x) / (majorAxis * majorAxis) + (axis.y * axis.y) / (minorAxis * minorAxis) <= 1;
		}

		public static void DrawCross(Vector3 origin, Color crossColor, float size)
		{
			Vector3 line1Start = origin + (Vector3.right * size);
			Vector3 line1End = origin - (Vector3.right * size);

			Debug.DrawLine(line1Start, line1End, crossColor);

			Vector3 line2Start = origin + (Vector3.up * size);
			Vector3 line2End = origin - (Vector3.up * size);

			Debug.DrawLine(line2Start, line2End, crossColor);

			Vector3 line3Start = origin + (Vector3.forward * size);
			Vector3 line3End = origin - (Vector3.forward * size);

			Debug.DrawLine(line3Start, line3End, crossColor);
		}

		#endregion

		#region Editor Methods
#if UNITY_EDITOR

		public static T CreateAsset<T>()
			where T : ScriptableObject
		{
			T asset = ScriptableObject.CreateInstance<T>();

			string path = AssetDatabase.GetAssetPath(Selection.activeObject);
			if (path == string.Empty)
			{
				path = "Assets";
			}
			else if (Path.GetExtension(path) != string.Empty)
			{
				path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), string.Empty);
			}

			string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(string.Format("{0}/New{1}.asset", path, typeof(T).Name));

			AssetDatabase.CreateAsset(asset, assetPathAndName);

			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
			EditorUtility.FocusProjectWindow();
			Selection.activeObject = asset;

			return asset;
		}

		public static void CreateFolder(string path)
		{
			if (!AssetDatabase.IsValidFolder(path))
			{
				path = path.Replace(@"\", "/");
				string parentFolder = Path.GetDirectoryName(path);

				var split = parentFolder.Split(new[] { '/' });
				parentFolder = "Assets";

				foreach (string folderName in split)
				{
					if (!AssetDatabase.IsValidFolder(parentFolder))
					{
						AssetDatabase.CreateFolder(parentFolder, folderName);
					}
					parentFolder = Path.Combine(parentFolder, folderName);
				}

				string newFolderName = Path.GetFileNameWithoutExtension(path);
				AssetDatabase.CreateFolder(parentFolder, newFolderName);
			}
		}

		public static void CreatePrefab(string path, string name, GameObject gameObject)
		{
			string localPath = string.Format("Assets/{0}/{1}.prefab", path, name);

			// Check if prefab and/or name already exists at path
			if (AssetDatabase.LoadAssetAtPath(localPath, typeof(GameObject)))
			{
				// Create dialog to ask if user is sure they want to overwrite existing prefab
				if (EditorUtility.DisplayDialog(
					"Are you sure?",
					string.Format("The prefab {0} already exists. Do you want to overwrite it?", name),
					"Yes",
					"No"))
				{
					CreateNewPrefab(localPath, gameObject);
				}
			}
			// If name doesn't exist, create new prefab
			else
			{
				CreateNewPrefab(localPath, gameObject);
			}
		}

		private static void CreateNewPrefab(string localPath, GameObject gameObject)
		{
			Object prefab = PrefabUtility.CreatePrefab(localPath, gameObject);
			PrefabUtility.ReplacePrefab(gameObject, prefab, ReplacePrefabOptions.ConnectToPrefab);
		}

#endif
		#endregion
	}
}
