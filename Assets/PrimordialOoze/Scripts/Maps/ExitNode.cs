namespace PrimordialOoze
{
	using PrimordialOoze.Extensions.Camera;
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

			Microbe playerMicrobe = Game.PlayerMicrobe;
			Microbe exitedMicrobe = Game.MicrobeMap.ExitCurrentMicrobe();

			if (exitedMicrobe != null)
			{
				exitedMicrobe.CheckImmobilized();
				Game.Player.transform.position = exitedMicrobe.transform.position;
				playerMicrobe.transform.localScale = playerMicrobe.GetScaleForMaxHealth();
				Camera.main.Zoom(
					playerMicrobe.GetOrthographicCamSizeForScale(),
					0.2f,
					() =>
						{
							Game.PlayerMicrobe.UpdateCameraScale();
						});
			}
			else
			{
				Debug.LogWarning("Exited microbe null for some reason.");
				Game.Player.transform.position = Vector2.zero;
				Game.PlayerMicrobe.UpdateCameraScale();
			}
		}
	}
}