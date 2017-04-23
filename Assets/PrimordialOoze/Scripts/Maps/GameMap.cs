namespace PrimordialOoze
{
	using System;
	using System.Collections.Generic;
	using Assets.PrimordialOoze.Scripts.Extensions.Vectors;
	using UnityEngine;


	public class GameMap : MonoBehaviour
	{
		[SerializeField]
		private int perimeterRadius = 20;

		[SerializeField]
		private int nucleusSize = 10;

		[SerializeField]
		private MazeCell wallPrefab;

		[SerializeField]
		private MazeCell destroyableWallPrefab;

		private MazeCell[,] mazeCells;
		private MazeCell[] perimeterCells;


		public enum CellType
		{
			Empty = 0,
			Wall = 1,
			DestroyableWall = 2
		}


		public void ClearPerimeter()
		{
			if (this.perimeterCells == null)
				return;

			foreach (var cell in this.perimeterCells)
			{
				if (cell != null)
					Destroy(cell.gameObject);
			}
		}


		public void ClearMaze()
		{
			if (this.mazeCells == null)
				return;

			foreach (var cell in this.mazeCells)
			{
				if (cell != null)
					Destroy(cell.gameObject);
			}
		}


		public void GeneratePerimeter()
		{
			ClearPerimeter();

			int x, y, r2;
			List<MazeCell> cells = new List<MazeCell>();
			int radius = this.perimeterRadius;
			int center = this.perimeterRadius;
			r2 = this.perimeterRadius * this.perimeterRadius;
			
			cells.Add(CreateCell(center, center + radius, CellType.Wall));
			cells.Add(CreateCell(center, center - radius, CellType.Wall));
			cells.Add(CreateCell(center + center, center, CellType.Wall));
			cells.Add(CreateCell(center - center, center, CellType.Wall));

			y = radius;
			x = 1;
			y = (int)(Math.Sqrt(r2 - 1) + 0.5);
			while (x < y)
			{
				cells.Add(CreateCell(center + x, center + y, CellType.Wall));
				cells.Add(CreateCell(center + x, center - y, CellType.Wall));
				cells.Add(CreateCell(center - x, center + y, CellType.Wall));
				cells.Add(CreateCell(center - x, center - y, CellType.Wall));
				cells.Add(CreateCell(center + y, center + x, CellType.Wall));
				cells.Add(CreateCell(center + y, center - x, CellType.Wall));
				cells.Add(CreateCell(center - y, center + x, CellType.Wall));
				cells.Add(CreateCell(center - y, center - x, CellType.Wall));
				x += 1;
				y = (int)(Math.Sqrt(r2 - x * x) + 0.5);
			}
			if (x == y)
			{
				cells.Add(CreateCell(center + x, center + y, CellType.Wall));
				cells.Add(CreateCell(center + x, center - y, CellType.Wall));
				cells.Add(CreateCell(center - x, center + y, CellType.Wall));
				cells.Add(CreateCell(center - x, center - y, CellType.Wall));
			}

			this.perimeterCells = cells.ToArray();
		}

		public void GenerateNewMap()
		{
			ClearMaze();
			GeneratePerimeter();

			Maze baseMaze = new Maze(this.nucleusSize);
			this.mazeCells = new MazeCell[this.nucleusSize, this.nucleusSize];

			for (int x = 0; x < this.nucleusSize; x++)
			{
				for (int y = 0; y < this.nucleusSize; y++)
				{
					if (baseMaze[x, y] != 0)
					{
						this.mazeCells[x, y] = CreateCell(
							x, y,
							(CellType)baseMaze[x, y],
							new Vector2(this.perimeterRadius * 0.75f, this.perimeterRadius * 0.75f));
					}
				}
			}

			
		}


		public void Start()
		{
			GenerateNewMap();
		}


		#region Helper Methods
		private MazeCell CreateCell(int x, int y, CellType type, Vector2? offset = null)
		{
			MazeCell newCell;

			if (type == CellType.Wall)
				newCell = Instantiate(this.wallPrefab);
			else if (type == CellType.DestroyableWall)
				newCell = Instantiate(this.destroyableWallPrefab);
			else
				return null;


			newCell.transform.parent = this.transform;
			newCell.transform.localPosition = new Vector3(
				x - this.nucleusSize * 0.5f + 0.5f,
				y - this.nucleusSize * 0.5f + 0.5f,
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