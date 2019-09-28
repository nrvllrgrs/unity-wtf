using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

public static class ToolEditor
{
	private const float SNAP_RAYCAST_OFFSET = 1f;

	[MenuItem("GameObject/Transform/Handle Position", false, 49)]
	private static void PrintHandlePosition()
	{
		Debug.LogFormat("Handle Position: ({0}, {1}, {2})", Tools.handlePosition.x, Tools.handlePosition.y, Tools.handlePosition.z);
	}
	
	[MenuItem("GameObject/Transform/Global Position", false, 49)]
	private static void PrintGlobalPosition()
	{
		foreach (var selected in Selection.gameObjects)
		{
			Debug.LogFormat("{3}: Global Position = ({0}, {1}, {2})", selected.transform.position.x, selected.transform.position.y, selected.transform.position.z, selected.name);
		}
	}

	[MenuItem("GameObject/Transform/Global Euler Angles", false, 49)]
	private static void PrintGlobalEulerAngles()
	{
		foreach (var selected in Selection.gameObjects)
		{
			Debug.LogFormat("{3}: Global Euler Angles = ({0}, {1}, {2})", selected.transform.eulerAngles.x, selected.transform.eulerAngles.y, selected.transform.eulerAngles.z, selected.name);
		}
	}

	[MenuItem("GameObject/Transform/Move Center to Origin", false, 49)]
	private static void MoveCenterToOrigin()
	{
		foreach (var selected in Selection.gameObjects)
		{
			var rendererBounds = selected.GetRendererBounds();
			selected.transform.position = -rendererBounds.center;
		}
	}

	[MenuItem("GameObject/Transform/Look At Parent", false, 49)]
	private static void LookAtParent()
	{
		foreach (var selected in Selection.gameObjects)
		{
			if (selected.transform.parent != null)
			{
				selected.transform.LookAt(selected.transform.parent);
			}
		}
	}

	[MenuItem("GameObject/Transform/Snap to Ground", false, 49)]
	private static void SnapToGround()
	{
		foreach (var selected in Selection.gameObjects)
		{
			Vector3 origin = selected.transform.position + (Vector3.up * SNAP_RAYCAST_OFFSET);

			RaycastHit hit;
			if (Physics.Raycast(origin, -Vector3.up, out hit, Mathf.Infinity, ~0))
			{
				selected.transform.position = hit.point;
			}
		}
	}

	[MenuItem("Tools/Camera/Get Editor Camera Position-Rotation")]
	private static void GetEditorCameraPositionRotation()
	{
		Camera camera = SceneView.lastActiveSceneView.camera;
		Debug.LogFormat("Scene View: Camera Position = {0}", camera.transform.position.ToString("F6"));
		Debug.LogFormat("Scene View: Camera Euler Angles = {0}", camera.transform.eulerAngles.ToString("F6"));
	}

	[MenuItem("Tools/Camera/Set Editor Camera Position-Rotation")]
	private static void SetEditorCameraPositionRotation()
	{
		Camera camera = SceneView.lastActiveSceneView.camera;
		Camera.main.transform.SetPositionAndRotation(camera.transform.position, camera.transform.rotation);
	}

	[MenuItem("GameObject/Bounds/Add Box Collider", false, 49)]
	private static void AddBoxBounds()
	{
		foreach (var obj in Selection.gameObjects)
		{
			// Add box collider
			if (obj.TryAddComponent(out BoxCollider boxCollider))
			{
				// Fit collider to geometry
				Bounds bounds = obj.GetRendererBounds();
				boxCollider.center = bounds.center;
				boxCollider.size = bounds.size;
			}
		}
	}

	[MenuItem("GameObject/Bounds/Set Horizontal Center Position", false, 49)]
	private static void SetHorizontalCenterPosition()
	{
		BoxCollider boxCollider = Selection.activeGameObject?.GetComponent<BoxCollider>();
		if (boxCollider != null)
		{
			boxCollider.transform.localPosition = new Vector3(
				GetCenterPosition(boxCollider.center.x, boxCollider.size.x),
				boxCollider.transform.localPosition.y,
				GetCenterPosition(boxCollider.center.z, boxCollider.size.z));
		}
	}

	[MenuItem("GameObject/Select/Match First Mesh", false, 49)]
	private static void SelectMatchFirstMesh()
	{
		Mesh findMesh = (from selected in Selection.gameObjects
						 let meshFilter = selected.GetComponent<MeshFilter>()
						 where meshFilter != null
						 select meshFilter.sharedMesh).FirstOrDefault();

		if (findMesh == null)
		{
			Debug.Log("Cannot make new selection! Current selection does not contain mesh!");
			return;
		}

		var activeScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
		Debug.LogFormat("Getting objects from {0} ({1} root objects)...", activeScene.name, activeScene.GetRootGameObjects().Length);

		List<GameObject> selection = new List<GameObject>();
		foreach (var root in activeScene.GetRootGameObjects())
		{
			foreach (var meshFilter in root.GetComponentsInChildren<MeshFilter>(true))
			{
				if (meshFilter.sharedMesh == findMesh)
				{
					selection.Add(meshFilter.gameObject);
				}
			}
		}

		Selection.objects = selection.ToArray();
	}

	private static float GetCenterPosition(float center, float size)
	{
		return Mathf.Abs((size / 2f) - center) / 2f - center;
	}
}

public class ReplacePrefabWindow : EditorWindow
{
	#region Variables

	private GameObject m_prefab = null;

	#endregion

	#region Methods

	[MenuItem("GameObject/Replace", false, 48)]
	public static void ShowWindow()
	{
		GetWindow(typeof(ReplacePrefabWindow), false, "Replace");
	}

	private void OnGUI()
	{
		GUILayout.Label("Replace Settings", EditorStyles.boldLabel);
#pragma warning disable CS0618 // Type or member is obsolete
		m_prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", obj: m_prefab, objType: typeof(GameObject));
#pragma warning restore CS0618 // Type or member is obsolete

		EditorGUI.BeginDisabledGroup(m_prefab == null);

		if (GUILayout.Button("Replace"))
		{
			var selectedObjects = Selection.gameObjects;
			foreach (var selected in selectedObjects)
			{
				var instance = (GameObject)PrefabUtility.InstantiatePrefab(m_prefab);
				instance.name = selected.name;

				instance.transform.SetParent(selected.transform.parent);
				instance.transform.SetPositionAndRotation(selected.transform.position, selected.transform.rotation);
				instance.transform.localScale = selected.transform.localScale;
			}

			// Destroy selected objects
			selectedObjects.DestroyItemsImmediate();
		}

		EditorGUI.EndDisabledGroup();
	}

	#endregion
}
