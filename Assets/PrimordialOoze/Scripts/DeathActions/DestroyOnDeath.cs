namespace PrimordialOoze
{
	using PrimordialOoze.Extensions.Coroutines;


	public class DestroyOnDeath : OnDeathAction
	{
		public override void OnKilled()
		{
			this.Damageable.Killed -= OnKilled;

			// Wait for next frame so any other Killed events can fire.
			this.WaitForNextFrame(
				() =>
					{
						Destroy(this.gameObject);
					});
		}
	}
}
