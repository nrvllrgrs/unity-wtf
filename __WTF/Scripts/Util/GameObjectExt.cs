using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
#endif

public static class GameObjectExt
{
	public static float SqrDistance(this GameObject obj, GameObject other)
	{
		if (other == null)
			return Mathf.Infinity;

		return obj.transform.position.SqrDistance(other.transform.position);
	}

	public static void DestroyChildren(this GameObject obj)
	{
#if UNITY_EDITOR
		if (Application.isPlaying)
		{
			for (int i = obj.transform.childCount - 1; i >= 0; --i)
			{
				Object.Destroy(obj.transform.GetChild(i).gameObject);
			}
		}
		else
		{
			obj.DestroyChildrenImmediate();
		}
#else
		for (int i = obj.transform.childCount - 1; i >= 0; --i)
		{
			Object.Destroy(obj.transform.GetChild(i).gameObject);
		}
#endif
	}

	public static void DestroyChildren<T>(this GameObject obj)
		where T : Component
	{
		var children = obj.transform.GetComponentsInChildren<T>();
		for (int i = children.Length - 1; i >= 0; --i)
		{
			Object.Destroy(obj.transform.GetChild(i).gameObject);
		}
	}

	public static void DestroyChildrenImmediate(this GameObject obj)
	{
		for (int i = obj.transform.childCount - 1; i >= 0; --i)
		{
			Object.DestroyImmediate(obj.transform.GetChild(i).gameObject);
		}
	}

	public static bool TryAddComponent<T>(this GameObject obj, out T component)
		where T : Component
	{
		var temp = obj.GetComponent<T>();
		if (temp == null)
		{
			component = obj.AddComponent<T>();
			return true;
		}

		component = temp;
		return false;
	}

	public static T AddComponent<T>(this GameObject obj, T source)
		where T : Component
	{
		T destination = obj.AddComponent<T>();
		source.CopyTo(destination);

		return destination;
	}

	public static Collider AddCollider(this GameObject obj, Collider srcCollider)
	{
		if (srcCollider == null)
			return null;

		if (srcCollider is BoxCollider)
		{
			return obj.AddComponent(srcCollider as BoxCollider);
		}
		else if (srcCollider is SphereCollider)
		{
			return obj.AddComponent(srcCollider as SphereCollider);
		}
		else if (srcCollider is CapsuleCollider)
		{
			return obj.AddComponent(srcCollider as CapsuleCollider);
		}
		else if (srcCollider is MeshCollider)
		{
			return obj.AddComponent(srcCollider as MeshCollider);
		}

		return null;
	}

	public static void CopyTo<T>(this T src, T dst)
		where T : Component
	{
		BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;
		foreach (var propertyInfo in src.GetType().GetProperties(flags))
		{
			if (propertyInfo.CanWrite)
			{
				try
				{
					propertyInfo.SetValue(dst, propertyInfo.GetValue(src), null);
				}
				catch { }
			}
		}

		foreach (var fieldInfo in src.GetType().GetFields(flags))
		{
			fieldInfo.SetValue(dst, fieldInfo.GetValue(src));
		}
	}

	public static void DestroyComponentsInChildren<T>(this Component component)
		where T : Component
	{
		DestroyComponentsInChildren<T>(component.gameObject);
	}

	public static void DestroyComponentsInChildren<T>(this GameObject gameObject)
		where T : Component
	{
		var components = gameObject.GetComponentsInChildren<T>();
		components.DestroyItems();
	}

	public static Transform[] GetAncestors(this Transform transform)
	{
		List<Transform> ancenstors = new List<Transform>();
		ancenstors.Add(transform);

		if (transform.parent != null)
		{
			ancenstors.AddRange(GetAncestors(transform.parent));
		}

		return ancenstors.ToArray();
	}

	public static Transform[] GetDescendants(this Transform transform)
	{
		List<Transform> children = new List<Transform>();
		children.Add(transform);

		for (int i = 0; i < transform.childCount; ++i)
		{
			children.AddRange(GetDescendants(transform.GetChild(i)));
		}

		return children.ToArray();
	}

	public static string GetHierarchyPath(this Transform transform)
	{
		string path = transform.name;

		Transform parent = transform.parent;
		while (parent != null)
		{
			path = string.Format("{0}/{1}", parent.name, path);
			parent = parent.parent;
		}

		return path;
	}

	public static void SetActive(this IEnumerable<GameObject> list, bool isActive)
	{
		foreach (var gameObject in list)
		{
			gameObject.SetActive(isActive);
		}
	}

	public static void SetChildrenActive(this Transform transform, bool isActive)
	{
		List<GameObject> childrenObjects = new List<GameObject>();
		for (int i = 0; i < transform.childCount; ++i)
		{
			childrenObjects.Add(transform.GetChild(i).gameObject);
		}

		childrenObjects.SetActive(isActive);
	}

	public static void Show(this GameObject obj)
	{
		obj.Show(true, false);
	}

	public static void ForceShow(this GameObject obj)
	{
		obj.Show(true, true);
	}

	public static void Hide(this GameObject obj)
	{
		obj.Show(false, false);
	}

	public static void ForceHide(this GameObject obj)
	{
		obj.Show(false, true);
	}

	private static void Show(this GameObject obj, bool isShown, bool force)
	{
		// Hide hand controller
		var rendererControl = obj.GetComponent<RendererControl>();
		if (rendererControl != null)
		{
			if (isShown)
			{
				if (force)
				{
					rendererControl.ForceShow();
				}
				else
				{
					rendererControl.Show();
				}
			}
			else
			{
				if (force)
				{
					rendererControl.ForceHide();
				}
				else
				{
					rendererControl.Hide();
				}
			}
		}
	}

	public static bool IsShowing(this GameObject obj)
	{
		var rendererControl = obj.GetComponent<RendererControl>();
		if (rendererControl != null)
		{
			return rendererControl.showing;
		}

		return obj.activeInHierarchy;
	}

	public static void AddEvent(this EventTrigger trigger, EventTriggerType triggerType, System.Action<BaseEventData> action)
	{
		var entry = new EventTrigger.Entry()
		{
			eventID = triggerType,
			callback = new EventTrigger.TriggerEvent()
		};
		entry.callback.AddListener(new UnityEngine.Events.UnityAction<BaseEventData>(action));

		trigger.triggers.Add(entry);
	}

	public static void RemoveEvent(this EventTrigger trigger, EventTriggerType triggerType, System.Action<BaseEventData> action)
	{
		var entry = new EventTrigger.Entry()
		{
			eventID = triggerType,
			callback = new EventTrigger.TriggerEvent()
		};
		entry.callback.RemoveListener(new UnityEngine.Events.UnityAction<BaseEventData>(action));

		trigger.triggers.Remove(entry);
	}

	public static GameObject FindParentWithTag(this GameObject obj, string tag)
	{
		if (obj == null || tag == null)
		{
			return null;
		}

		Transform target = obj.transform;
		while (target.parent != null)
		{
			if (target.parent.tag == tag)
			{
				return target.parent.gameObject;
			}
			target = target.parent;
		}

		if (target.tag == tag)
		{
			Debug.LogWarningFormat("Could not find parent with tag {0}., This gameObject has tag {0}", tag);
			return target.gameObject;
		}

		return null; // Could not find a parent with given tag.
	}
	
	public static void MoveToScene(this GameObject obj, Scene scene, bool force = false)
	{
		if (Equals(obj.scene, scene))
			return;

		if (obj.transform.parent != null && !force)
		{
			Debug.LogErrorFormat("Could not move {0} to {1}! Object is not root.", obj.name, scene.name);
			return;
		}
		
		obj.transform.SetParent(null);
		SceneManager.MoveGameObjectToScene(obj, scene);
	}

	public static Bounds GetRendererBounds(this GameObject gameObject)
	{
		// Remember current position / rotation
		Vector3 position = gameObject.transform.position;
		Quaternion rotation = gameObject.transform.rotation;

		// Set position / rotation for bounds calculation
		gameObject.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);

		float minX = Mathf.Infinity, minY = Mathf.Infinity, minZ = Mathf.Infinity;
		float maxX = Mathf.NegativeInfinity, maxY = Mathf.NegativeInfinity, maxZ = Mathf.NegativeInfinity;

		foreach (var meshFilter in gameObject.GetComponentsInChildren<SkinnedMeshRenderer>())
		{
			var verts = meshFilter.sharedMesh.vertices.Select(v => meshFilter.transform.rotation * v);
			var xVerts = verts.Select(x => x.x);
			var yVerts = verts.Select(x => x.y);
			var zVerts = verts.Select(x => x.z);

			// Min verts
			minX = Mathf.Min(new float[] { minX }.Concat(xVerts).ToArray());
			minY = Mathf.Min(new float[] { minY }.Concat(yVerts).ToArray());
			minZ = Mathf.Min(new float[] { minZ }.Concat(zVerts).ToArray());

			// Max verts
			maxX = Mathf.Max(new float[] { maxX }.Concat(xVerts).ToArray());
			maxY = Mathf.Max(new float[] { maxY }.Concat(yVerts).ToArray());
			maxZ = Mathf.Max(new float[] { maxZ }.Concat(zVerts).ToArray());
		}

		Bounds bounds = new Bounds();
		bounds.center = new Vector3(
			(minX + maxX) / 2f,
			(minY + maxY) / 2f,
			(minZ + maxZ) / 2f);

		bounds.extents = new Vector3(
			maxX - minX,
			maxY - minY,
			maxZ - minZ);

		// Restore previous position / rotation
		gameObject.transform.SetPositionAndRotation(position, rotation);

		return bounds;
	}

	public static Bounds GetColliderBounds(this GameObject gameObject)
	{
		// Remember current position / rotation
		Vector3 position = gameObject.transform.position;
		Quaternion rotation = gameObject.transform.rotation;

		// Set position / rotation for bounds calculation
		gameObject.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);

		Bounds bounds = new Bounds();
		foreach (var collider in gameObject.GetComponentsInChildren<Collider>())
		{
			bounds.Encapsulate(collider.bounds);
		}

		// Restore previous position / rotation
		gameObject.transform.SetPositionAndRotation(position, rotation);

		return bounds;
	}

	public static void SetLayer(this GameObject gameObject, int layer, bool includeChildren = false)
	{
		gameObject.layer = layer;
		
		if (includeChildren)
		{
			for (int i = 0; i < gameObject.transform.childCount; ++i)
			{
				gameObject.transform.GetChild(i).gameObject.SetLayer(layer, includeChildren);
			}
		}
	}

	public static void GetRelativePositionAndRotation(this Transform transform, Transform relativeTo, out Vector3 localPosition, out Quaternion localRotation)
	{
		transform.GetRelativePositionAndRotation(relativeTo.position, relativeTo.rotation, out localPosition, out localRotation);
	}

	public static void GetRelativePositionAndRotation(this Transform transform, Vector3 relativePosition, Quaternion relativeRotation, out Vector3 localPosition, out Quaternion localRotation)
	{
		localPosition = transform.InverseTransformPoint(relativePosition);
		localRotation = Quaternion.Inverse(transform.rotation) * relativeRotation;
	}

	public static void SetRelativePositionAndRotation(this Transform transform, Transform relativeTo, Transform target)
	{
		transform.SetRelativePositionAndRotation(relativeTo, target.position, target.rotation);
	}

	public static void SetRelativePositionAndRotation(this Transform transform, Transform relativeTo, Vector3 position, Quaternion rotation)
	{
		transform.GetRelativePositionAndRotation(relativeTo, out Vector3 localPosition, out Quaternion localRotation);
		rotation *= localRotation;
		position += rotation * localPosition;

		transform.SetPositionAndRotation(position, rotation);
	}

	public static T GetComponent<T>(this GameObject gameObject, ref T value)
		where T : Component
	{
		if (value == null)
		{
			value = gameObject.GetComponent<T>();
		}
		return value;
	}

	public static T GetComponent<T>(this Component component, ref T value)
		where T : Component
	{
		return component.gameObject.GetComponent(ref value);
	}

	public static T GetComponentInChildren<T>(this GameObject gameObject, ref T value)
		where T : Component
	{
		if (value == null)
		{
			value = gameObject.GetComponentInChildren<T>();
		}
		return value;
	}

	public static T GetComponentInChildren<T>(this Component component, ref T value)
		where T : Component
	{
		return component.gameObject.GetComponentInChildren(ref value);
	}

	public static T[] GetComponentsInChildren<T>(this GameObject gameObject, ref T[] value)
		where T : Component
	{
		if (value == null || value.Length == 0)
		{
			value = gameObject.GetComponentsInChildren<T>();
		}
		return value;
	}

	public static T GetComponentInParent<T>(this GameObject gameObject, ref T value)
		where T : Component
	{
		if (value == null)
		{
			value = gameObject.GetComponentInParent<T>();
		}
		return value;
	}

	public static T GetComponentInParent<T>(this Component component, ref T value)
		where T : Component
	{
		return component.gameObject.GetComponentInParent(ref value);
	}

	public static T[] GetComponentsInParent<T>(this GameObject gameObject, ref T[] value)
		where T : Component
	{
		if (value == null || value.Length == 0)
		{
			value = gameObject.GetComponentsInParent<T>();
		}
		return value;
	}

	public static GameObject CloneRenderers(this GameObject obj, bool includeAnimators = false)
	{
		if (TryCloneRenderers(obj, null, out GameObject clone, includeAnimators))
		{
			return clone;
		}
		return null;
	}

	public static GameObject CloneRenderers(this GameObject obj, string childName, bool includeAnimators = false)
	{
		Transform child = obj.transform.Find(childName);
		if (child != null)
		{
			GameObject clone = child.gameObject.CloneRenderers(includeAnimators);
			if (clone != null)
			{
				GameObject cloneParent = new GameObject(obj.name);
				clone.transform.SetParent(cloneParent.transform);

				// Set offset
				obj.transform.GetRelativePositionAndRotation(child.transform, out Vector3 localPosition, out Quaternion localRotation);
				clone.transform.localPosition = localPosition;
				clone.transform.localRotation = localRotation;

				return cloneParent;
			}
		}
		return null;
	}

	private static bool TryCloneRenderers(this GameObject obj, Transform parent, out GameObject clone, bool includeAnimators)
	{
		// Initalize output
		bool result = false;

		// Create clone
		clone = new GameObject(obj.name);
		if (parent != null)
		{
			clone.transform.SetParent(parent);
		}

		clone.transform.localPosition = obj.transform.localPosition;
		clone.transform.localRotation = obj.transform.localRotation;
		clone.transform.localScale = obj.transform.localScale;

		// Clone children
		// Must be done before animator so it can properly rebind to transforms
		for (int i = 0; i < obj.transform.childCount; ++i)
		{
			result |= TryCloneRenderers(obj.transform.GetChild(i).gameObject, clone.transform, out GameObject childClone, includeAnimators);
		}

		// Get renderer / mesh filter from object
		if (obj.activeInHierarchy)
		{
			if (includeAnimators)
			{
				var srcAnimator = obj.GetComponent<Animator>();
				if (srcAnimator != null && srcAnimator.enabled)
				{
					// Create animator
					var dstAnimator = clone.AddComponent(srcAnimator);
					dstAnimator.Rebind();

					result = true;
				}
			}

			var srcRenderer = obj.GetComponent<MeshRenderer>();
			if (srcRenderer != null && srcRenderer.enabled)
			{
				var srcMeshFilter = obj.GetComponent<MeshFilter>();
				if (srcMeshFilter != null)
				{
					// Create renderer / mesh filter
					// Copy renderer / mesh filter fields
					var dstRenderer = clone.AddComponent(srcRenderer);
					var dstMeshFilter = clone.AddComponent(srcMeshFilter);

					var dstMaterials = new Material[srcRenderer.materials.Length];
					for (int i = 0; i < srcRenderer.materials.Length; ++i)
					{
						dstMaterials[i] = Object.Instantiate(srcRenderer.materials[i]);
					}

					// Cannot modify materials in loop
					// Must create list and then assign
					dstRenderer.materials = dstMaterials;
					result = true;
				}
			}
		}

		if (!result)
		{
			Object.Destroy(clone);
		}

		return result;
	}
}
