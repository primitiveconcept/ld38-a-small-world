namespace PrimordialOoze
{
	using System.Linq;
	using UnityEngine;


	public class MicrobeScaler : MonoBehaviour
	{
		public void ChangeScale(float scalarAdjustment)
		{
			Vector3 newScale = this.transform.localScale * scalarAdjustment;

			Microbe microbe = GetComponent<Microbe>();
			float largestMicrobeScale = Microbe.GetScaleForMaxHealth(
				microbe.OriginalScale,
				microbe.MaxHealth).x;
			float smallestMicrobeScale = largestMicrobeScale / 2;
			//float smallestMicrobeScale = Game.MicrobeMap.GetSmallestMicrobeScale();

			if (newScale.x < smallestMicrobeScale)
				newScale = new Vector3(smallestMicrobeScale, smallestMicrobeScale, 1);
			else if (newScale.x > largestMicrobeScale)
				newScale = new Vector3(largestMicrobeScale, largestMicrobeScale, 1);
			
			if (this.transform.localScale != newScale)
			{
				this.transform.localScale = newScale;
				microbe.UpdateCameraBasedOnScaled();
			}
		}


		#region Helper Methods
		
		#endregion
	}
}