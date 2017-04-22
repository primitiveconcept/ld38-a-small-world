namespace PrimordialOoze
{
	using UnityEngine;
	using UnityEngine.UI;


	public class SightUI : MonoBehaviour
	{
		private RectTransform rectTransform;
		private Image image;


		public void Awake()
		{
			this.rectTransform = GetComponent<RectTransform>();
			this.image = GetComponent<Image>();
		}


		public void UpdateSight(float sightRadius)
		{
			float recede = sightRadius * 0.5f;
			this.rectTransform.anchorMin = new Vector2(-recede, -recede);
			this.rectTransform.anchorMax = new Vector2(1 + recede, 1 + recede);
		}
	}
}
