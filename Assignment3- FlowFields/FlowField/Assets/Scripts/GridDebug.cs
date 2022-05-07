using UnityEditor;
using UnityEngine;


public enum FlowFieldDisplayType { None, FlowField, CostField, IntegrationField };

public class GridDebug : MonoBehaviour
{
	public GridController gridController;
	public bool displayGrid;

	public FlowFieldDisplayType curDisplayType;

	private Vector2Int gridSize;
	private float GridCellRadius;
	private FlowField curFlowField;

	public Sprite[] ffIcons;

	private void Start()
	{
		ffIcons = Resources.LoadAll<Sprite>("Sprites/FFicons");
	}

	public void SetFlowField(FlowField newFlowField)
	{
		curFlowField = newFlowField;
		GridCellRadius = newFlowField.cellRadius;
		gridSize = newFlowField.gridSize;
	}

	public void DrawFlowField()
	{
		ClearGridCellDisplay();

		switch (curDisplayType)
		{
			case FlowFieldDisplayType.FlowField:
				DisplayAllGridCells();
				break;

			default:
				break;
		}
	}

	private void DisplayAllGridCells()
	{
		if (curFlowField == null) { return; }
		foreach (GridCell curGridCell in curFlowField.grid)
		{
			DisplayGridCell(curGridCell);
		}
	}


	private void DisplayGridCell(GridCell GridCell)
	{
		GameObject iconGO = new GameObject();
		SpriteRenderer iconSR = iconGO.AddComponent<SpriteRenderer>();
		iconGO.transform.parent = transform;
		iconGO.transform.position = GridCell.worldPos;

		if (GridCell.cost == 0)
		{
			iconSR.sprite = ffIcons[3];
			Quaternion newRot = Quaternion.Euler(90, 0, 0);
			iconGO.transform.rotation = newRot;
		}
		else if (GridCell.cost == byte.MaxValue)
		{
			iconSR.sprite = ffIcons[2];
			Quaternion newRot = Quaternion.Euler(90, 0, 0);
			iconGO.transform.rotation = newRot;
		}
		else if (GridCell.bestDirection == GridDirection.North)
		{
			iconSR.sprite = ffIcons[0];
			Quaternion newRot = Quaternion.Euler(90, 0, 0);
			iconGO.transform.rotation = newRot;
		}
		else if (GridCell.bestDirection == GridDirection.South)
		{
			iconSR.sprite = ffIcons[0];
			Quaternion newRot = Quaternion.Euler(90, 180, 0);
			iconGO.transform.rotation = newRot;
		}
		else if (GridCell.bestDirection == GridDirection.East)
		{
			iconSR.sprite = ffIcons[0];
			Quaternion newRot = Quaternion.Euler(90, 90, 0);
			iconGO.transform.rotation = newRot;
		}
		else if (GridCell.bestDirection == GridDirection.West)
		{
			iconSR.sprite = ffIcons[0];
			Quaternion newRot = Quaternion.Euler(90, 270, 0);
			iconGO.transform.rotation = newRot;
		}
		else if (GridCell.bestDirection == GridDirection.NorthEast)
		{
			iconSR.sprite = ffIcons[1];
			Quaternion newRot = Quaternion.Euler(90, 0, 0);
			iconGO.transform.rotation = newRot;
		}
		else if (GridCell.bestDirection == GridDirection.NorthWest)
		{
			iconSR.sprite = ffIcons[1];
			Quaternion newRot = Quaternion.Euler(90, 270, 0);
			iconGO.transform.rotation = newRot;
		}
		else if (GridCell.bestDirection == GridDirection.SouthEast)
		{
			iconSR.sprite = ffIcons[1];
			Quaternion newRot = Quaternion.Euler(90, 90, 0);
			iconGO.transform.rotation = newRot;
		}
		else if (GridCell.bestDirection == GridDirection.SouthWest)
		{
			iconSR.sprite = ffIcons[1];
			Quaternion newRot = Quaternion.Euler(90, 180, 0);
			iconGO.transform.rotation = newRot;
		}
		else
		{
			iconSR.sprite = ffIcons[0];
		}
	}

	public void ClearGridCellDisplay()
	{
		foreach (Transform t in transform)
		{
			GameObject.Destroy(t.gameObject);
		}
	}
	
	private void OnDrawGizmos()
	{
		if (displayGrid)
		{
			if (curFlowField == null)
			{
				DrawGrid(gridController.gridSize, Color.yellow, gridController.cellRadius);
			}
			else
			{
				DrawGrid(gridSize, Color.green, GridCellRadius);
			}
		}

		if (curFlowField == null) { return; }

		GUIStyle style = new GUIStyle(GUI.skin.label);
		style.alignment = TextAnchor.MiddleCenter;

		switch (curDisplayType)
		{
			case FlowFieldDisplayType.CostField:
				foreach (GridCell curGridCell in curFlowField.grid)
				{
					Handles.Label(curGridCell.worldPos, curGridCell.cost.ToString(), style);
				}
				break;

			case FlowFieldDisplayType.IntegrationField:

				foreach (GridCell curGridCell in curFlowField.grid)
				{
					Handles.Label(curGridCell.worldPos, curGridCell.bestCost.ToString(), style);
				}
				break;

			default:
				break;
		}

	}

	private void DrawGrid(Vector2Int drawGridSize, Color drawColor, float drawGridCellRadius)
	{
		Gizmos.color = drawColor;
		for (int x = 0; x < drawGridSize.x; x++)
		{
			for (int y = 0; y < drawGridSize.y; y++)
			{
				Vector3 center = new Vector3(drawGridCellRadius * 2 * x + drawGridCellRadius, 0, drawGridCellRadius * 2 * y + drawGridCellRadius);
				Vector3 size = Vector3.one * drawGridCellRadius * 2;
				Gizmos.DrawWireCube(center, size);
			}
		}
	}
}
