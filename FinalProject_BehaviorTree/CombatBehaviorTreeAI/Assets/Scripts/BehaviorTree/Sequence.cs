using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : Node
{
	protected List<Node> nodeList = new List<Node>();

	public Sequence(List<Node> nodeList)
	{
		this.nodeList = nodeList;
	}
	public override NodeState Evaluate()
	{
		bool isAnyNodeRunning = false;
		foreach(Node node in nodeList)
		{
			switch(node.Evaluate())
			{
				case NodeState.RUNNING:
					isAnyNodeRunning = true;
					break;
				case NodeState.SUCCESS:
					break;
				case NodeState.FAILURE:
					nodeState = NodeState.FAILURE;
					return nodeState;
				default:
					break;
			}
		}
		nodeState = isAnyNodeRunning ? NodeState.RUNNING : NodeState.SUCCESS;
		return nodeState;
	}
}
