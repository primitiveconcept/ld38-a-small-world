namespace PrimordialOoze
{
	using UnityEngine;

	public class ExitNode : MonoBehaviour
	{
		public void OnTriggerEnter2D(Collider2D other)
		{
			if (other.GetComponent<PlayerMicrobeInput>() == null)
				return;

			if (Game.MicrobeMap.CurrentMicrobe == null)
			{
				Debug.Log("ExitNode triggered outside of MicrobeMap!");
				return;
			}

			Game.MicrobeMap.ExitCurrentMicrobe();
		}
	}
}
