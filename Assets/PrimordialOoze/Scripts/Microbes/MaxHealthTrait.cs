namespace PrimordialOoze
{
	using System;


	[Serializable]
	public class MaxHealthTrait : MicrobeTrait
	{
		public override void Activate()
		{
			// TODO: Change map size.
			this.MicrobeData.MaxHealth += this.Value;
		}


		public override void Deactivate()
		{
			// TODO: Change map size.
			this.MicrobeData.MaxHealth -= this.Value;
		}
	}
}