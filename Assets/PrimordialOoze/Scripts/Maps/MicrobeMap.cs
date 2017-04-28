namespace PrimordialOoze
{
	using System;
	using System.Collections.Generic;
	using PrimordialOoze.Extensions.Camera;
	using PrimordialOoze.Extensions.Vectors;
	using UnityEngine;


	public class MicrobeMap : MonoBehaviour
	{
		public const float NucleusToHealthRatio = 10 / Microbe.HealthScalingFactor;
		public const float PerimeterToHealthRatio = 16 / Microbe.HealthScalingFactor;

		[SerializeField]
		private Microbe microbePrefab;

		[SerializeField]
		private GameObject[] mapCellPrefabs; // Must sort according to type enum.

		[SerializeField]
		private GameObject[] traitPrefabs;

		[SerializeField]
		private GameObject veinPrefab;

		private MicrobeData currentMicrobe;
		private GameObject[,] nucleusCells;
		private List<GameObject> perimeterCells;
		private List<Microbe> microbes;
		private MicrobeTraitToggle[] traits; // Must sort according to type enum.
		private GameObject veinsInstance;

		#region Properties
		public MicrobeData CurrentMicrobe
		{
			get { return this.currentMicrobe; }
		}


		public List<Microbe> Microbes
		{
			get { return this.microbes; }
		}


		private int NucleusSize
		{
			get
			{
				return (int)(this.currentMicrobe.MaxHealth
							* NucleusToHealthRatio);
			}
		}


		private int PerimeterRadius
		{
			get
			{
				return (int)(this.currentMicrobe.MaxHealth
							* PerimeterToHealthRatio);
			}
		}
		#endregion


		public void ClearMicrobes()
		{
			if (this.microbes == null)
				return;

			for (int i = 0; i < this.microbes.Count; i++)
			{
				if (this.microbes[i] != null)
					//this.microbes[i].gameObject.SetActive(false);
					Destroy(this.microbes[i].gameObject);
			}
		}


		public void ClearNucleus()
		{
			if (this.nucleusCells == null)
				return;

			foreach (GameObject cell in this.nucleusCells)
			{
				if (cell != null)
					Destroy(cell.gameObject);
			}

			if (this.veinsInstance != null)
				Destroy(this.veinsInstance.gameObject);
		}


		public void ClearPerimeters()
		{
			if (this.perimeterCells == null)
				this.perimeterCells = new List<GameObject>();

			foreach (GameObject cell in this.perimeterCells)
			{
				if (cell != null)
					Destroy(cell.gameObject);
			}
		}


		public void ClearTraits()
		{
			if (this.traits == null)
				return;

			for (int i = 0; i < this.traits.Length; i++)
			{
				if (this.traits[i] != null)
					Destroy(this.traits[i].gameObject);
			}
		}


		public void EnterMicrobe(MicrobeData microbeData)
		{
			this.currentMicrobe = microbeData;
			GenerateNewMap();
			Game.Player.transform.SetParent(this.transform);
			Vector2 offset = new Vector2(this.PerimeterRadius * 0.50f, this.PerimeterRadius * 0.50f);
			Game.Player.transform.localPosition = GetMapCellCoordinate(0, 0, offset);
			Game.Player.transform.localScale = Game.PlayerMicrobe.GetScaleForMaxHealth();
			Camera.main.backgroundColor = Game.InsideMicrobeColor;
			Camera.main.Zoom(
				Game.PlayerMicrobe.GetOrthographicCamSizeForScale(),
				0.2f);
		}


		/// <summary>
		/// Exit current microbe map.
		/// </summary>
		/// <returns>Microbe just existed.</returns>
		public Microbe ExitCurrentMicrobe()
		{
			Microbe exitedMicrobe = null;
			MicrobeData exitedMicrobeData = this.currentMicrobe;

			// Exited microbe will still exist in MicrobeMap.
			// Set map to parent, so exited microbe will be instantiated.
			if (this.currentMicrobe.ParentMicrobeData != null)
				EnterMicrobe(exitedMicrobeData.ParentMicrobeData);

			// Back to overworld.
			else
			{
				Camera.main.backgroundColor = Game.OverworldColor;
			}

			exitedMicrobe = Game.FindMicrobe(exitedMicrobeData);

			return exitedMicrobe;
		}


		public void GenerateMicrobes()
		{
			MapData internalMap = this.currentMicrobe.Map;
			int numberOfMicrobes = internalMap.Microbes.Count;
			this.microbes = new List<Microbe>();
			for (int i = 0; i < numberOfMicrobes; i++)
			{
				this.Microbes.Add(Instantiate(this.microbePrefab, this.transform));
				this.microbes[i].Data = internalMap.Microbes[i];
				this.microbes[i].transform.localPosition = GetRadialPosition(i);
				this.microbes[i].Initialize();
			}
		}


		public void GenerateNewMap()
		{
			ClearNucleus();
			GenerateNucleus();

			ClearPerimeters();
			RegeneratePerimeter();

			ClearMicrobes();
			GenerateMicrobes();

			ClearTraits();
			GenerateTraits();
		}


		public void GenerateNucleus()
		{
			Maze baseMaze = new Maze(this.NucleusSize);
			this.nucleusCells = new GameObject[this.NucleusSize, this.NucleusSize];
			Vector2 offset = new Vector2(this.PerimeterRadius * 0.75f, this.PerimeterRadius * 0.75f);

			for (int x = 0; x < this.NucleusSize; x++)
			{
				for (int y = 0; y < this.NucleusSize; y++)
				{
					if (baseMaze[x, y] != 0)
					{
						GameObject prefab = GetMapCellPrefab((MapCell.Type)baseMaze[x, y]);

						this.nucleusCells[x, y] =
							InstantiateIntoMapCell(x, y, prefab, offset);

						if (baseMaze[x, y] == 4)
						{
							this.veinsInstance = InstantiateIntoMapCell(x, y, this.veinPrefab, offset);
							this.veinsInstance.transform.localScale = new Vector3(
								this.NucleusSize * 0.15f,
								this.NucleusSize * 0.15f,
								1);
						}
					}
				}
			}
		}


		public void GeneratePerimeter(GameObject perimeterObject, int? perimeterRadius = null)
		{
			int x, y, r2;
			List<GameObject> cells = new List<GameObject>();
			int radius = this.PerimeterRadius;
			if (perimeterRadius != null)
				radius = perimeterRadius.Value;
			int center = radius;
			r2 = this.PerimeterRadius * this.PerimeterRadius;

			cells.Add(InstantiateIntoMapCell(center, center + radius, perimeterObject));
			cells.Add(InstantiateIntoMapCell(center, center - radius, perimeterObject));
			cells.Add(InstantiateIntoMapCell(center + center, center, perimeterObject));
			cells.Add(InstantiateIntoMapCell(center - center, center, perimeterObject));

			y = radius;
			x = 1;
			y = (int)(Math.Sqrt(r2 - 1) + 0.5);
			while (x < y)
			{
				cells.Add(InstantiateIntoMapCell(center + x, center + y, perimeterObject));
				cells.Add(InstantiateIntoMapCell(center + x, center - y, perimeterObject));
				cells.Add(InstantiateIntoMapCell(center - x, center + y, perimeterObject));
				cells.Add(InstantiateIntoMapCell(center - x, center - y, perimeterObject));
				cells.Add(InstantiateIntoMapCell(center + y, center + x, perimeterObject));
				cells.Add(InstantiateIntoMapCell(center + y, center - x, perimeterObject));
				cells.Add(InstantiateIntoMapCell(center - y, center + x, perimeterObject));
				cells.Add(InstantiateIntoMapCell(center - y, center - x, perimeterObject));
				x += 1;
				y = (int)(Math.Sqrt(r2 - x * x) + 0.5);
			}
			if (x == y)
			{
				cells.Add(InstantiateIntoMapCell(center + x, center + y, perimeterObject));
				cells.Add(InstantiateIntoMapCell(center + x, center - y, perimeterObject));
				cells.Add(InstantiateIntoMapCell(center - x, center + y, perimeterObject));
				cells.Add(InstantiateIntoMapCell(center - x, center - y, perimeterObject));
			}

			this.perimeterCells.AddRange(cells);
		}


		public void GenerateTraits()
		{
			MapData internalMap = this.currentMicrobe.Map;
			int numberOfTraits = internalMap.Traits.Length;
			this.traits = new MicrobeTraitToggle[numberOfTraits];
			Vector2 offset = new Vector2(this.PerimeterRadius * 0.75f, this.PerimeterRadius * 0.75f);
			for (int i = 0; i < numberOfTraits; i++)
			{
				int x = UnityEngine.Random.Range(0, this.NucleusSize);
				int y = UnityEngine.Random.Range(0, this.NucleusSize);

				MicrobeTrait data = internalMap.Traits[i];
				this.traits[i] = InstantiateIntoMapCell(
						x,
						y,
						this.traitPrefabs[(int)data.Type],
						offset)
					.GetComponent<MicrobeTraitToggle>();
				this.traits[i].Data = data;
				this.traits[i].UpdateSprite();
			}
		}


		public Vector3 GetMapCellCoordinate(int x, int y, Vector2? offset = null)
		{
			Vector3 position = new Vector3(
				x - this.NucleusSize * 0.5f + 0.5f,
				y - this.NucleusSize * 0.5f + 0.5f,
				0);
			if (offset != null)
			{
				position = position
					.AdjustX(offset.Value.x)
					.AdjustY(offset.Value.y);
			}

			return position;
		}


		public GameObject GetMapCellPrefab(MapCell.Type type)
		{
			if (type == MapCell.Type.Empty)
				return null;

			return this.mapCellPrefabs[(int)type - 1];
		}


		public Vector3 GetRadialPosition(int step)
		{
			step++;
			if (step % 2 == 0)
				step *= -1;

			Vector2 offset = new Vector2(this.PerimeterRadius * 0.50f, this.PerimeterRadius * 0.50f);
			Vector3 position = GetMapCellCoordinate(step, step, offset);

			return position;
		}


		public void RegeneratePerimeter()
		{
			GeneratePerimeter(GetMapCellPrefab(MapCell.Type.ExitNode));
			GeneratePerimeter(GetMapCellPrefab(MapCell.Type.DestroyableWall));
		}


		#region Helper Methods
		private GameObject InstantiateIntoMapCell(int x, int y, GameObject prefab, Vector2? offset = null)
		{
			GameObject cellObject;

			if (prefab == null)
				return null;
			else
				cellObject = Instantiate(prefab, this.transform);

			cellObject.transform.localPosition = new Vector3(
				x - this.NucleusSize * 0.5f + 0.5f,
				y - this.NucleusSize * 0.5f + 0.5f,
				0);
			if (offset != null)
			{
				cellObject.transform.localPosition =
					cellObject.transform.localPosition
						.AdjustX(offset.Value.x)
						.AdjustY(offset.Value.y);
			}

			return cellObject;
		}
		#endregion
	}
}


#region Editor
#if UNITY_EDITOR

namespace PrimordialOoze
{
	using UnityEditor;
	using UnityEngine;


	[CustomEditor(typeof(MicrobeMap))]
	public class GameMapInspector : Editor
	{
		private MicrobeMap microbeMap;


		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			if (Application.isPlaying)
			{
				if (this.microbeMap == null)
					this.microbeMap = target as MicrobeMap;

				if (GUILayout.Button("Regenerate Map"))
				{
					this.microbeMap.GenerateNewMap();
				}
			}
		}
	}
}

#endif
#endregion