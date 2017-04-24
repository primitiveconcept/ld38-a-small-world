namespace PrimordialOoze.Extensions.Coroutines
{
	using System;
	using System.Collections;
	using UnityEngine;


	public static class CoroutineExtensions
	{
		public static void WaitForNextFrame(
			this MonoBehaviour monoBehaviour,
			Action callback)
		{
			monoBehaviour.StartCoroutine(Wait(callback));
		}


		public static void WaitForSeconds(
			this MonoBehaviour monoBehaviour,
			float seconds,
			Action callback)
		{
			monoBehaviour.StartCoroutine(Wait(seconds, callback));
		}


		#region Helper Methods
		private static IEnumerator Wait(Action callback)
		{
			yield return null;

			if (callback != null)
				callback();
		}


		private static IEnumerator Wait(float seconds, Action callback)
		{
			yield return new WaitForSeconds(seconds);

			if (callback != null)
				callback();
		}
		#endregion
	}
}