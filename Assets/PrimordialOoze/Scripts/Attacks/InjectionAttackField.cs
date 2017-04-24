namespace PrimordialOoze
{
	using System;
	using UnityEngine;


	/// <summary>
	/// Causes GameObject to do damage upon contact.
	/// </summary>
	[RequireComponent(typeof(Collider2D))]
	[Serializable]
	public class InjectionAttackField : MonoBehaviour
	{
		[SerializeField]
		private LayerMask collisionMask;

		[SerializeField]
		private GameObject damageEffect;

		[SerializeField]
		private InjectionAttack owner;

		private Coroutine injectionCoroutine = null;


		public void DoDamage(Collider2D target)
		{
			Microbe targetMicrobe = target.GetComponent<Microbe>();

			if (target == null
				|| targetMicrobe == null)
			{
				this.owner.OnInjectionFailure();
				return;
			}

			if ((this.collisionMask.value & (1 << target.gameObject.layer)) == 0)
			{
				this.owner.OnInjectionFailure();
				return;
			}

			if (target.gameObject == this.owner)
			{
				this.owner.OnInjectionFailure();
				return;
			}
			
			GamePhysics ownerPhysics = this.owner.GetComponent<GamePhysics>();
			GamePhysics targetPhysics = target.GetComponent<GamePhysics>();
			ownerPhysics.SetMovement(Vector2.zero);
			targetPhysics.SetMovement(Vector2.zero);

			if (targetMicrobe.GetCurrentHealthPercent() <= 0.5f)
			{
				this.owner.OnInjectionSuccess(targetMicrobe);
			}
			else
			{
				this.owner.OnInjectionFailure();
			}

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