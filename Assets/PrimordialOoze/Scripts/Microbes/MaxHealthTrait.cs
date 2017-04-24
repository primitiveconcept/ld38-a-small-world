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
			IncreaseMicrobeStats(this.MicrobeData);
			ResizeMap();
		}


		public override void Deactivate()
		{
			DecreaseMicrobeStats(this.MicrobeData);
			ResizeMap();
		}


		#region Helper Methods
		private void DecreaseMicrobeStats(MicrobeData microbeData)
		{
			microbeData.MaxHealth -= (this.Value * 5);
			if (microbeData.ParentMicrobeData != null)
				DecreaseMicrobeStats(microbeData.ParentMicrobeData);
		}


		private void IncreaseMicrobeStats(MicrobeData microbeData)
		{
			microbeData.MaxHealth += (this.Value * 5);
			if (microbeData.ParentMicrobeData != null)
				IncreaseMicrobeStats(microbeData.ParentMicrobeData);
		}


		private void ResizeMap()
		{
			Game.MicrobeMap.ClearPerimeters();
			Game.MicrobeMap.RegeneratePerimeter();
		}
		#endregion
	}
}