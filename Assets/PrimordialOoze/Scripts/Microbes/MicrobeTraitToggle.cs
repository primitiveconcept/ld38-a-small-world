namespace PrimordialOoze
{
	using UnityEngine;


	public class MicrobeTraitToggle : MonoBehaviour,
										IDamageable
	{
		[SerializeField]
		private MicrobeTrait data;
		
		#region Properties
		public int CurrentHealth { get; set; }


		public MicrobeTrait Data
		{
			get { return this.data; }
		}


		public float InvulnerabilityDuration { get; private set; }
		public float InvulnerabilityTimeLeft { get; set; }
		public bool IsInvulnerable { get; private set; }
		public int MaxHealth { get; private set; }
		#endregion


		public int TakeDamage(int amount)
		{
			Toggle();

			return 0;
		}


		public void Toggle()
		{
			this.data.Toggle();
		}
	}
}