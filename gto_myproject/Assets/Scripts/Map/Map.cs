using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class Map : MonoBehaviour, ICellRange
{
	public int Height;
	public int Width;

	public Cell[] CellPrefabs;

	private Cell[,] _cells;
	private Cell _selectedCell;

	public void SelectCell(Point pos)
	{
		foreach (var c in _cells)
		{
			if (!c.Position.Equals(pos)) continue;
			_selectedCell = c;
			c.Select();
			return;
		}
	}

	public Cell[,] GetCellArray()
	{
		return _cells;
	}

	public List<Cell> GetCellsInRange(Cell center, int range, bool includeEnemies)
	{
		var cellsInRange = new List<Cell>();

		cellsInRange.Add(center);

		for (int steps = 0; steps < range; steps++)
		{
			var temp = new List<Cell>();
			foreach (var cell in cellsInRange)
			{
				var neighbors = GetCellNeighbors(cell);

				foreach (var neighbor in neighbors)
				{
					if (includeEnemies)
					{
						if (!cellsInRange.Contains(neighbor) && neighbor.CellType == CellType.GROUND) temp.Add(neighbor);
					}
					else
					{
						if (!cellsInRange.Contains(neighbor) && neighbor.CanMove()) temp.Add(neighbor);
					}
				}
			}

			cellsInRange.AddRange(temp);
		}

		return cellsInRange;
	}

	public int GetDistance(Cell cell1, Cell cell2)
	{
		throw new System.NotImplementedException();
	}

	public List<Cell> GetCellNeighbors(Cell cell)
	{
		var neighbors = new List<Cell>();
		var pos = cell.Position;

		if (pos.x + 1 < Width) neighbors.Add(_cells[pos.x + 1, pos.y]);
		if (pos.x - 1 >= 0) neighbors.Add(_cells[pos.x - 1, pos.y]);
		if (pos.y + 1 < Height) neighbors.Add(_cells[pos.x, pos.y + 1]);
		if (pos.y - 1 >= 0) neighbors.Add(_cells[pos.x, pos.y - 1]);

		return neighbors;
	}

	public void SelectCell(Cell cell)
	{
		SelectCell(cell.Position);
	}

	public Cell GetSelectedCell()
	{
		return _selectedCell;
	}

	public void UnselectCell()
	{
		_selectedCell = null;
	}

	private void Awake()
	{
		GenerateMap();
	}

	private Cell GetRandomFreeCell(Point from, Point to)
	{
		var freeCells = new List<Cell>();
		foreach (var c in _cells)
		{
			if (c.Empty && c.CellType == CellType.GROUND && WithinBounds(c.Position, from, to))
			{
				freeCells.Add(c);
			}
		}

		var i = Random.Range(0, freeCells.Count);
		return freeCells[i];
	}

	private bool WithinBounds(Point pos, Point from, Point to)
	{
		if (pos.x >= from.x && pos.x < to.x)
		{
			if (pos.y >= from.y && pos.y < to.y)
				return true;
		}

		return false;
	}

	public Cell GetRandomStartingPosition(int playerId)
	{
		switch (playerId)
		{
			case 0:
				return GetRandomFreeCell(new Point(0, 0), new Point(Width, Height / 2));
			case 1:
				return GetRandomFreeCell(new Point(0, Height / 2), new Point(Width, Height));
		}

		return null;
	}

	private void GenerateMap()
	{
		_cells = new Cell[Width, Height];

		var seedX = Random.Range(0, 100);
		var seedY = Random.Range(0, 100);
		
		for (int x = 0; x < Width; x++)
		{
			for (int y = 0; y < Height; y++)
			{
				var height = Mathf.PerlinNoise((seedX + x) / 7.0f, (seedY + y) / 7.0f);

				var index = 0;
				var cellType = GetCellType(height);
				
				switch (cellType)
				{
					case CellType.WATER:	index = 0; break;
					case CellType.GROUND:	index = 1; break;
					case CellType.MOUNTAIN: index = 2; break;
					case CellType.NONE:		Debug.LogError("Bad CellType"); break;
				}
				
				_cells[x, y] = Instantiate(CellPrefabs[index], new Vector3(x, 0, y), Quaternion.identity, gameObject.transform);
				_cells[x, y].Construct(this, new Point(x, y), cellType);
			}
		}
	}
	
	private static CellType GetCellType(float height)
	{
		if (height >= 0.0 && height < 0.2) return CellType.WATER;
		if (height >= 0.2 && height < 0.7) return CellType.GROUND;
		if (height >= 0.7 && height < 9.0) return CellType.MOUNTAIN;

		return CellType.NONE;
	}
}