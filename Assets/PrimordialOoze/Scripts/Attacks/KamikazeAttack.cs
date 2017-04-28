namespace PrimordialOoze
{
	using PrimordialOoze.Extensions.Coroutines;
	using UnityEngine;
	using UnityEngine.SceneManagement;


	public class KamikazeAttack : SecondaryAttack
	{
		[SerializeField]
		private GameObject burstEffect;

		public override void Attack(float x, float y)
		{
			if (GetComponent<PlayerMicrobeInput>() == null)
				return;

			Microbe player = Instantiate(Game.PlayerPrefab, this.transform.parent);
			player.Initialize();
			player.transform.position = this.transform.position;
			Camera.main.GetComponent<CameraController>().Target = player.transform;
			
			Game.SetSecondaryAttackText("Inject");
			Game.ShowHintText("Health restored.", 3f);

			this.Microbe.Input.Locked = true;
			if (this.burstEffect != null)
			{
				Instantiate(
					this.burstEffect, 
					this.transform.position, 
					this.transform.rotation);
			}
			this.Microbe.Animator.Play(Microbe.DeathAnimation);
			this.WaitForSeconds(
				0.5f,
				() =>
					{
						MicrobeData microbeParent = Game.MicrobeMap.CurrentMicrobe;
						if (microbeParent != null
							&& microbeParent.Map.Microbes.Contains(this.Microbe.Data))
						{
							microbeParent.Map.Microbes.Remove(this.Microbe.Data);
						}
						else if (Game.OverworldMicrobes.Contains(this.Microbe))
						{
							Game.OverworldMicrobes.Remove(this.Microbe);
						}
						Destroy(this.gameObject);


						if (Game.OverworldMicrobes.Count == 0)
						{
							SceneManager.LoadScene(3);
						}

						if (Game.OverworldMicrobes.Count == 1
							&& Game.OverworldMicrobes[0].Defense > Game.PlayerMicrobe.Strength
							&& Game.OverworldMicrobes[0].CurrentHealth > 0)
						{
							// Lost, because final microbe is undefeatable.
							SceneManager.LoadScene(4);
						}
					});
		}
	}
}