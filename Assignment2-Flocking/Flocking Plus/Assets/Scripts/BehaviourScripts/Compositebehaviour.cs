using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Flock/Behaviour/Composit")]
public class Compositebehaviour : FlockBehaviour
{
	public FlockBehaviour[] behaviours;
	public float[] weights;
	public override Vector2 CalulateMove(FlockAgent agent, List<Transform> context, Flock flock)
	{
		// data mismatch
		if(weights.Length != behaviours.Length)
		{
			Debug.LogError("Data Mismatch in " + name, this);
			return Vector2.zero;
		}

		// setup move
		Vector2 move = Vector2.zero;

		for(int i = 0; i < behaviours.Length; ++i)
		{
			Vector2 partialMove = behaviours[i].CalulateMove(agent, context, flock) * weights[i];

			if(partialMove != Vector2.zero)
			{
				if(partialMove.sqrMagnitude > weights[i] * weights[i])
				{
					partialMove.Normalize();
					partialMove *= weights[i];
				}

				move += partialMove;
			}
		}

		return move;
	}
}
