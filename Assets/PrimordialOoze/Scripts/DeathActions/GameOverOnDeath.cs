namespace PrimordialOoze
{
	using PrimordialOoze.Extensions.Coroutines;
	using UnityEngine;
	using UnityEngine.SceneManagement;


	public class GameOverOnDeath : OnDeathAction
	{
		[SerializeField]
		private GameObject deathEffect;

		public override void OnKilled()
		{
			Collider2D collider = GetComponent<Collider2D>();
			collider.enabled = false;
			Microbe microbe = GetComponent<Microbe>();
			microbe.Animator.Play(Microbe.DeathAnimation);
			if (this.deathEffect != null)
			{
				Instantiate(
					this.deathEffect,
					this.transform.position,
					this.transform.rotation);
			}
			this.WaitForSeconds(1.3f,
				() =>
					{
						SceneManager.LoadScene(2);
					});
			
		}
	}
}
