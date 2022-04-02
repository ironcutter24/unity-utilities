using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
	public static class Util
	{
		public static Vector3Int ScreenSize { get { return Vector3Int.right * Screen.width + Vector3Int.up * Screen.height; } }

		public static Vector3 ScreenCenter { get { return Vector3.right * ScreenSize.x * .5f + Vector3.up * ScreenSize.y * .5f; } }

		public static bool Chance(float chanceOfSuccess)
		{
			return Random.Range(0f, 1f) <= chanceOfSuccess;
		}

		public static void TryAction(System.Action action)
		{
			if (action != null)
				action();
		}

		public static void TryAction(System.Action<float> action, float parameter)
		{
			if (action != null)
				action(parameter);
		}

		public static IEnumerator _Transition(float duration, System.Action<float> callback)
		{
			float mult = 1 / duration;
			float state = 0;
			while (state < 1f)
			{
				callback(state);
				yield return null;
				state = Mathf.Clamp(state + Time.deltaTime * mult, 0f, 1f);
			}
			callback(state);
		}
	}

	public static class UMath
	{
		public static float Normalize(float value, float min, float max)
		{
			return (value - min) / (max - min);
		}

		public static float DivideByZero(float dividend, float divider)
		{
			try
			{
				return dividend / divider;
			}
			catch (System.DivideByZeroException)
			{
				return float.PositiveInfinity;
			}
		}
	}

	public class Timer
	{
		private float timeFromStart = Mathf.NegativeInfinity;
		private float duration;

		public float Elapsed { get { return Time.time - timeFromStart; } }
		public bool IsExpired { get { return Elapsed > duration; } }
		public float Completion { get { return UMath.Normalize(Elapsed, 0f, duration); } }


		public void Set(float duration)
		{
			timeFromStart = Time.time;
			this.duration = duration;
		}
	}

	public class OneTimeEvent
	{
		bool performed = false;

		System.Func<bool> Condition;
		System.Action Callback;

		public OneTimeEvent(System.Func<bool> condition, System.Action callback)
		{
			this.Condition = condition;
			this.Callback = callback;
		}

		public void Evaluate()
		{
			if (!performed && Condition())
			{
				Callback();
				performed = true;
			}
		}

		public void Reset()
		{
			performed = false;
		}
	}
}