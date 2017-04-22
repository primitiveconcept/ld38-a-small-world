namespace PrimordialOoze
{
	using UnityEngine;


	public class EnemyMicrobeInput : MicrobeInput
	{
		private Collider2D collider;


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
				this.Microbe.SightDistance);

			if (targetsHit > 0
				&& hits[0].transform == player.transform)
			{
				Debug.DrawRay(
					this.transform.position,
					playerDirection.normalized * this.Microbe.SightDistance,
					Color.red);
				this.Microbe.Move(playerDirection.normalized);
			}
			else
			{
				Debug.DrawRay(
					this.transform.position,
					playerDirection * this.Microbe.SightDistance,
					Color.yellow);
			}
		}
	}
}