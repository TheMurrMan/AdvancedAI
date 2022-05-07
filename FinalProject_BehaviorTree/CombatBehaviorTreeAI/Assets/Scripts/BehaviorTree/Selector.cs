using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : Node
{
	protected List<Node> nodeList = new List<Node>();

	public Selector(List<Node> nodeList)
	{
		this.nodeList = nodeList;
	}
	public override NodeState Evaluate()
	{
		foreach (Node node in nodeList)
		{
			switch (node.Evaluate())
			{
				case NodeState.RUNNING:
					nodeState = NodeState.RUNNING;
					return nodeState;
				case NodeState.SUCCESS:
					nodeState = NodeState.SUCCESS;
					return nodeState;
				case NodeState.FAILURE:
					break;
				default:
					break;
			}
		}
		nodeState = NodeState.FAILURE;
		return nodeState;
	}
}
