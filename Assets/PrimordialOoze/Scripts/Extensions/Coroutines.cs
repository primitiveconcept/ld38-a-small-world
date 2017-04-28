namespace PrimordialOoze
{
	using System;
	using System.Collections;
	using UnityEngine;
	using UnityEngine.UI;


	public class Coroutines : MonoBehaviour
	{
		private static Coroutines _instance;


		#region Properties
		public static Coroutines Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new GameObject("Global Coroutines").AddComponent<Coroutines>();
					_instance.gameObject.hideFlags = HideFlags.HideInHierarchy;
				}

				return _instance;
			}
		}
		#endregion


		/// <summary>
		/// Execute an action after given number of seconds.
		/// </summary>
		/// <param name="seconds">Seconds to wait.</param>
		/// <param name="callback">Action to perform.</param>
		/// <returns></returns>
		public static IEnumerator Delay(float seconds, Action callback)
		{
			yield return new WaitForSeconds(seconds);
			callback();
		}


		/// <summary>
		/// Fades the specified image to the target opacity and duration.
		/// </summary>
		/// <param name="target">Target.</param>
		/// <param name="opacity">Opacity.</param>
		/// <param name="duration">Duration.</param>
		public static IEnumerator FadeImage(
			Image target,
			float duration,
			Color color,
			Action callback = null)
		{
			if (target == null)
				yield break;

			float alpha = target.color.a;

			for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / duration)
			{
				if (target == null)
					yield break;
				Color newColor = new Color(color.r, color.g, color.b, Mathf.SmoothStep(alpha, color.a, t));
				target.color = newColor;
				yield return null;
			}
			target.color = color;
			if (callback != null)
				callback();
		}


		/// <summary>
		/// Fades the specified image to the target opacity and duration.
		/// </summary>
		/// <param name="target">Target.</param>
		/// <param name="opacity">Opacity.</param>
		/// <param name="duration">Duration.</param>
		public static IEnumerator FadeSprite(SpriteRenderer target, float duration, Color color, Action callback = null)
		{
			if (target == null)
				yield break;

			float alpha = target.material.color.a;

			float t = 0f;
			while (t < 1.0f)
			{
				if (target == null)
					yield break;

				Color newColor = new Color(color.r, color.g, color.b, Mathf.SmoothStep(alpha, color.a, t));
				target.material.color = newColor;

				t += Time.deltaTime / duration;

				yield return null;
			}

			if (target != null)
			{
				Color finalColor = new Color(color.r, color.g, color.b, Mathf.SmoothStep(alpha, color.a, t));
				target.material.color = finalColor;
			}

			if (callback != null)
				callback();
		}


		/// <summary>
		/// Fades the specified image to the target opacity and duration.
		/// </summary>
		/// <param name="target">Target.</param>
		/// <param name="opacity">Opacity.</param>
		/// <param name="duration">Duration.</param>
		public static IEnumerator FadeText(Text target, float duration, Color color)
		{
			if (target == null)
				yield break;

			float alpha = target.color.a;

			for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / duration)
			{
				if (target == null)
					yield break;
				Color newColor = new Color(color.r, color.g, color.b, Mathf.SmoothStep(alpha, color.a, t));
				target.color = newColor;
				yield return null;
			}
			target.color = color;
		}


		/// <summary>
		/// Execute an action on next frame..
		/// </summary>
		/// <param name="callback">Action to perform.</param>
		/// <returns></returns>
		public static IEnumerator OnNextFrame(Action callback)
		{
			yield return 0;
			callback();
		}


		/// <summary>
		/// Start a coroutine from global coroutine GameObject instance.
		/// Use when a Coroutine's executions shouldn't be tied to a specific object.
		/// </summary>
		/// <param name="routine">Coroutine to run.</param>
		/// <returns>Coroutine</returns>
		public static Coroutine StartGlobal(IEnumerator routine)
		{
			return Instance.StartCoroutine(routine);
		}
	}
}