using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfluenceMapControl : MonoBehaviour
{
	public static InfluenceMapControl instance { get; private set; }
	[SerializeField] Transform bottomLeft;

	[SerializeField] Transform topRight;

	[SerializeField] GameObject blueTower;
	[SerializeField] GameObject redTower;
	[SerializeField] float gridCellSize;
	[SerializeField] int gridHeight;
	[SerializeField] int gridWidth;
	Vector3 towerOffset = new Vector3(0,2,0);
	[SerializeField] float influenceSize = 0.3f;
					 
	[SerializeField] float momentum = 0.8f;
					 
	[SerializeField] int updateFrequency = 3;
					 
	[SerializeField] InfluenceMap influenceMap;

	[SerializeField] GridDisplay display;

	public List<GameObject> blueTowerList;
	public List<GameObject> redTowerList;

	void CreateMap()
	{
		influenceMap = new InfluenceMap(gridWidth, gridHeight, influenceSize, momentum);

		display.SetGridData(influenceMap);
		display.CreateMesh(gridCellSize);
	}

	public void RegisterTower(ITower t)
	{
		influenceMap.RegisterTower(t);
	}

	public void DeregisterTower(ITower t)
	{
		influenceMap.DeregisterTower(t);
	}


	public Vector2I GetGridPosition(Vector3 pos)
	{
		int x = (int)((pos.x - bottomLeft.position.x) / gridCellSize);
		int y = (int)((pos.z - bottomLeft.position.z) / gridCellSize);

		return new Vector2I(x, y);
	}

	public void GetMovementLimits(out Vector3 _bottomLeft, out Vector3 _topRight)
	{
		_bottomLeft = bottomLeft.position;
		_topRight = topRight.position;
	}

	void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		CreateMap();

		InvokeRepeating("TowerUpdate", 0.001f, 1.0f / updateFrequency);
	}

	void TowerUpdate()
	{
		influenceMap.UpdateTower();
	}

	private void Start()
	{
		redTowerList = new List<GameObject>();
		blueTowerList = new List<GameObject>();
	}

	void Update()
	{
		influenceMap.InfluenceSize = influenceSize;
		influenceMap.Momentum = momentum;

		//RemoveDeadTowers();
		Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit mouseHit;
		if (Physics.Raycast(mouseRay, out mouseHit) && Input.GetMouseButtonDown(0))
		{
			// is it within the grid
			// if so, call SetInfluence in that grid position to 1.0
			Vector3 hit = mouseHit.point;
			if (hit.x > bottomLeft.position.x && hit.x < topRight.position.x && hit.z > bottomLeft.position.z && hit.z < topRight.position.z)
			{
				Vector2I gridPos = GetGridPosition(hit);
				if (gridPos.x < influenceMap.Width && gridPos.y < influenceMap.Height)
				{
					GameObject newBlueTower = Instantiate(blueTower, hit + towerOffset, Quaternion.identity);
					InfluenceMapControl map = FindObjectOfType<InfluenceMapControl>();
					newBlueTower.GetComponent<InfluenceTower>().SetValue(-1);
					newBlueTower.GetComponent<InfluenceTower>().SetInfluenceMap(map);
					blueTowerList.Add(newBlueTower);
					//SetInfluence(gridPos, (Input.GetMouseButton(0) ? 1.0f : -1.0f));
				}
			}
		}

		else if (Physics.Raycast(mouseRay, out mouseHit) && Input.GetMouseButtonDown(1))
		{
			// is it within the grid
			// if so, call SetInfluence in that grid position to 1.0
			Vector3 hit = mouseHit.point;
			if (hit.x > bottomLeft.position.x && hit.x < topRight.position.x && hit.z > bottomLeft.position.z && hit.z < topRight.position.z)
			{
				Vector2I gridPos = GetGridPosition(hit);
				if (gridPos.x < influenceMap.Width && gridPos.y < influenceMap.Height)
				{
					GameObject newRedTower = Instantiate(redTower, hit + towerOffset, Quaternion.identity);
					InfluenceMapControl map = FindObjectOfType<InfluenceMapControl>();
					newRedTower.GetComponent<InfluenceTower>().SetValue(1);
					newRedTower.GetComponent<InfluenceTower>().SetInfluenceMap(map);
					redTowerList.Add(newRedTower);
				}
			}
		}
	}

	void RemoveDeadTowers()
	{
		foreach(GameObject tower in blueTowerList)
		{
			if(tower.GetComponent<InfluenceTower>().GetHealth() <= 0)
			{
				//blueTowerList.Remove(tower);
				tower.GetComponent<InfluenceTower>().DeleteTower();
				Destroy(tower);
			}
		}

		foreach (GameObject tower in redTowerList)
		{
			if (tower.GetComponent<InfluenceTower>().GetHealth() <= 0)
			{
				//redTowerList.Remove(tower);
				tower.GetComponent<InfluenceTower>().DeleteTower();
				Destroy(tower);
			}
		}
	}
	public int GetGridHeight()
	{
		return gridHeight;
	}

	public int GetGridWidth()
	{
		return gridWidth;
	}
}