namespace PrimordialOoze
{
	using UnityEngine;


	public class EnemyMicrobeInput : MicrobeInput
	{
		[SerializeField]
		private float attackInterval = 1f;

		private new Collider2D collider;

		private float nextAttack;


		public override void Update()
		{
			base.Update();

			if (this.nextAttack > 0)
				this.nextAttack -= Time.deltaTime;
		}

		public override void Awake()
		{
			base.Awake();
			this.collider = GetComponent<Collider2D>();
		}


		public override void ProcessDirectionalInput()
		{
			PlayerMicrobeInput player = FindObjectOfType<PlayerMicrobeInput>();

			Vector3 playerDirection =
				(player.transform.position
				- this.transform.position)
				.normalized;

			RaycastHit2D[] hits = new RaycastHit2D[1];

			int targetsHit = collider.Raycast(
				playerDirection,
				hits,
				this.Microbe.SightDistance * 5);

			if (targetsHit > 0
				&& hits[0].transform == player.transform)
			{
				Debug.DrawRay(
					this.transform.position,
					playerDirection.normalized * this.Microbe.SightDistance * 5,
					Color.red);
				this.Microbe.Move(playerDirection.normalized);
			}
			else
			{
				Debug.DrawRay(
					this.transform.position,
					playerDirection * this.Microbe.SightDistance * 5,
					Color.yellow);
			}
		}


		public override void ProcessAttackInput()
		{
			if (this.nextAttack > 0)
				return;

			PlayerMicrobeInput player = FindObjectOfType<PlayerMicrobeInput>();

			Vector3 playerDirection =
				(player.transform.position
				- this.transform.position)
				.normalized;

			RaycastHit2D[] hits = new RaycastHit2D[1];

			int targetsHit = collider.Raycast(
				playerDirection,
				hits,
				this.Microbe.SightDistance * 2);

			if (targetsHit > 0
				&& hits[0].transform == player.transform)
			{
				// 50% of the time, bork the attack.
				if (UnityEngine.Random.Range(0, 2) == 0)
				{
					this.nextAttack = this.attackInterval;
					return;
				}

				Debug.DrawRay(
					this.transform.position,
					playerDirection.normalized * this.Microbe.SightDistance * 2,
					Color.green);

				var attack = this.PrimaryAttack;
				if (attack != null)
				{
					attack.Attack(
						playerDirection.normalized.x,
						playerDirection.normalized.y);
					this.nextAttack = this.attackInterval;
				}
					
			}
		}
	}
}