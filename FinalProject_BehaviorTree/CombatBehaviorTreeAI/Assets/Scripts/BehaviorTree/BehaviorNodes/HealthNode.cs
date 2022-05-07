using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthNode : Node
{
	private AIController ai;
	private float healthThreshold;

	public HealthNode(AIController ai, float healthThreshold)
	{
		this.ai = ai;
		this.healthThreshold = healthThreshold;
	}

	public override NodeState Evaluate()
	{
		return ai.CurrentHealth <= healthThreshold ? NodeState.SUCCESS: NodeState.FAILURE;
	}
}
