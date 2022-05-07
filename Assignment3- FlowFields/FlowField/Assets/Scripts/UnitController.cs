using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    public GridController gridController;
    public GameObject unitPrefab;
    public int numUnitToSpawn;
    public float moveSpeed;

    private List<GameObject> unitsInGame;

	private void Awake()
	{
        unitsInGame = new List<GameObject>();
	}
	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
		{
            SpawnUnits();
		}

        if (Input.GetKeyDown(KeyCode.D))
        {
            DestroyUnits();
        }
    }

	private void FixedUpdate()
	{
		if(gridController.currentFlowField == null)
		{
            return;
		}

        foreach(GameObject unit in unitsInGame)
		{
            GridCell nodeBelow = gridController.currentFlowField.GetCellFromWorldPos(unit.transform.position);
            Vector3 moveDir = new Vector3(nodeBelow.bestDirection.vector.x, 0, nodeBelow.bestDirection.vector.y);
            Rigidbody rb = unit.GetComponent<Rigidbody>();
            rb.velocity = moveDir * moveSpeed;
		}
	}

    private void SpawnUnits()
	{
        Vector2Int gridSize = gridController.gridSize;
        float nodeRadius = gridController.cellRadius;
        Vector2 maxSpawnPos = new Vector2(gridSize.x * nodeRadius * 2 + nodeRadius, gridSize.y * nodeRadius * 2 + nodeRadius);
        int colMask = LayerMask.GetMask("Impassible, Units");
        Vector3 newPos;

        for(int i = 0; i < numUnitToSpawn; ++i)
		{
            GameObject newUnit = Instantiate(unitPrefab);
            newUnit.transform.parent = transform;
            unitsInGame.Add(newUnit);

            do
            {
                newPos = new Vector3(Random.Range(0, maxSpawnPos.x), 0.25f, Random.Range(0, maxSpawnPos.y));
                newUnit.transform.position = newPos;
            }
            while (Physics.OverlapSphere(newPos, 0.25f, colMask).Length > 0);
		}
	}

    private void DestroyUnits()
	{
        foreach(GameObject go in unitsInGame)
		{
            Destroy(go);
		}

        unitsInGame.Clear();
	}
}
