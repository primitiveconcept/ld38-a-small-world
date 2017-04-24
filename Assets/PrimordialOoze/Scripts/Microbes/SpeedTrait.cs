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
			IncreaseMicrobeStats(this.MicrobeData);
		}


		public override void Deactivate()
		{
			DecreaseMicrobeStats(this.MicrobeData);
		}


		#region Helper Methods
		private void DecreaseMicrobeStats(MicrobeData microbeData)
		{
			microbeData.Acceleration -= this.Value;
			microbeData.MaxSpeed -= this.Value;
			if (microbeData.ParentMicrobeData != null)
				DecreaseMicrobeStats(microbeData.ParentMicrobeData);
		}


		private void IncreaseMicrobeStats(MicrobeData microbeData)
		{
			microbeData.Acceleration += this.Value;
			microbeData.MaxSpeed += this.Value;
			if (microbeData.ParentMicrobeData != null)
				IncreaseMicrobeStats(microbeData.ParentMicrobeData);
		}
		#endregion
	}
}