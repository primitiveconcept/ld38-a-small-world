namespace PrimordialOoze
{
	using System;


	[Serializable]
	public class MaxHealthTrait : MicrobeTrait
	{
		#region Properties
		public override TraitType Type
		{
			get { return TraitType.MaxHealth; }
		}
		#endregion


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