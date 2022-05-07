using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FlockAgent : MonoBehaviour
{
    Collider2D agentCollider;
    public Collider2D AgentCollider { get { return agentCollider; } }

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.gameObject.CompareTag("Player"))
		{
			Debug.Log("Hit");
			Flock.instance.increaseHitCount();
		}
		
	}

	// Start is called before the first frame update
	void Start()
    {
        agentCollider = GetComponent<Collider2D>();
    }

	public void Move(Vector2 vel)
	{
        transform.up = vel;
        transform.position += (Vector3)vel * Time.deltaTime;
	}
}
