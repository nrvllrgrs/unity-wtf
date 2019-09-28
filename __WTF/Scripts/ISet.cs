namespace UnityEngine.Workshop
{
	public interface ISet
	{
		void Add(GameObject item, bool autoRemoveOnDestroy = true);
		void Remove(GameObject item);
		bool Contains(GameObject item);
	}
}
