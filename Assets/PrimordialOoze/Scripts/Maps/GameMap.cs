namespace PrimordialOoze
{
	using System;
	using System.Collections.Generic;
	using PrimordialOoze.Extensions.Vectors;
	using UnityEngine;


	public class GameMap : MonoBehaviour
	{
		public const float NucleusToHealthRatio = 0.1f;
		public const float PerimeterToHealthRatio = 0.2f;

		[SerializeField]
		private Microbe microbePrefab;

		[SerializeField]
		private MapCell wallPrefab;

		[SerializeField]
		private MapCell destroyableWallPrefab;

		[SerializeField]
		private MicrobeTraitToggle[] traitPrefabs;

		private MicrobeData currentMicrobe;
		private MicrobeData previousMicrobe;
		private MapCell[,] nucleusCells;
		private MapCell[] perimeterCells;
		private Microbe[] microbes;
		private MicrobeTraitToggle[] traits; // Must sort according to type enum.


		#region Properties
		public MicrobeData CurrentMicrobe
		{
			get { return this.currentMicrobe; }
		}


		public Microbe[] Microbes
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

			for (int i = 0; i < this.microbes.Length; i++)
			{
				if (this.microbes[i] != null)
					Destroy(this.microbes[i].gameObject);
			}
		}


		public void ClearNucleus()
		{
			if (this.nucleusCells == null)
				return;

			foreach (MapCell cell in this.nucleusCells)
			{
				if (cell != null)
					Destroy(cell.gameObject);
			}
		}


		public void ClearPerimeter()
		{
			if (this.perimeterCells == null)
				return;

			foreach (MapCell cell in this.perimeterCells)
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


		public void ExitCurrentMicrobe()
		{
			if (this.currentMicrobe.ParentMicrobe != null)
				SetCurrentMicrobe(this.currentMicrobe.ParentMicrobe);
			else
			{
				// TODO: Go to main map.
			}
		}


		public void GenerateMicrobes()
		{
			ClearMicrobes();
			MapData internalMap = this.currentMicrobe.InternalMap;
			int numberOfMicrobes = internalMap.Microbes.Count;
			this.microbes = new Microbe[numberOfMicrobes];
			for (int i = 0; i < numberOfMicrobes; i++)
			{
				this.microbes[i] = Instantiate(this.microbePrefab);
				this.microbes[i].Data = internalMap.Microbes[i];
			}
		}


		public void GenerateNewMap()
		{
			GenerateNucleus();
			GeneratePerimeter();
			GenerateMicrobes();
			GenerateTraits();
		}


		public void GeneratePerimeter()
		{
			ClearPerimeter();

			int x, y, r2;
			List<MapCell> cells = new List<MapCell>();
			int radius = this.PerimeterRadius;
			int center = this.PerimeterRadius;
			r2 = this.PerimeterRadius * this.PerimeterRadius;

			cells.Add(CreateCell(center, center + radius, MapCell.Type.Wall));
			cells.Add(CreateCell(center, center - radius, MapCell.Type.Wall));
			cells.Add(CreateCell(center + center, center, MapCell.Type.Wall));
			cells.Add(CreateCell(center - center, center, MapCell.Type.Wall));

			y = radius;
			x = 1;
			y = (int)(Math.Sqrt(r2 - 1) + 0.5);
			while (x < y)
			{
				cells.Add(CreateCell(center + x, center + y, MapCell.Type.Wall));
				cells.Add(CreateCell(center + x, center - y, MapCell.Type.Wall));
				cells.Add(CreateCell(center - x, center + y, MapCell.Type.Wall));
				cells.Add(CreateCell(center - x, center - y, MapCell.Type.Wall));
				cells.Add(CreateCell(center + y, center + x, MapCell.Type.Wall));
				cells.Add(CreateCell(center + y, center - x, MapCell.Type.Wall));
				cells.Add(CreateCell(center - y, center + x, MapCell.Type.Wall));
				cells.Add(CreateCell(center - y, center - x, MapCell.Type.Wall));
				x += 1;
				y = (int)(Math.Sqrt(r2 - x * x) + 0.5);
			}
			if (x == y)
			{
				cells.Add(CreateCell(center + x, center + y, MapCell.Type.Wall));
				cells.Add(CreateCell(center + x, center - y, MapCell.Type.Wall));
				cells.Add(CreateCell(center - x, center + y, MapCell.Type.Wall));
				cells.Add(CreateCell(center - x, center - y, MapCell.Type.Wall));
			}

			this.perimeterCells = cells.ToArray();
		}


		public void GenerateTraits()
		{
			ClearTraits();

			MapData internalMap = this.currentMicrobe.InternalMap;
			int numberOfTraits = internalMap.Traits.Length;
			this.traits = new MicrobeTraitToggle[numberOfTraits];
			for (int i = 0; i < numberOfTraits; i++)
			{
				MicrobeTrait data = internalMap.Traits[i];
				this.traits[i] = Instantiate(this.traitPrefabs[(int)data.Type]);
				this.traits[i].Data = data;
			}
		}


		public void SetCurrentMicrobe(MicrobeData microbeData)
		{
			this.currentMicrobe = microbeData;
			GenerateNewMap();
		}


		#region Helper Methods
		private MapCell CreateCell(int x, int y, MapCell.Type type, Vector2? offset = null)
		{
			MapCell newCell;

			if (type == MapCell.Type.Wall)
				newCell = Instantiate(this.wallPrefab);
			else if (type == MapCell.Type.DestroyableWall)
				newCell = Instantiate(this.destroyableWallPrefab);
			else
				return null;

			newCell.transform.parent = this.transform;
			newCell.transform.localPosition = new Vector3(
				x - this.NucleusSize * 0.5f + 0.5f,
				y - this.NucleusSize * 0.5f + 0.5f,
				0);
			if (offset != null)
			{
				newCell.transform.localPosition =
					newCell.transform.localPosition
						.AdjustX(offset.Value.x)
						.AdjustY(offset.Value.y);
			}

			return newCell;
		}


		private void GenerateNucleus()
		{
			ClearNucleus();

			Maze baseMaze = new Maze(this.NucleusSize);
			this.nucleusCells = new MapCell[this.NucleusSize, this.NucleusSize];

			for (int x = 0; x < this.NucleusSize; x++)
			{
				for (int y = 0; y < this.NucleusSize; y++)
				{
					if (baseMaze[x, y] != 0)
					{
						this.nucleusCells[x, y] = CreateCell(
							x,
							y,
							(MapCell.Type)baseMaze[x, y],
							new Vector2(this.PerimeterRadius * 0.75f, this.PerimeterRadius * 0.75f));
					}
				}
			}
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


	[CustomEditor(typeof(GameMap))]
	public class GameMapInspector : Editor
	{
		private GameMap gameMap;


		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			if (Application.isPlaying)
			{
				if (this.gameMap == null)
					this.gameMap = target as GameMap;

				if (GUILayout.Button("Regenerate Map"))
				{
					this.gameMap.GenerateNewMap();
				}
			}
		}
	}
}

#endif
#endregion