namespace PrimordialOoze.Extensions.Camera
{
	using System;
	using System.Collections;
	using PrimordialOoze.Extensions.Vectors;
	using UnityEngine;
	using Coroutines = PrimordialOoze.Coroutines;


	public static class CameraExtensions
	{
		public static IEnumerator PerformVibrateCoroutine(
			Camera camera,
			float magnitude,
			float frequency,
			float duration,
			Action callback = null)
		{
			Vector3 originalPosition = camera.transform.position;
			int iterations = (int)(duration / frequency);

			for (int i = 0; i < iterations; i++)
			{
				float sign = (i % 2 == 0)
								? -1
								: 1;

				camera.transform.position = camera.transform.position.AdjustX(magnitude * sign);
				yield return new WaitForSeconds(frequency);
			}

			camera.transform.position = originalPosition;

			if (callback != null)
				callback();
		}


		public static IEnumerator PerformZoomCoroutine(
			Camera camera,
			float toSize,
			float overSeconds,
			Action callback = null)
		{
			float originalZoom = camera.orthographicSize;
			float timePassed = 0;
			while (timePassed < overSeconds)
			{
				camera.orthographicSize = Mathf.Lerp(
					originalZoom,
					toSize,
					timePassed / overSeconds);
				timePassed += Time.deltaTime;
				yield return null;
			}
			camera.orthographicSize = toSize;

			if (callback != null)
				callback();
		}


		public static void Vibrate(
			this Camera camera,
			float magnitude,
			float frequency,
			float duration,
			Action callback = null)
		{
			Coroutines.StartGlobal(
				PerformVibrateCoroutine(
					camera,
					magnitude,
					frequency,
					duration,
					callback));
		}


		public static void Zoom(this Camera camera, float toSize, float overSeconds, Action callback = null)
		{
			Coroutines.StartGlobal(
				PerformZoomCoroutine(
					camera,
					toSize,
					overSeconds,
					callback
				));
		}
	}
}