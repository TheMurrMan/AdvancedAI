using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsCoveredNode : Node
{
	private Transform target;
	private Transform origin;
	private AIController ai;

	public IsCoveredNode(Transform target, Transform origin, AIController ai)
	{
		this.target = target;
		this.origin = origin;
		this.ai = ai;
	}

	public override NodeState Evaluate()
	{
		RaycastHit hit;

		if(Physics.Raycast(origin.position, target.position - origin.position, out hit))
		{
			if(hit.collider.transform != target)
			{
				Debug.Log("Healing");
				ai.isBehindCover = true;
				return NodeState.SUCCESS;
			}
		}
		ai.isBehindCover = false;
		return NodeState.FAILURE;
	}
}
