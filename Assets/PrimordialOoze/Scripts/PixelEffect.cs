namespace PrimordialOoze
{
	using UnityEngine;


	[ExecuteInEditMode]
	public class PixelEffect : MonoBehaviour
	{
		private static Shader _pixelShader;

		[SerializeField]
		[Range(0, 16)]
		private int pixelSize = 4;

		[SerializeField]
		private bool scalePixelSize = false;

		private SpriteRenderer spriteRenderer;
		private Material material;
		private int previousPixelSize;
		private Vector3 previousScale;
		private bool wasPreviouslyScalingPixelSize;


		#region Properties
		public static Shader PixelShader
		{
			get
			{
				if (_pixelShader == null)
					_pixelShader = Shader.Find("Filter2D/Unlit/Sprite Mosaic");
				return _pixelShader;
			}
		}
		#endregion


		public void AutosetMaterialProperties()
		{
			Vector2 sizeInPixels = GetPixelSize();
			this.material.SetFloat("_TexSizeX", (int)sizeInPixels.x);
			this.material.SetFloat("_TexSizeY", (int)sizeInPixels.y);

			if (this.scalePixelSize)
			{
				float newPixelSize = Mathf.Round(
					((sizeInPixels.x + sizeInPixels.y) / 200)
					* this.pixelSize);
				this.material.SetFloat("_MosaicSizeX", newPixelSize);
				this.material.SetFloat("_MosaicSizeY", newPixelSize);
			}
			else
			{
				this.material.SetFloat("_MosaicSizeX", this.pixelSize);
				this.material.SetFloat("_MosaicSizeY", this.pixelSize);
			}
		}


		public void Awake()
		{
			this.spriteRenderer = GetComponent<SpriteRenderer>();
			this.material = new Material(PixelShader);
			this.material.name = "RuntimePixelEffect";
			this.spriteRenderer.material = this.material;
			this.previousPixelSize = this.pixelSize;
			this.previousScale = this.transform.root.lossyScale;
			this.wasPreviouslyScalingPixelSize = this.scalePixelSize;
			AutosetMaterialProperties();
		}


		public void Update()
		{
			if (this.transform.lossyScale != this.previousScale
				|| this.pixelSize != this.previousPixelSize
				|| this.scalePixelSize != this.wasPreviouslyScalingPixelSize)
			{
				this.previousScale = this.transform.lossyScale;
				this.previousPixelSize = this.pixelSize;
				this.wasPreviouslyScalingPixelSize = this.scalePixelSize;
				AutosetMaterialProperties();
			}
		}


		#region Helper Methods
		private Vector2 GetPixelSize()
		{
			//get world space size (this version operates on the bounds of the object, so expands when rotating)
			//Vector3 world_size = GetComponent<SpriteRenderer>().bounds.size;

			//get world space size (this version handles rotating correctly)
			Vector2 spriteSize = this.spriteRenderer.sprite.rect.size;
			Vector2 localSpriteSize = spriteSize / this.spriteRenderer.sprite.pixelsPerUnit;
			Vector3 worldSize = localSpriteSize;
			worldSize.x *= transform.lossyScale.x;
			worldSize.y *= transform.lossyScale.y;

			//convert to screen space size
			Vector3 screenSize = 0.5f * worldSize / Camera.main.orthographicSize;
			screenSize.y *= Camera.main.aspect;

			//size in pixels
			Vector3 pixelSize = new Vector3(screenSize.x * Camera.main.pixelWidth, screenSize.y * Camera.main.pixelHeight, 0)
								* 0.5f;

			return pixelSize;
		}
		#endregion
	}
}