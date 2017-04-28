namespace PrimordialOoze
{
	using PrimordialOoze.Extensions.Colors;
	using UnityEngine;


	public class MapNucleus : MapCell,
							IInjectable
	{
		public bool CanBeInjectedBy(Microbe injector)
		{
			return true;
		}


		public override int TakeDamage(int amount)
		{
			this.SpriteRenderer.Flicker(Color.white, SpriteRenderer.color, null);
			Game.ShowHintText("Use an Inject attack on the nucleus to take over the enemy coccus.", 5f);

			return 0;
		}


		public void CompleteInjection(Microbe injector)
		{
			if (Game.MicrobeMap.CurrentMicrobe == null)
			{
				Debug.Log("MapNucleus injected outside of MicrobeMap!");
				return;
			}

			Microbe exitedMicrobe = Game.MicrobeMap.ExitCurrentMicrobe();

			if (exitedMicrobe != null)
			{
				exitedMicrobe.CurrentHealth = exitedMicrobe.MaxHealth;
				exitedMicrobe.SpriteRenderer.color = Game.PlayerMicrobe.OriginalColor;
				exitedMicrobe.CurrentColor = Game.PlayerMicrobe.OriginalColor;
				Camera.main.GetComponent<CameraController>().Target = exitedMicrobe.transform;
				Destroy(Game.Player.gameObject);
				Destroy(exitedMicrobe.Input);
				exitedMicrobe.gameObject.AddComponent<PlayerMicrobeInput>();
				exitedMicrobe.Initialize();
				exitedMicrobe.Input.Locked = false;
				Game.SetSecondaryAttackText("Self Destruct");
				Game.ShowHintText("Right-click to burst out of the enemy, and restore health.", 5f);
			}
			else
			{
				Debug.Log("Microbe was null for some reason.");
			}
		}
	}
}