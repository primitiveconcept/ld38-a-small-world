namespace PrimordialOoze
{
	using UnityEngine;
	using UnityEngine.EventSystems;


	public class Tooltip : MonoBehaviour, 
		IPointerEnterHandler, 
		IPointerExitHandler, 
		ISelectHandler, 
		IDeselectHandler
	{
		[SerializeField]
		private string text;


		public void OnDeselect(BaseEventData eventData)
		{
			StopHover();
		}


		public void OnPointerEnter(PointerEventData eventData)
		{
			StartHover(new Vector3(eventData.position.x, eventData.position.y - 18f, 0f));
		}


		public void OnPointerExit(PointerEventData eventData)
		{
			StopHover();
		}


		public void OnSelect(BaseEventData eventData)
		{
			StartHover(transform.position);
		}


		#region Helper Methods
		void StartHover(Vector3 position)
		{
			Game.ShowTooltip(text, position);
		}


		void StopHover()
		{
			Game.HideTooltip();
		}
		#endregion
	}
}