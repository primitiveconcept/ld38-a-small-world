namespace PrimordialOoze
{
	using PrimordialOoze.Extensions.Coroutines;
	using UnityEngine;


	public class EjectPlayerOnDeath : OnDeathAction
	{
		public override void OnKilled()
		{
			if (Game.MicrobeMap.CurrentMicrobe == null)
			{
				Debug.Log("MapNucleus destroyed outside of MicrobeMap!");
				return;
			}

			Microbe exitedMicrobe = Game.MicrobeMap.ExitCurrentMicrobe();

			if (exitedMicrobe != null)
			{
				exitedMicrobe.Input.Locked = true;
				exitedMicrobe.Animator.Play(Microbe.DeathAnimation);
				exitedMicrobe.WaitForSeconds(
					0.5f,
					() =>
						{
							MicrobeData microbeParent = Game.MicrobeMap.CurrentMicrobe;
							if (microbeParent != null
								&& microbeParent.Map.Microbes.Contains(exitedMicrobe.Data))
							{
								microbeParent.Map.Microbes.Remove(exitedMicrobe.Data);
							}
							Destroy(exitedMicrobe.gameObject);
						});
			}
			else
			{
				Debug.Log("Microbe was null for some reason.");
			}
		}
	}
}