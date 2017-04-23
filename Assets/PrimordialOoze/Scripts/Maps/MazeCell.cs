namespace PrimordialOoze
{
	using UnityEngine;


	public class MazeCell : MonoBehaviour,
							IDamageable
	{
		[SerializeField]
		private int currentHealth;

		[SerializeField]
		private int maxHealth = 10;

		[SerializeField]
		private bool isInvulnerable;

		[SerializeField]
		private float invulnerabilityDuration;

		[SerializeField]
		private float invulnerabilityTimeLeft;


		#region Properties
		public int CurrentHealth
		{
			get { return this.currentHealth; }
			set { this.currentHealth = value; }
		}


		public float InvulnerabilityDuration
		{
			get { return this.invulnerabilityDuration; }
			private set { this.invulnerabilityDuration = value; }
		}


		public float InvulnerabilityTimeLeft
		{
			get { return this.invulnerabilityTimeLeft; }
			set { this.invulnerabilityTimeLeft = value; }
		}


		public bool IsInvulnerable
		{
			get { return this.isInvulnerable; }
			private set { this.isInvulnerable = value; }
		}


		public int MaxHealth
		{
			get { return this.maxHealth; }
			private set { this.maxHealth = value; }
		}
		#endregion


		public int TakeDamage(int amount)
		{
			amount = IDamageableExtensions.TakeDamage(this, amount);

			if (this.CurrentHealth == 0)
				Destroy(this.gameObject);

			return amount;
		}
	}
}