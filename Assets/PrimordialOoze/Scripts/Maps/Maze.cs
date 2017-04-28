namespace PrimordialOoze
{
	using System;
	using System.Security.Cryptography;
	using UnityEngine;


	public class Maze
	{
		private static RNGCryptoServiceProvider random = new RNGCryptoServiceProvider();

		private readonly int size;
		private int[,] cells;


		#region Constructors
		public Maze(int size)
		{
			this.size = size;
			GenerateNewMaze();
		}
		#endregion


		#region Properties
		public int[,] Cells
		{
			get { return this.cells; }
		}


		public int Size
		{
			get { return this.size; }
		}


		public int this[int x, int y]
		{
			get { return this.cells[x, y]; }
		}
		#endregion


		public void GenerateNewMaze()
		{
			this.cells = new int[size, size];

			int mid = (int)(size / 2) - 1;
			for (int i = 0; i < size; i++)
			{
				for (int j = 0; j < size; j++)
				{
					// Nucleus center
					if (i == mid
						&& j == mid)
					{
						this.cells[i, j] = 4;
					}
					// Nucleus wall
					else if (i != 0
						&& i != this.size - 1
						&& j != 0
						&& j != this.size - 1)
					{
						this.cells[i, j] = 1;
					}

					// Nucleus membrane
					else
						this.cells[i, j] = 2;

				}
			}

			int x = RandomInteger(0, size);
			while (x % 2 == 0)
			{
				x = RandomInteger(0, size);
			}

			int y = RandomInteger(0, size);
			while (y % 2 == 0)
			{
				y = RandomInteger(0, size);
			}

			this.cells[x, y] = 0;
			ShuffleCells(x, y);
		}


		#region Helper Methods
		private static int RandomInteger(int min, int max)
		{
			uint scale = uint.MaxValue;
			while (scale == uint.MaxValue)
			{
				byte[] fourBytes = new byte[4];
				random.GetBytes(fourBytes);
				scale = BitConverter.ToUInt32(fourBytes, 0);
			}

			return (int)(min + (max - min)
						* (scale / (double)uint.MaxValue));
		}


		private void Shuffle(int[] directions)
		{
			for (int i = directions.Length; i > 1; i--)
			{
				int j = RandomInteger(0, i);
				int temp = directions[j];
				directions[j] = directions[i - 1];
				directions[i - 1] = temp;
			}
		}


		private void ShuffleCells(int x, int y)
		{
			int[] directions = { 1, 2, 3, 4 };
			Shuffle(directions);

			for (int i = 0; i < directions.Length; i++)
			{
				switch (directions[i])
				{
					// Up
					case 1:
						if (x - 2 <= 0)
							continue;
						if (this.cells[x - 2, y] != 0
							&& this.cells[x - 2, y] != 4)
						{
							this.cells[x - 2, y] = 0;
							this.cells[x - 1, y] = 0;
							ShuffleCells(x - 2, y);
						}
						break;

					// Right
					case 2:
						if (y + 2 >= this.size - 1)
							continue;
						if (this.cells[x, y + 2] != 0
							&& this.cells[x, y + 2] != 4)
						{
							this.cells[x, y + 2] = 0;
							this.cells[x, y + 1] = 0;
							ShuffleCells(x, y + 2);
						}
						break;

					// Down
					case 3:
						if (x + 2 >= this.size - 1)
							continue;
						if (this.cells[x + 2, y] != 0
							&& this.cells[x + 2, y] != 4)
						{
							this.cells[x + 2, y] = 0;
							this.cells[x + 1, y] = 0;
							ShuffleCells(x + 2, y);
						}
						break;

					// Left
					case 4:
						if (y - 2 <= 0)
							continue;
						if (this.cells[x, y - 2] != 0
							&& this.cells[x, y - 2] != 4)
						{
							this.cells[x, y - 2] = 0;
							this.cells[x, y - 1] = 0;
							ShuffleCells(x, y - 2);
						}
						break;
				}
			}
		}
		#endregion
	}
}