namespace PrimordialOoze
{
	using System;
	using System.Collections;
	using PrimordialOoze.Extensions.Camera;
	using PrimordialOoze.Extensions.Coroutines;
	using UnityEngine;


	public class InjectionAttack : SecondaryAttack
	{
		[SerializeField]
		private GameObject injectionField;

		[SerializeField]
		private GameObject injectionEffect;


		public override void Attack(float x, float y)
		{
			if (this.Microbe.IsAttacking
				|| this.Microbe.IsCoolingDown
				|| this.Microbe.IsAttacking
				|| this.injectionField == null)
			{
				return;
			}

			this.Microbe.RotateAwayFrom(x, y);
			this.Microbe.IsAttacking = true;
			this.Microbe.InvulnerabilityTimeLeft = 0.5f;
			this.Microbe.Animator.Play(Microbe.AttackAnimation);
			this.Microbe.GamePhysics.SetMovement(
				new Vector2(x, y).normalized
				* ((this.Microbe.AttackSpeed + this.Microbe.MaxSpeed) / 2));
			this.injectionField.SetActive(true);
			StartCoroutine(WaitToFinishInjection());
		}


		public override void Awake()
		{
			base.Awake();

			if (this.injectionField != null)
				this.injectionField.SetActive(false);
		}


		public void OnInjectionFailure()
		{
			this.Microbe.Animator.Play(Microbe.IdleAnimation);
		}


		public void OnInjectionSuccess(IInjectable injectable)
		{
			this.Microbe.InvulnerabilityTimeLeft = 1f;
			this.Microbe.Animator.Play(Microbe.InjectAnimation);
			this.WaitForSeconds(
				0.5f,
				() =>
					{
						Camera.main.Zoom(this.transform.lossyScale.x, 0.2f,
							() =>
								{
									if (this.injectionEffect != null)
									{
										Instantiate(
											this.injectionEffect,
											this.transform.position,
											this.transform.rotation);
									}
									injectable.CompleteInjection(this.Microbe);
									this.Microbe.Animator.Play(Microbe.IdleAnimation);
								});
					});
		}


		#region Helper Methods
		private IEnumerator WaitToFinishInjection()
		{
			this.Microbe.IsMoving = true;
			yield return new WaitForSeconds(this.Microbe.AttackDuration);

			this.Microbe.Animator.Play(Microbe.IdleAnimation);
			this.Microbe.IsAttacking = false;
			this.Microbe.IsCoolingDown = true;
			this.Microbe.GamePhysics.SetMovement(Vector2.zero);
			this.injectionField.SetActive(false);
			StartCoroutine(this.Microbe.WaitForCooldownEnd());
		}
		#endregion
	}
}