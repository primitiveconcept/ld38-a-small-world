namespace PrimordialOoze
{
	using System;
	using UnityEngine;


	/// <summary>
	/// Causes GameObject to do damage upon contact.
	/// </summary>
	[RequireComponent(typeof(Collider2D))]
	[Serializable]
	public class ContactDamage : MonoBehaviour
	{
		[SerializeField]
		private LayerMask collisionMask;

		[SerializeField]
		private int damage;

		[SerializeField]
		private GameObject damageEffect;

		[SerializeField]
		private GameObject owner;


		#region Properties
		public LayerMask CollisionMask
		{
			get { return this.collisionMask; }
			set { this.collisionMask = value; }
		}


		public int Damage
		{
			get { return this.damage; }
			set { this.damage = value; }
		}


		public GameObject DamageEffect
		{
			get { return this.damageEffect; }
			set { this.damageEffect = value; }
		}


		public GameObject Owner
		{
			get { return this.owner; }
			set { this.owner = value; }
		}
		#endregion


		public void Awake()
		{
			if (this.owner == null)
				this.owner = this.gameObject;
		}


		public void DoDamage(Collider2D target)
		{
			IDamageable damageable = target.GetComponent<IDamageable>();

			if (target == null
				|| damageable == null)
			{
				return;
			}

			if ((this.collisionMask.value & (1 << target.gameObject.layer)) == 0)
				return;

			if (target.gameObject == this.owner)
				return;

			// Deal damage.
			damageable.TakeDamage(this.damage);
			if (this.owner != null)
				this.gameObject.SetActive(false);

			// Instantiate effect if there is one.
			if (this.damageEffect != null)
			{
				Instantiate(
					this.damageEffect,
					target.transform.position,
					target.transform.rotation);
			}
		}


		/// <summary>
		/// Do damage upon touching solid collider.
		/// </summary>
		/// <param name="collision">Collision details.</param>
		public void OnCollisionStay2D(Collision2D collision)
		{
			DoDamage(collision.collider);
		}


		public void OnEnable()
		{
			ForceCollisionCheck();
		}


		/// <summary>
		/// Triggered when something collides with the melee attack
		/// </summary>
		/// <param name="collider">Collider.</param>
		public void OnTriggerEnter2D(Collider2D collider)
		{
			DoDamage(collider);
		}


		/// <summary>
		/// Triggered when something collides with the melee attack
		/// </summary>
		/// <param name="collider">Collider.</param>
		public void OnTriggerStay2D(Collider2D collider)
		{
			DoDamage(collider);
		}


		#region Helper Methods
		private void ForceCollisionCheck()
		{
			this.transform.Translate(new Vector2(0.01f, 0.01f));
			this.transform.Translate(new Vector2(-0.01f, -0.01f));
		}
		#endregion
	}
}