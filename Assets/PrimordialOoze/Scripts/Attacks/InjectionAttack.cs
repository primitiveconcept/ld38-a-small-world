namespace PrimordialOoze
{
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

			this.Microbe.IsAttacking = true;
			this.Microbe.Animator.Play(Microbe.InjectAnimation);
			this.Microbe.GamePhysics.SetMovement(
				new Vector2(x, y).normalized
				* ((this.Microbe.AttackSpeed + this.Microbe.MaxSpeed) / 2));
			this.injectionField.SetActive(true);
			StartCoroutine(WaitToFinishInjection());
		}


		public void OnInjectionFailure()
		{
			this.Microbe.Animator.Play(Microbe.IdleAnimation);
		}


		public void OnInjectionSuccess(Microbe targetMicrobe)
		{
			Animator cameraAnimator = Camera.main.GetComponent<Animator>();
			cameraAnimator.Play(Microbe.CameraZoomInAnimation);
			this.WaitForSeconds(
				0.5f,
				() =>
				{
					Game.MicrobeMap.SetCurrentMicrobe(targetMicrobe.Data);
					cameraAnimator.Play(Microbe.CameraIdleAnimation);
					this.Microbe.Animator.Play(Microbe.IdleAnimation);
				});
		}

		public override void Awake()
		{
			base.Awake();

			if (this.injectionField != null)
				this.injectionField.SetActive(false);
		}


		#region Helper Methods
		private IEnumerator WaitToFinishInjection()
		{
			this.Microbe.IsMoving = true;
			yield return new WaitForSeconds(this.Microbe.AttackDuration);

			this.Microbe.IsAttacking = false;
			this.Microbe.IsCoolingDown = true;
			this.Microbe.GamePhysics.SetMovement(Vector2.zero);
			this.injectionField.SetActive(false);
			StartCoroutine(this.Microbe.WaitForCooldownEnd());
		}
		#endregion
	}
}