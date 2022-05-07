using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    public GameObject blueTeamUnitPrefab;
    public GameObject redTeamUnitPrefab;
    public int numUnitToSpawn;

    private List<GameObject> blueTeamUnitsInGame;
    private List<GameObject> redTeamUnitsInGame;

	private void Awake()
	{
        redTeamUnitsInGame = new List<GameObject>();
        blueTeamUnitsInGame = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
		{
            SpawnUnits(blueTeamUnitPrefab, blueTeamUnitsInGame);
		}

        if (Input.GetKeyDown(KeyCode.R))
        {
            SpawnUnits(redTeamUnitPrefab,redTeamUnitsInGame);
        }


        if (Input.GetKeyDown(KeyCode.D))
        {
            DestroyUnits();
        }
    }


    private void SpawnUnits(GameObject unitPrefab, List<GameObject> unitsInGame)
	{
        Vector3 newPos;

        for(int i = 0; i < numUnitToSpawn; ++i)
		{
            GameObject newUnit = Instantiate(unitPrefab);
            newUnit.transform.parent = transform;
            unitsInGame.Add(newUnit);
            
            newPos = new Vector3(Random.Range(0, InfluenceMapControl.instance.GetGridWidth()), 0.25f, Random.Range(0, InfluenceMapControl.instance.GetGridHeight()));
            newUnit.transform.position = newPos;
		}
	}

    private void DestroyUnits()
	{
        foreach(GameObject go in blueTeamUnitsInGame)
		{
            Destroy(go);
		}

        foreach (GameObject go in redTeamUnitsInGame)
        {
            Destroy(go);
        }

        blueTeamUnitsInGame.Clear();
        redTeamUnitsInGame.Clear();
    }
}
