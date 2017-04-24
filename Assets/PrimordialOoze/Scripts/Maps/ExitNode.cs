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

			var exitedMicrobe = Game.MicrobeMap.ExitCurrentMicrobe();

			if (exitedMicrobe != null)
			{
				exitedMicrobe.CurrentHealth = exitedMicrobe.MaxHealth;
				exitedMicrobe.Input.Locked = false;
				exitedMicrobe.SpriteRenderer.color = exitedMicrobe.OriginalColor;
				exitedMicrobe.CurrentColor = exitedMicrobe.OriginalColor;
				Game.Player.transform.position = exitedMicrobe.transform.position;
				Game.Player.transform.localScale = Vector3.one * Game.MicrobeMap.GetSmallestMicrobeScale();
				Game.PlayerMicrobe.UpdateCameraBasedOnScaled();
			}
		}
	}
}
