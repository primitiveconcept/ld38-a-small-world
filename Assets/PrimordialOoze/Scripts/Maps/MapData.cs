namespace PrimordialOoze
{
	using System;
	using System.Collections.Generic;
	using UnityEngine;
	using Random = UnityEngine.Random;


	public class MapData
	{
		private IList<MicrobeData> microbes;
		private MicrobeTrait[] traits;


		#region Constructors
		public MapData(MicrobeData microbeData)
		{
			PopulateNewMicrobes(microbeData);
			PopulateNewTraits(microbeData);
		}
		#endregion


		#region Properties
		public IList<MicrobeData> Microbes
		{
			get { return this.microbes; }
			set { this.microbes = value; }
		}


		public MicrobeTrait[] Traits
		{
			get { return this.traits; }
			set { this.traits = value; }
		}
		#endregion


		#region Helper Methods
		private MicrobeTrait CreateRandomTrait()
		{
			int randomType = Random.Range(0, 5);
			int randomValue = Random.Range(1, 6);
			MicrobeTrait newTrait = null;
			switch (randomType)
			{
				case 0:
					newTrait = new MaxHealthTrait();
					break;
				case 1:
					newTrait = new SightTrait();
					break;
				case 2:
					newTrait = new SpeedTrait();
					break;
				case 3:
					newTrait = new StrengthTrait();
					break;
				case 4:
					newTrait = new DefenseTrait();
					break;
			}

			newTrait.Value = randomValue;

			return newTrait;
		}


		private void PopulateNewMicrobes(MicrobeData parent)
		{
			Debug.Log("Created new microbe child data.");
			int numberOfMicrobes = (int)Math.Floor(
				parent.MaxHealth
				/ (Microbe.HealthScalingFactor / 3));
			if (this.microbes != null)
				this.microbes.Clear();
			else
				this.microbes = new List<MicrobeData>();
			for (int i = 0; i < numberOfMicrobes; i++)
			{
				this.microbes.Add(parent.Clone());
				this.microbes[i].ParentMicrobeData = parent;
				this.microbes[i].MaxHealth /= 2;
				this.microbes[i].CurrentHealth = this.microbes[i].MaxHealth;
			}
		}


		private void PopulateNewTraits(MicrobeData parent)
		{
			int numberOfTraits = (int)Math.Ceiling(
				parent.MaxHealth
				/ (Microbe.HealthScalingFactor / 3));
			this.traits = new MicrobeTrait[numberOfTraits];
			for (int i = 0; i < numberOfTraits; i++)
			{
				this.traits[i] = CreateRandomTrait();
				this.traits[i].MicrobeData = parent;
			}
		}
		#endregion
	}
}