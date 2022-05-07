using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/ObsticleAvoidance")]
public class ObsticleAvoidance : ContextFilters
{
	public LayerMask mask;
	public override List<Transform> Filter(FlockAgent agent, List<Transform> orig)
	{
		List<Transform> filtered = new List<Transform>();

		foreach (Transform item in orig)
		{
			if (mask == (mask | (1 << item.gameObject.layer)))
			{
				filtered.Add(item);
			}
		}

		return filtered;
	}
}
