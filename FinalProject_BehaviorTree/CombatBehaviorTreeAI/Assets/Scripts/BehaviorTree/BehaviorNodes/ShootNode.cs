using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class ShootNode : Node
{
	private NavMeshAgent agent;
	private AIController ai;
	private Transform target;

	private Vector3 currentVelocity;
	private float smoothDamp;
	private int damage = 2;
	public float fireRate = 5f;
	public float nextTimeToFire = 0f;
	// Start is called before the first frame update
	public ShootNode(NavMeshAgent agent, AIController ai, Transform target)
	{
		this.agent = agent;
		this.ai = ai;
		this.target = target;
		smoothDamp = 1f;
	}

	public override NodeState Evaluate()
	{
		// Shoot at player
		agent.isStopped = true;
		ai.SetColor(Color.green);

		Vector3 direction = target.position - ai.transform.position;
		Vector3 currentDirection = Vector3.SmoothDamp(ai.transform.forward, direction, ref currentVelocity, smoothDamp);
		Quaternion rotation = Quaternion.LookRotation(currentDirection, Vector3.up);
		ai.transform.rotation = rotation;

		if (Time.time >= nextTimeToFire)
		{
			nextTimeToFire = Time.time + 1f / fireRate;
			Shoot();
		}
		return NodeState.RUNNING;
	}

	void Shoot()
	{
		RaycastHit hit;
		if (Physics.Raycast(ai.transform.position, ai.transform.forward, out hit))
		{
			Debug.DrawLine(ai.transform.position, ai.transform.forward, Color.red);
			PlayerController target = hit.transform.GetComponent<PlayerController>();

			if (target != null)
			{
				target.TakeDamage(damage);
			}
		}
	}
}
