namespace PrimordialOoze
{
	using System;
	using PrimordialOoze.Extensions.Colors;
	using UnityEngine;


	public class MicrobeTraitToggle : MonoBehaviour,
									IDamageable
	{
		[SerializeField]
		private MicrobeTrait data;

		[SerializeField]
		private GameObject switchOnEffect;

		[SerializeField]
		private GameObject switchOffEffect;

		private SpriteRenderer spriteRenderer;

		public event Action<IDamageable> Damaged;
		public event Action Killed;


		#region Properties
		public int CurrentHealth { get; set; }


		public MicrobeTrait Data
		{
			get { return this.data; }
			set { this.data = value; }
		}


		public float InvulnerabilityDuration
		{
			get { return 0.6f; }
		}


		public float InvulnerabilityTimeLeft { get; set; }
		public bool IsInvulnerable { get; private set; }
		public int MaxHealth { get; private set; }
		#endregion


		public void Awake()
		{
			this.spriteRenderer = GetComponent<SpriteRenderer>();
		}


		public int TakeDamage(int amount)
		{
			if (this.InvulnerabilityTimeLeft > 0
				|| this.IsInvulnerable)
			{
				return 0;
			}

			Toggle();
			this.InvulnerabilityTimeLeft = this.InvulnerabilityDuration;

			return 0;
		}


		public void Toggle()
		{
			this.data.Toggle();

			if (this.data.Activated
				&& this.switchOnEffect != null)
			{
				Instantiate(
					this.switchOnEffect,
					this.transform.position,
					this.transform.rotation);
			}
			else if (!this.data.Activated
				&& this.switchOffEffect != null)
			{
				Instantiate(
					this.switchOffEffect,
					this.transform.position,
					this.transform.rotation);
			}


			UpdateSprite();
		}


		public void Update()
		{
			this.CountdownInvulnerabilityTimeLeft();
		}


		public void UpdateSprite()
		{
			if (this.data.Activated)
			{
				this.spriteRenderer.color =
					this.spriteRenderer.color.SetAlpha(1);
			}
			else
			{
				this.spriteRenderer.color =
					this.spriteRenderer.color.SetAlpha(0.5f);
			}
		}
	}
}