namespace PrimordialOoze
{
	using System;
	using PrimordialOoze.Extensions.Camera;
	using UnityEngine;


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
			Game.ShowHintText("Coccus Max Health and size increased!", 3f);
			IncreaseMicrobeStats(this.MicrobeData);
			ResizeMap();
		}


		public override void Deactivate()
		{
			Game.ShowHintText("Coccus Max Health and size decreased.", 3f);
			DecreaseMicrobeStats(this.MicrobeData);
			ResizeMap();
		}


		#region Helper Methods
		private void DecreaseMicrobeStats(MicrobeData microbeData)
		{
			microbeData.MaxHealth -= (this.Value * 2);
			if (microbeData.ParentMicrobeData != null)
				DecreaseMicrobeStats(microbeData.ParentMicrobeData);
		}


		private void IncreaseMicrobeStats(MicrobeData microbeData)
		{
			microbeData.MaxHealth += (this.Value * 2);
			if (microbeData.ParentMicrobeData != null)
				IncreaseMicrobeStats(microbeData.ParentMicrobeData);
		}


		private void ResizeMap()
		{
			CameraController cameraController = Camera.main.GetComponent<CameraController>();
			cameraController.FollowsPlayer = false;
			Camera.main.Vibrate(0.5f, 0.02f, 1f,
				() =>
					{
						Game.MicrobeMap.ClearNucleus();
						Game.MicrobeMap.GenerateNucleus();
						Game.MicrobeMap.ClearTraits();
						Game.MicrobeMap.GenerateTraits();
						Game.MicrobeMap.ClearPerimeters();
						Game.MicrobeMap.RegeneratePerimeter();
						cameraController.FollowsPlayer = true;
					});
		}
		#endregion
	}
}