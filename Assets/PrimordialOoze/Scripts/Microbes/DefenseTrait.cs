namespace PrimordialOoze
{
	public class DefenseTrait : MicrobeTrait
	{
		#region Properties
		public override TraitType Type
		{
			get { return TraitType.Defense; }
		}
		#endregion


		public override void Activate()
		{
			Game.ShowHintText("Coccus Defense increased!", 3f);
			IncreaseMicrobeStats(this.MicrobeData);
		}


		public override void Deactivate()
		{
			Game.ShowHintText("Coccus Defense decreased.", 3f);
			DecreaseMicrobeStats(this.MicrobeData);
		}


		#region Helper Methods
		private void DecreaseMicrobeStats(MicrobeData microbeData)
		{
			microbeData.Defense -= this.Value;
			if (microbeData.ParentMicrobeData != null)
				DecreaseMicrobeStats(microbeData.ParentMicrobeData);
		}


		private void IncreaseMicrobeStats(MicrobeData microbeData)
		{
			microbeData.Defense += this.Value;
			if (microbeData.ParentMicrobeData != null)
				IncreaseMicrobeStats(microbeData.ParentMicrobeData);
		}
		#endregion
	}
}
