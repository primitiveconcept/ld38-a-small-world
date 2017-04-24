namespace PrimordialOoze
{
	using PrimordialOoze.Extensions.Coroutines;


	public class EjectPlayerOnDeath : OnDeathAction
	{
		private Microbe microbe;

		public override void Awake()
		{
			base.Awake();

			this.microbe = GetComponent<Microbe>();
		}

		public override void OnKilled()
		{
			this.Damageable.Killed -= OnKilled;

			if (GetComponent<PlayerMicrobeInput>() != null)
				Instantiate(Game.PlayerPrefab);

			this.microbe.Animator.Play(Microbe.DeathAnimation);
			this.WaitForSeconds(0.5f,
				() =>
					{
						Destroy(this.gameObject);
					});
		}
	}
}
