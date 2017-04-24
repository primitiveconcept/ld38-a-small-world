namespace PrimordialOoze
{
	using System;
	using UnityEngine;


	public class MapCell : MonoBehaviour,
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

		public event Action<IDamageable> Damaged;
		public event Action Killed;


		public enum Type
		{
			Empty = 0,
			Wall = 1,
			DestroyableWall = 2,
			ExitNode = 3
		}


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
			amount = this.DeductHealth(amount);
			if (amount > 0)
			{
				if (this.Damaged != null)
					this.Damaged(this);

				if (this.CurrentHealth == 0
					&& this.Killed != null)
				{
					Debug.Log("Killed.");
					this.Killed();
				}
			}

			return amount;
		}


		public void Update()
		{
			this.CountdownInvulnerabilityTimeLeft();
		}
	}
}