namespace UnityEngine.SceneManagement
{
	public static class SceneManagerUtil
	{
		#region Methods

		public static bool IsLoaded(Scene scene)
		{
			return IsLoaded(scene.name);
		}

		public static bool IsLoaded(string sceneName)
		{
			for (int i = 0; i < SceneManager.sceneCount; ++i)
			{
				Scene loadedScene = SceneManager.GetSceneAt(i);
				if (loadedScene.name == sceneName)
					return true;
			}

			return false;
		}

		#endregion
	}
}