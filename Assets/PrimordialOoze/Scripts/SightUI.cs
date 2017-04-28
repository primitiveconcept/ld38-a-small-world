namespace PrimordialOoze
{
	using UnityEngine;
	using UnityEngine.UI;


	public class SightUI : MonoBehaviour
	{
		private RectTransform _rectTransform;
		private Image image;


		#region Properties
		private RectTransform RectTransform
		{
			get
			{
				if (this._rectTransform == null)
					this._rectTransform = GetComponent<RectTransform>();
				return this._rectTransform;
			}
		}
		#endregion


		public void Awake()
		{
			this.image = GetComponent<Image>();
		}


		public void UpdateSight(float sightRadius)
		{
			float recede = sightRadius * 0.5f;
			this.RectTransform.anchorMin = new Vector2(-recede, -recede);
			this.RectTransform.anchorMax = new Vector2(1 + recede, 1 + recede);
		}
	}
}