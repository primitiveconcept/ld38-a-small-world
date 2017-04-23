namespace PrimordialOoze
{
	using System;


	[Serializable]
	public class StrengthTrait : MicrobeTrait
	{
		public override void Activate()
		{
			this.MicrobeData.Strength += this.Value;
		}


		public override void Deactivate()
		{
			this.MicrobeData.Strength -= this.Value;
		}
	}
}
