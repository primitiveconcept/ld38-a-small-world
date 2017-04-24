namespace PrimordialOoze
{
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

			Destroy(this.gameObject);
		}
	}
}
