using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowField
{
	public GridCell[,] grid { get; private set; }
	public Vector2Int gridSize { get; private set; }
	public float cellRadius { get; private set; }
	public GridCell destinationCell;

	private float cellDiameter;

	public FlowField(float _cellRadius, Vector2Int _gridSize)
	{
		cellRadius = _cellRadius;
		cellDiameter = cellRadius * 2f;
		gridSize = _gridSize;
	}

	public void CreateGrid()
	{
		
		grid = new GridCell[gridSize.x, gridSize.y];

		for (int x = 0; x < gridSize.x; x++)
		{
			for (int y = 0; y < gridSize.y; y++)
			{
				Vector3 worldPos = new Vector3(cellDiameter * x + cellRadius, 0, cellDiameter * y + cellRadius);
				grid[x, y] = new GridCell(worldPos, new Vector2Int(x, y));
				//Debug.Log(x + ", " + y);
			}
		}
	}

	public void CreateCostField()
	{
		Vector3 cellHalfExtents = Vector3.one * cellRadius;
		int terrainMask = LayerMask.GetMask("Water","Impassible", "RoughTerrain","Mountain" );
		foreach (GridCell curCell in grid)
		{
			Collider[] obstacles = Physics.OverlapBox(curCell.worldPos, cellHalfExtents, Quaternion.identity, terrainMask);
			bool hasIncreasedCost = false;
			foreach (Collider col in obstacles)
			{
				if (col.gameObject.layer == 8)
				{
					curCell.IncreaseCost(255);
					continue;
				}
				else if (!hasIncreasedCost && col.gameObject.layer == 4)
				{
					curCell.IncreaseCost(5);
					hasIncreasedCost = true;
				}
				else if (!hasIncreasedCost && col.gameObject.layer == 9)
				{
					curCell.IncreaseCost(3);
					hasIncreasedCost = true;
				}

				else if (!hasIncreasedCost && col.gameObject.layer == 10)
				{
					curCell.IncreaseCost(10);
					hasIncreasedCost = true;
				}
			}
		}
	}

	public void CreateIntegrationField(GridCell _destinationCell)
	{
		destinationCell = _destinationCell;

		destinationCell.cost = 0;
		destinationCell.bestCost = 0;

		Queue<GridCell> cellsToCheck = new Queue<GridCell>();

		cellsToCheck.Enqueue(destinationCell);

		while (cellsToCheck.Count > 0)
		{
			GridCell curCell = cellsToCheck.Dequeue();
			List<GridCell> curNeighbors = GetNeighborCells(curCell.gridIndex, GridDirection.CardinalDirections);
			foreach (GridCell curNeighbor in curNeighbors)
			{
				if (curNeighbor.cost == byte.MaxValue) { continue; }
				if (curNeighbor.cost + curCell.bestCost < curNeighbor.bestCost)
				{
					curNeighbor.bestCost = (ushort)(curNeighbor.cost + curCell.bestCost);
					cellsToCheck.Enqueue(curNeighbor);
				}
			}
		}
	}

	public void CreateFlowField()
	{
		foreach (GridCell curCell in grid)
		{
			List<GridCell> curNeighbors = GetNeighborCells(curCell.gridIndex, GridDirection.AllDirections);

			int bestCost = curCell.bestCost;

			foreach (GridCell curNeighbor in curNeighbors)
			{
				if (curNeighbor.bestCost < bestCost)
				{
					bestCost = curNeighbor.bestCost;
					curCell.bestDirection = GridDirection.GetDirectionFromV2I(curNeighbor.gridIndex - curCell.gridIndex);
				}
			}
		}
	}

	private List<GridCell> GetNeighborCells(Vector2Int nodeIndex, List<GridDirection> directions)
	{
		List<GridCell> neighborCells = new List<GridCell>();

		foreach (Vector2Int curDirection in directions)
		{
			GridCell newNeighbor = GetCellAtRelativePos(nodeIndex, curDirection);
			if (newNeighbor != null)
			{
				neighborCells.Add(newNeighbor);
			}
		}
		return neighborCells;
	}

	private GridCell GetCellAtRelativePos(Vector2Int orignPos, Vector2Int relativePos)
	{
		Vector2Int finalPos = orignPos + relativePos;

		if (finalPos.x < 0 || finalPos.x >= gridSize.x || finalPos.y < 0 || finalPos.y >= gridSize.y)
		{
			return null;
		}

		else { return grid[finalPos.x, finalPos.y]; }
	}

	public GridCell GetCellFromWorldPos(Vector3 worldPos)
	{
		float percentX = worldPos.x / (gridSize.x * cellDiameter);
		float percentY = worldPos.z / (gridSize.y * cellDiameter);

		percentX = Mathf.Clamp01(percentX);
		percentY = Mathf.Clamp01(percentY);

		int x = Mathf.Clamp(Mathf.FloorToInt((gridSize.x) * percentX), 0, gridSize.x - 1);
		int y = Mathf.Clamp(Mathf.FloorToInt((gridSize.y) * percentY), 0, gridSize.y - 1);
		return grid[x, y];
	}
}
