using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITower
{
    Vector2I GridPosition { get; }
    float Value { get; }
}

public class InfluenceTower : MonoBehaviour, ITower
{
	public static InfluenceTower instance { get; private set; }
	[SerializeField] float value;
	public float Value { get { return value; } }

	public InfluenceMapControl map;

	public int towerHealth;

	private void Awake()
	{
		if(instance == null)
		{
			instance = this;
		}
	}
	public Vector2I GridPosition
	{
		get { return map.GetGridPosition(transform.position); }
	}

	// Use this for initialization
	void Start()
	{
		map.RegisterTower(this);
	}

	private void Update()
	{
		if(towerHealth <= 0)
		{
			Debug.Log("DIE");
			DeleteTower();
			Destroy(gameObject);
		}
	}
	public void DeleteTower()
	{
		map.DeregisterTower(this);
	}
	public void SetInfluenceMap(InfluenceMapControl _influenceMap)
	{
		map = _influenceMap;
	}

	public void SetValue(int _value)
	{
		value = _value;
	}

	public void TakeDamage(int damage)
	{
		towerHealth -= damage;
	}

	public int GetHealth()
	{
		return towerHealth;
	}
}
