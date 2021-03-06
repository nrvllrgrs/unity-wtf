﻿namespace UnityEngine.AddressableAssets
{
	public class PersistentAddressable : MonoBehaviour
	{
#if ADDRESSABLE_ASSETS

		#region Variables

		[SerializeField]
		private AssetReferenceGameObject m_asset;

		#endregion

		#region Properties

		public AssetReferenceGameObject asset { get => m_asset; set => m_asset = value; }

		#endregion

		#region Static Methods

		public static void SetAsset(GameObject obj, AssetReferenceGameObject asset)
		{
			if (obj == null)
				return;

			var persistentAddressable = obj.GetComponent<PersistentAddressable>();
			if (persistentAddressable != null)
			{
				persistentAddressable.asset = asset;
			}
		}

		public static AssetReferenceGameObject GetAsset(GameObject obj)
		{
			if (obj == null)
				return null;

			TryGetAsset(obj, out AssetReferenceGameObject asset);
			return asset;
		}

		public static bool TryGetAsset(GameObject obj, out AssetReferenceGameObject asset)
		{
			var persistentAddressable = obj.GetComponent<PersistentAddressable>();
			if (persistentAddressable != null)
			{
				asset = persistentAddressable.asset;
				return true;
			}

			asset = null;
			return false;
		}

		#endregion
#endif
	}
}
