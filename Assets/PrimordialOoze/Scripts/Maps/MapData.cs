namespace PrimordialOoze
{
	using System;
	using System.Collections.Generic;
	using Random = UnityEngine.Random;


	public class MapData
	{
		private IList<MicrobeData> microbes;
		private MicrobeTrait[] traits;

		#region Properties
		public IList<MicrobeData> Microbes
		{
			get { return this.microbes; }
			set { this.microbes = value; }
		}
		#endregion


		public MapData(MicrobeData microbeData)
		{
			PopulateNewMicrobes(microbeData);
			PopulateNewTraits(microbeData);
		}


		private void PopulateNewMicrobes(MicrobeData parent)
		{
			int numberOfMicrobes = (int)Math.Ceiling(parent.MaxHealth / 25f);
			this.microbes = new MicrobeData[numberOfMicrobes];
			for (int i = 0; i < numberOfMicrobes; i++)
			{
				this.microbes[i] = parent.Clone();
				this.microbes[i].MaxHealth /= 2;
				this.microbes[i].CurrentHealth = this.microbes[i].MaxHealth;
			}
		}


		private void PopulateNewTraits(MicrobeData parent)
		{
			int numberOfTraits = (int)(parent.MaxHealth * 0.25f);
			this.traits = new MicrobeTrait[numberOfTraits];
			for (int i = 0; i < numberOfTraits; i++)
			{

				this.traits[i] = CreateRandomTrait();
			}
		}


		private MicrobeTrait CreateRandomTrait()
		{
			int randomType = Random.Range(0, 3);
			int randomValue = Random.Range(1, 5);
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
			}

			newTrait.Value = randomValue;

			return newTrait;
		}
	}
}