namespace PrimordialOoze
{
	using System;
	using System.Collections;
	using UnityEngine;


	public interface IInjectable
	{
		bool CanBeInjectedBy(Microbe injector);
		void CompleteInjection(Microbe injector);
	}


	public static class InjectableExtensions
	{
		public static IEnumerator ZoomOnInjection(this MonoBehaviour monoBehaviour, Action callback)
		{
			while (Camera.main.orthographicSize > monoBehaviour.transform.localScale.x + 0.5f)
			{
				Camera.main.orthographicSize = Mathf.Lerp(
					Camera.main.orthographicSize,
					monoBehaviour.transform.localScale.x,
					Time.deltaTime * 15);
				yield return null;
			}

			callback();
		}
	}
}
