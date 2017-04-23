namespace PrimordialOoze
{
	using System;


	[Serializable]
	public class SightTrait : MicrobeTrait
	{
		public override TraitType Type
		{
			get { return TraitType.Sight; }
		}

		public override void Activate()
		{
			this.MicrobeData.SightDistance += this.Value;
		}


		public override void Deactivate()
		{
			this.MicrobeData.SightDistance -= this.Value;
		}
	}
}
