namespace PrimordialOoze.Extensions.Colors
{
	using UnityEngine;


	public static class ColorExtensions
	{
		public static Color SetAlpha(this Color color, float value)
		{
			return new Color(color.r, color.g, color.b, value);
		}
	}
}
