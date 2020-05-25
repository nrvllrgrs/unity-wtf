using UnityEngine;

public static class PhysicsUtil
{
	#region Methods

	public static void IgnoreCollision(GameObject obj, GameObject otherObj, bool ignore)
	{
		var colliders = obj.GetComponentsInChildren<Collider>(true);
		var otherColliders = otherObj.GetComponentsInChildren<Collider>(true);

		foreach (var collider in colliders)
		{
			foreach (var otherCollider in otherColliders)
			{
				Physics.IgnoreCollision(collider, otherCollider, ignore);
			}
		}
	}

	#endregion
}
