namespace PrimordialOoze
{
	using System;


	[Serializable]
	public class SightTrait : MicrobeTrait
	{
		#region Properties
		public override TraitType Type
		{
			get { return TraitType.Sight; }
		}
		#endregion


		public override void Activate()
		{
			Game.ShowHintText("Coccus Sight Range increased!", 3f);
			IncreaseMicrobeStats(this.MicrobeData);
		}


		public override void Deactivate()
		{
			Game.ShowHintText("Coccus Sight Range decreased.", 3f);
			DecreaseMicrobeStats(this.MicrobeData);
		}


		#region Helper Methods
		private void DecreaseMicrobeStats(MicrobeData microbeData)
		{
			microbeData.SightDistance -= this.Value;
			if (microbeData.ParentMicrobeData != null)
				DecreaseMicrobeStats(microbeData.ParentMicrobeData);
		}


		private void IncreaseMicrobeStats(MicrobeData microbeData)
		{
			microbeData.SightDistance += this.Value;
			if (microbeData.ParentMicrobeData != null)
				IncreaseMicrobeStats(microbeData.ParentMicrobeData);
		}
		#endregion
	}
}