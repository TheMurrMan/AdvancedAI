using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Allignment")]
public class AllignmentBehaviour : FilteredFlockBehavior
{
	public override Vector2 CalulateMove(FlockAgent agent, List<Transform> context, Flock flock)
	{
		// if there are no nearby neighbors, return current allignment
		if (context.Count == 0)
		{
			return agent.transform.up;
		}

		Vector2 allignmentMove = Vector2.zero;
		foreach (Transform item in context)
		{
			allignmentMove += (Vector2)item.transform.up;
		}

		allignmentMove /= context.Count;

		return allignmentMove;
	}
}
