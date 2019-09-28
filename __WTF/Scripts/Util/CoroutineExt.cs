using System.Collections;
using UnityEngine;

public static class CoroutineExt
{
	public static Coroutine Invoke(this MonoBehaviour behaviour, System.Action action, float delay, bool repeat = false)
	{
		return behaviour.StartCoroutine(InvokeDelay(action, delay, repeat));
	}

	private static IEnumerator InvokeDelay(System.Action action, float delay, bool repeat)
	{
		do
		{
			yield return new WaitForSeconds(delay);
			action?.Invoke();
		} while (repeat);
	}

	public static Coroutine Invoke(this MonoBehaviour behaviour, System.Action action, float initialDelay, float repeatDelay)
	{
		return behaviour.StartCoroutine(InvokeDelay(action, initialDelay, repeatDelay));
	}

	private static IEnumerator InvokeDelay(System.Action action, float initialDelay, float repeatDelay)
	{
		if (initialDelay > 0f)
		{
			yield return new WaitForSeconds(initialDelay);
		}
		action?.Invoke();

		if (repeatDelay > 0f)
		{
			while (true)
			{
				yield return new WaitForSeconds(repeatDelay);
				action?.Invoke();
			}
		}
	}

	public static Coroutine WaitWhile(this MonoBehaviour behaviour, System.Func<bool> func, System.Action action)
	{
		return behaviour.WaitWhile(func, 0, action);
	}

	public static Coroutine WaitWhile(this MonoBehaviour behaviour, System.Func<bool> func, int delayedFrames, System.Action action)
	{
		return behaviour.StartCoroutine(WaitWhile(func, true, delayedFrames, action));
	}

	public static Coroutine WaitUntil(this MonoBehaviour behaviour, System.Func<bool> func, System.Action action)
	{
		return behaviour.WaitUntil(func, 0, action);
	}

	public static Coroutine WaitUntil(this MonoBehaviour behaviour, System.Func<bool> func, int delayedFrames, System.Action action)
	{
		return behaviour.StartCoroutine(WaitWhile(func, false, delayedFrames, action));
	}

	private static IEnumerator WaitWhile(System.Func<bool> func, bool condition, int delayedFrames, System.Action action)
	{
		yield return new WaitWhile(() => func() == condition);

		for (int i = delayedFrames; i < delayedFrames; ++i)
		{
			yield return new WaitForEndOfFrame();
		}
		action?.Invoke();
	}

	public static Coroutine WaitForSeconds(this MonoBehaviour behaviour, float seconds, System.Action action)
	{
		if (seconds > 0)
		{
			return behaviour.StartCoroutine(WaitForSeconds(seconds, action));
		}
		else
		{
			action?.Invoke();
		}
		return null;
	}

	private static IEnumerator WaitForSeconds(float seconds, System.Action action)
	{
		yield return new WaitForSeconds(seconds);
		action?.Invoke();
	}

	public static Coroutine WaitForFrames(this MonoBehaviour behaviour, int delayedFrames, System.Action action)
	{
		if (delayedFrames > 0)
		{
			return behaviour.StartCoroutine(WaitForFrames(delayedFrames, action));
		}
		else
		{
			action?.Invoke();
		}
		return null;
	}

	private static IEnumerator WaitForFrames(int delayedFrames, System.Action action)
	{
		for (int i = 0; i < delayedFrames; ++i)
		{
			yield return new WaitForEndOfFrame();
		}
		action?.Invoke();
	}

	public static Coroutine WaitForEndOfFrame(this MonoBehaviour behaviour, System.Action action)
	{
		return action != null
			? behaviour.StartCoroutine(WaitForEndOfFrame(action))
			: null;
	}

	private static IEnumerator WaitForEndOfFrame(System.Action action)
	{
		yield return new WaitForEndOfFrame();
		action?.Invoke();
	}
	
	public static Coroutine WaitForNextFrame(this MonoBehaviour behaviour, System.Action action)
	{
		return action != null
			? behaviour.WaitForFrames(2, action)
			: null;
	}
	
	public static void StopAndClearCoroutine(this MonoBehaviour behaviour, Coroutine routine)
	{
		if (routine != null)
		{
			behaviour.StopCoroutine(routine);
			routine = null;
		}
	}
}
