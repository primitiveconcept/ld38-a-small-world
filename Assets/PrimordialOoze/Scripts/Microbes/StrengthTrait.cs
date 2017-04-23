namespace PrimordialOoze
{
	using System;


	[Serializable]
	public class StrengthTrait : MicrobeTrait
	{
		#region Properties
		public override TraitType Type
		{
			get { return TraitType.Strength; }
		}
		#endregion


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