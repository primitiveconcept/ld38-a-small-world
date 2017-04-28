namespace PrimordialOoze
{
	using UnityEngine;


	public class ScrollingBackground : MonoBehaviour
	{
		public float HorizontalSpeed = 0.5f;
		public float VerticalSpeed = 0;

		private Renderer renderer;


		public void Awake()
		{
			this.renderer = GetComponent<Renderer>();
		}


		public void Update()
		{
			Vector2 offset = new Vector2(
				Time.time * this.HorizontalSpeed,
				Time.time * this.VerticalSpeed);
			this.renderer.material.mainTextureOffset = offset;
		}
	}
}