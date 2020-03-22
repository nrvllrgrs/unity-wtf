using UnityEngine;
using UnityEngine.Workshop;
using Ludiq;

[IncludeInSettings(true)]
public static class GameObjectUtil
{
	#region Instantiate Methods

	public static GameObject Instantiate(GameObject template)
	{
		return Instantiate(template, null);
	}

	public static T Instantiate<T>(T template)
		where T : Component
	{
		return Instantiate(template.gameObject, null).GetComponent<T>();
	}

	public static GameObject Instantiate(GameObject template, Transform parent)
	{
		return Instantiate(template, parent, true);
	}

	public static T Instantiate<T>(T template, Transform parent)
		where T : Component
	{
		return Instantiate(template.gameObject, parent).GetComponent<T>();
	}

	public static GameObject Instantiate(GameObject template, Transform parent, bool instantiateInWorldSpace)
	{
		return instantiateInWorldSpace || parent == null
			? Instantiate(template, Vector3.zero, Quaternion.identity, parent)
			: Instantiate(template, parent.transform.position, parent.transform.rotation, parent);
	}

	public static T Instantiate<T>(T template, Transform parent, bool instantiateInWorldSpace)
		where T : Component
	{
		return Instantiate(template.gameObject, parent, instantiateInWorldSpace).GetComponent<T>();
	}

	public static GameObject Instantiate(GameObject template, Vector3 position, Quaternion rotation)
	{
		return Instantiate(template, position, rotation, null);
	}

	public static T Instantiate<T>(T template, Vector3 position, Quaternion rotation)
		where T : Component
	{
		return Instantiate(template.gameObject, position, rotation).GetComponent<T>();
	}

	public static GameObject Instantiate(GameObject template, Vector3 position, Quaternion rotation, Transform parent)
	{
		PoolItem item = template.GetComponent<PoolItem>();
		if (!PoolManager.Exists || item == null || PoolManager.Instance.CanInstantiate(item))
		{
			return Object.Instantiate(template, position, rotation, parent);
		}

		var instance = PoolManager.Instance.Get(item);
		if (instance == null)
		{
			Debug.LogErrorFormat("Could not instantiate {0}! You may want to increase the {1} limit.", item.name, item.key);
			return null;
		}

		// Set position and rotation
		instance.gameObject.transform.SetPositionAndRotation(position, rotation);

		// Set parent
		if (parent != null)
		{
			instance.transform.parent.SetParent(parent);
		}

		// Activate object
		instance.gameObject.SetActive(true);
		return instance.gameObject;
	}

	public static T Instantiate<T>(T template, Vector3 position, Quaternion rotation, Transform parent)
		where T : Component
	{
		return Instantiate(template.gameObject, position, rotation, parent).GetComponent<T>();
	}

	#endregion

	#region Destroy Methods

	public static void Destroy(GameObject obj)
	{
		var poolItem = obj.GetComponent<PoolItem>();
		if (poolItem == null || !PoolManager.Exists)
		{
			Object.Destroy(obj);
		}

		// Item exists in a pool...
		// Turn off entity
		obj.SetActive(false);
	}

	public static void Destroy(GameObject obj, float t)
	{
		var poolItem = obj.GetComponent<PoolItem>();
		if (poolItem == null || !PoolManager.Exists)
		{
			Object.Destroy(obj, t);
		}
		else
		{
			// Item exists in a pool...
			// Turn off entity after delay
			poolItem.WaitForSeconds(t, () =>
			{
				obj.SetActive(false);
			});
		}
	}

	#endregion
}
