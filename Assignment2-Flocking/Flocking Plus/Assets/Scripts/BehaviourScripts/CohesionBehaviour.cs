using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Cohesion" )]
public class CohesionBehaviour : FilteredFlockBehavior
{
	Vector2 currentVelocity;
	public float agentSmoothTime = 0.5f;
	public override Vector2 CalulateMove(FlockAgent agent, List<Transform> context, Flock flock)
	{
		// if there are no nearby neighbors, return zero vector
		if(context.Count == 0)
		{
			return Vector2.zero;
		}

		Vector2 cohesionMove = Vector2.zero;
		foreach (Transform item in context)
		{
			cohesionMove += (Vector2)item.position;
		}

		cohesionMove /= context.Count;

		// create offset from agent
		cohesionMove -= (Vector2)agent.transform.position;
		cohesionMove = Vector2.SmoothDamp(agent.transform.up, cohesionMove, ref currentVelocity, agentSmoothTime);

		return cohesionMove;
	}
}
