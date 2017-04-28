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
			Game.ShowHintText("Coccus Attack Strength increased!", 3f);
			IncreaseMicrobeStats(this.MicrobeData);
		}


		public override void Deactivate()
		{
			Game.ShowHintText("Coccus Attack Strength decreased.", 3f);
			DecreaseMicrobeStats(this.MicrobeData);
		}


		#region Helper Methods
		private void DecreaseMicrobeStats(MicrobeData microbeData)
		{
			microbeData.Strength -= this.Value;
			if (microbeData.ParentMicrobeData != null)
				DecreaseMicrobeStats(microbeData.ParentMicrobeData);
		}


		private void IncreaseMicrobeStats(MicrobeData microbeData)
		{
			microbeData.Strength += this.Value;
			if (microbeData.ParentMicrobeData != null)
				IncreaseMicrobeStats(microbeData.ParentMicrobeData);
		}
		#endregion
	}
}