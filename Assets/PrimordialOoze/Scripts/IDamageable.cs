namespace PrimordialOoze
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;


	public interface IDamageable
	{
		event Action<IDamageable> Damaged;
		event Action Killed;


		#region Properties
		int CurrentHealth { get; set; }
		float InvulnerabilityDuration { get; }
		float InvulnerabilityTimeLeft { get; set; }
		bool IsInvulnerable { get; }
		int MaxHealth { get; }
		#endregion


		int TakeDamage(int amount);
	}


	public static class IDamageableExtensions
	{
		/// <summary>
		/// Reduces invulnerability time by deltaTime.
		/// </summary>
		/// <param name="damageable">Damageable to reduce invulnerability timer of.</param>
		public static void CountdownInvulnerabilityTimeLeft(this IDamageable damageable)
		{
			if (damageable.InvulnerabilityTimeLeft <= 0)
				return;

			damageable.InvulnerabilityTimeLeft -= Time.deltaTime;
			if (damageable.InvulnerabilityTimeLeft < 0)
				damageable.InvulnerabilityTimeLeft = 0;
		}


		/// <summary>
		/// Get the percent of health left for an IDamageable.
		/// </summary>
		/// <param name="damageable">Damageable to get health percent of.</param>
		/// <returns>Percent of health left, between 0 and 1.</returns>
		public static float GetCurrentHealthPercent(this IDamageable damageable)
		{
			return (float)damageable.CurrentHealth / damageable.MaxHealth;
		}


		/// <summary>
		/// Deal damage to damageable object.
		/// </summary>
		/// <param name="damageable">IDamageable taking damage</param>
		/// <param name="amount">Amount of damage.</param>
		/// <returns>Amount of damage done.</returns>
		public static int DeductHealth(this IDamageable damageable, int amount)
		{
			if (damageable.InvulnerabilityTimeLeft > 0
				|| damageable.IsInvulnerable)
			{
				return 0;
			}

			if (damageable.CurrentHealth - amount < 0)
			{
				amount = damageable.CurrentHealth;
			}
			else
			{
				damageable.InvulnerabilityTimeLeft = damageable.InvulnerabilityDuration;
			}

			damageable.CurrentHealth -= amount;
			if (damageable.CurrentHealth < 0)
				damageable.CurrentHealth = 0;

			return amount;
		}
	}
}