namespace PrimordialOoze
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using PrimordialOoze.Extensions.Vectors;
	using UnityEngine;


	public class MicrobeMap : MonoBehaviour
	{
		public const float NucleusToHealthRatio = 0.1f;
		public const float PerimeterToHealthRatio = 0.2f;

		[SerializeField]
		private Microbe microbePrefab;

		[SerializeField]
		private MapCell[] mapCellPrefabs; // Must sort according to type enum.

		[SerializeField]
		private MicrobeTraitToggle[] traitPrefabs;

		private MicrobeData currentMicrobe;
		private MicrobeData previousMicrobe;
		private MapCell[,] nucleusCells;
		private List<MapCell> perimeterCells;
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


		public void ClearPerimeters()
		{
			if (this.perimeterCells == null)
				this.perimeterCells = new List<MapCell>();

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
			var parentMicrobeData =
				Game.MicrobeMap.CurrentMicrobe.ParentMicrobeData;
			if (parentMicrobeData != null)
			{
				Debug.Log("Teleporting player to parent microbe.");
				Game.MicrobeMap.SetCurrentMicrobe(parentMicrobeData);
				Microbe parentMicrobe = Game.MicrobeMap.FindMicrobe(parentMicrobeData);
				if (parentMicrobe != null)
					Game.Player.transform.position = parentMicrobe.transform.position;
				else
					Debug.Log("Couldn't find parent microbe to teleport to.");
			}
			else
			{
				Debug.Log("Teleporting to overworld.");
				// TODO.
			}
		}


		public Microbe FindMicrobe(MicrobeData microbeData)
		{
			return this.microbes.FirstOrDefault(
				microbe => microbe.Data == microbeData);
		}


		public void GenerateMicrobes()
		{
			MapData internalMap = this.currentMicrobe.Map;
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
			ClearNucleus();
			GenerateNucleus();

			ClearPerimeters();
			GeneratePerimeter(MapCell.Type.ExitNode);
			GeneratePerimeter(MapCell.Type.DestroyableWall);

			ClearMicrobes();
			GenerateMicrobes();

			ClearTraits();
			GenerateTraits();
		}


		public void GeneratePerimeter(MapCell.Type perimeterType)
		{
			int x, y, r2;
			List<MapCell> cells = new List<MapCell>();
			int radius = this.PerimeterRadius;
			int center = this.PerimeterRadius;
			r2 = this.PerimeterRadius * this.PerimeterRadius;

			cells.Add(CreateCell(center, center + radius, perimeterType));
			cells.Add(CreateCell(center, center - radius, perimeterType));
			cells.Add(CreateCell(center + center, center, perimeterType));
			cells.Add(CreateCell(center - center, center, perimeterType));

			y = radius;
			x = 1;
			y = (int)(Math.Sqrt(r2 - 1) + 0.5);
			while (x < y)
			{
				cells.Add(CreateCell(center + x, center + y, perimeterType));
				cells.Add(CreateCell(center + x, center - y, perimeterType));
				cells.Add(CreateCell(center - x, center + y, perimeterType));
				cells.Add(CreateCell(center - x, center - y, perimeterType));
				cells.Add(CreateCell(center + y, center + x, perimeterType));
				cells.Add(CreateCell(center + y, center - x, perimeterType));
				cells.Add(CreateCell(center - y, center + x, perimeterType));
				cells.Add(CreateCell(center - y, center - x, perimeterType));
				x += 1;
				y = (int)(Math.Sqrt(r2 - x * x) + 0.5);
			}
			if (x == y)
			{
				cells.Add(CreateCell(center + x, center + y, perimeterType));
				cells.Add(CreateCell(center + x, center - y, perimeterType));
				cells.Add(CreateCell(center - x, center + y, perimeterType));
				cells.Add(CreateCell(center - x, center - y, perimeterType));
			}

			this.perimeterCells.AddRange(cells);
		}


		public void GenerateTraits()
		{
			MapData internalMap = this.currentMicrobe.Map;
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

			if (type == MapCell.Type.Empty)
				return null;
			else
				newCell = Instantiate(this.mapCellPrefabs[(int)type - 1]);

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