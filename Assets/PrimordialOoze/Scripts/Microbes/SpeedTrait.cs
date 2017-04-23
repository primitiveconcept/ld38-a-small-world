namespace PrimordialOoze
{
	using System;


	[Serializable]
	public class SpeedTrait : MicrobeTrait
	{
		#region Properties
		public override TraitType Type
		{
			get { return TraitType.Speed; }
		}
		#endregion


		public override void Activate()
		{
			this.MicrobeData.Acceleration += this.Value;
			this.MicrobeData.MaxSpeed += this.Value;
		}


		public override void Deactivate()
		{
			this.MicrobeData.Acceleration -= this.Value;
			this.MicrobeData.MaxSpeed -= this.Value;
		}
	}
}