using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public GameObject closestTower;
    public float moveSpeed;

    public UnitTeam team;
    public enum UnitTeam
	{
        Red,
        Blue
	}
    
    // Update is called once per frame
    void Update()
    {
        if(closestTower == null)
		{
            if (team == UnitTeam.Blue)
            {
                FindClosestEnemyTower("RedTower");
            }

            if (team == UnitTeam.Red)
            {
                FindClosestEnemyTower("BlueTower");
            }
        }

        if(closestTower != null)
		{
            MoveUnit();
        }
    }

    void MoveUnit()
	{
        Vector3 targetPos = closestTower.transform.position;
        Vector3 currentPos = this.transform.position;

        Vector3 moveDir = (targetPos - currentPos).normalized;

        float distance = Vector3.Distance(currentPos, targetPos);

        if(closestTower != null)
		{
            if (distance >= 1f)
            {
                transform.position += moveDir * (moveSpeed * Time.deltaTime);
            }

            else
            {
                DamageTower(closestTower);
            }
        }
	}

    void DamageTower(GameObject closestTower)
	{
        closestTower.GetComponent<InfluenceTower>().TakeDamage(1);
	}

    void FindClosestEnemyTower(string tag)
    {
        closestTower = null;
        float distanceToClosestPlayerUnit = Mathf.Infinity;
        GameObject[] towers = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject tower in towers)
        {
            float distanceToPlayerUnit = (tower.transform.position - transform.position).sqrMagnitude;
            if (distanceToPlayerUnit < distanceToClosestPlayerUnit)
            {
                distanceToClosestPlayerUnit = distanceToPlayerUnit;
                closestTower = tower;
            }
        }
    }
}
