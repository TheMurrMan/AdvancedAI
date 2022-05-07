using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public Vector2Int gridSize;
    public float cellRadius = 0.5f;
    public FlowField currentFlowField;
	public GridDebug gridDebug;
    private void InitFlowField()
	{
        currentFlowField = new FlowField(cellRadius, gridSize);
        currentFlowField.CreateGrid();
		gridDebug.SetFlowField(currentFlowField);
	}

	private void Update()
	{
		if(Input.GetMouseButtonDown(0))
		{
			InitFlowField();
			currentFlowField.CreateCostField();

			Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 20f);
			Debug.Log(mousePos);
			Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);
			GridCell destinationCell = currentFlowField.GetCellFromWorldPos(worldMousePos);
			Debug.Log(destinationCell);
			currentFlowField.CreateIntegrationField(destinationCell);
			currentFlowField.CreateFlowField();
			gridDebug.DrawFlowField();
		}
	}
}
