namespace Assets.PrimordialOoze.Scripts.Extensions.Vectors
{
	using UnityEngine;


	public static class VectorExtensions
	{
		public static Vector2 AdjustX(this Vector2 vector, float value)
		{
			return new Vector2(vector.x + value, vector.y);
		}


		public static Vector3 AdjustX(this Vector3 vector, float value)
		{
			return new Vector3(vector.x + value, vector.y, vector.z);
		}


		public static Vector2 AdjustY(this Vector2 vector, float value)
		{
			return new Vector2(vector.x, vector.y + value);
		}


		public static Vector3 AdjustY(this Vector3 vector, float value)
		{
			return new Vector3(vector.x, vector.y + value, vector.z);
		}


		public static Vector3 AdjustZ(this Vector3 vector, float value)
		{
			return new Vector3(vector.x, vector.y, vector.z + value);
		}


		public static Vector2 SetX(this Vector2 vector, float value)
		{
			return new Vector2(value, vector.y);
		}


		public static Vector3 SetX(this Vector3 vector, float value)
		{
			return new Vector3(value, vector.y, vector.z);
		}


		public static Vector2 SetY(this Vector2 vector, float value)
		{
			return new Vector2(vector.x, value);
		}


		public static Vector3 SetY(this Vector3 vector, float value)
		{
			return new Vector3(vector.x, value, vector.z);
		}


		public static Vector3 SetZ(this Vector3 vector, float value)
		{
			return new Vector3(vector.x, vector.y, value);
		}
	}
}