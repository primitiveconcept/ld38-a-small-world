namespace PrimordialOoze.Extensions.Colors
{
	using System;
	using System.Collections;
	using UnityEngine;


	public static class ColorExtensions
	{
		public static Color SetAlpha(this Color color, float value)
		{
			return new Color(color.r, color.g, color.b, value);
		}


		public static IEnumerator Flicker(
			this SpriteRenderer spriteRenderer,
			Color flickerColor,
			Color originalColor,
			Action callback,
			float flickerDuration = 0.125f,
			float flickerSpeed = 0.025f)
		{
			int iterations = (int)(flickerDuration / flickerSpeed);
			for (int i = 0; i < iterations; i++)
			{
				spriteRenderer.color = originalColor;
				yield return new WaitForSeconds(flickerSpeed);
				spriteRenderer.color = flickerColor;
				yield return new WaitForSeconds(flickerSpeed);
			}

			spriteRenderer.color = originalColor;
			if (callback != null)
				callback();
		}
	}
}
