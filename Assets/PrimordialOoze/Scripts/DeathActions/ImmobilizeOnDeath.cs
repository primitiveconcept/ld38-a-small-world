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
			EnemyMicrobeInput enemyInput = GetComponent<EnemyMicrobeInput>();
			enemyInput.Locked = true;
			this.microbe.CurrentColor = Color.green;
			this.WaitForSeconds(0.5f, () => this.microbe.UpdateOpacity());

			/* TODO: Use to eject player.
						if (GetComponent<PlayerMicrobeInput>() != null)
							Instantiate(Game.PlayerPrefab);
							*/

			/* TODO: User for actual killing.
						this.microbe.Animator.Play(Microbe.DeathAnimation);
						this.WaitForSeconds(0.5f,
							() =>
								{
									Destroy(this.gameObject);
								});
								*/
		}
	}
}