namespace PrimordialOoze
{
	using System;
	using UnityEngine;


	[Serializable]
	public abstract class MicrobeTrait
	{
		[SerializeField]
		private int value;

		private bool activated = false;
		private MicrobeData microbeData;


		public enum TraitType
		{
			MaxHealth = 0,
			Sight = 1,
			Speed = 2,
			Strength = 3,
			Defense = 4
		}


		#region Properties
		public bool Activated
		{
			get { return this.activated; }
		}


		public MicrobeData MicrobeData
		{
			get { return this.microbeData; }
			set { this.microbeData = value; }
		}


		public abstract TraitType Type { get; }


		public int Value
		{
			get { return this.value; }
			set { this.value = value; }
		}
		#endregion


		public abstract void Activate();
		public abstract void Deactivate();


		public void Toggle()
		{
			if (this.activated)
			{
				this.activated = false;
				Deactivate();
			}
			else
			{
				this.activated = true;
				Activate();
			}
		}
	}
}