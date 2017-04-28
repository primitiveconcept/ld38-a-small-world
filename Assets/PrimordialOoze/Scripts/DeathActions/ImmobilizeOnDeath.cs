namespace PrimordialOoze
{
	using PrimordialOoze.Extensions.Coroutines;
	using UnityEngine;


	public class ImmobilizeOnDeath : OnDeathAction
	{
		private Microbe microbe;


		public override void Awake()
		{
			base.Awake();

			this.microbe = GetComponent<Microbe>();
		}


		public override void OnKilled()
		{
			this.microbe.CurrentColor = Color.green;
			this.microbe.UpdateOpacity();

			EnemyMicrobeInput enemyInput = GetComponent<EnemyMicrobeInput>();
			PlayerMicrobeInput playerInput = GetComponent<PlayerMicrobeInput>();

			if (enemyInput != null)
			{
				enemyInput.Locked = true;
				Game.ShowHintText("Use an Inject attack to enter the enemy while immobilized.", 5f);
			}
			
			// Killed while player was controlling enemy.
			if (playerInput != null)
			{
				Microbe player = Instantiate(Game.PlayerPrefab, this.transform.parent);
				Destroy(playerInput);
				player.transform.position = this.transform.position;
				Camera.main.GetComponent<CameraController>().Target = player.transform;
				player.Initialize();

				Game.SetSecondaryAttackText("Inject");
				Game.ShowHintText("Ejected from imobilized cell. Try re-injecting.", 3f);
			}		
			
		}
	}
}