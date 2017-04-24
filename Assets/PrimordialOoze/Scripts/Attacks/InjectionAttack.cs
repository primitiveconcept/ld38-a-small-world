namespace PrimordialOoze
{
	using System;
	using System.Collections;
	using PrimordialOoze.Extensions.Coroutines;
	using UnityEngine;


	public class InjectionAttack : SecondaryAttack
	{
		[SerializeField]
		private GameObject injectionField;


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


		public void OnInjectionSuccess(Microbe targetMicrobe)
		{
			this.Microbe.Animator.Play(Microbe.InjectAnimation);
			this.WaitForSeconds(
				0.5f,
				() =>
					{
						StartCoroutine(
							ZoomCamera(
								() =>
									{
										Game.MicrobeMap.SetCurrentMicrobe(targetMicrobe.Data);
										if (targetMicrobe != null)
											Destroy(targetMicrobe.gameObject);
										this.Microbe.Animator.Play(Microbe.IdleAnimation);
									}));
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


		private IEnumerator ZoomCamera(Action callback)
		{
			while (Camera.main.orthographicSize > this.transform.localScale.x + 0.5f)
			{
				Camera.main.orthographicSize = Mathf.Lerp(
					Camera.main.orthographicSize,
					this.transform.localScale.x,
					Time.deltaTime * 15);
				yield return null;
			}

			callback();
		}
		#endregion
	}
}