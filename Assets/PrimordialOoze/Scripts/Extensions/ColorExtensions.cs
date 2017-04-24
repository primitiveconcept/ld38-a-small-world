namespace PrimordialOoze.Extensions.Colors
{
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
			float flickerSpeed = 0.025f)
		{
			for (int i = 0; i < 5; i++)
			{
				spriteRenderer.color = originalColor;
				yield return new WaitForSeconds(flickerSpeed);
				spriteRenderer.color = flickerColor;
				yield return new WaitForSeconds(flickerSpeed);
			}

			spriteRenderer.color = originalColor;
		}
	}
}
