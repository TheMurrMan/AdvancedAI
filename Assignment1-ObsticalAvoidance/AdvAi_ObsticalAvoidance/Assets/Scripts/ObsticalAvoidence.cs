using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObsticalAvoidence : MonoBehaviour
{
	// Ray Stuff
	public float maxSteerAngle = 45f;
	public float rayLength = 3f;
	public float frontRayLegnth = 5f;
	public Vector3 frontRayPosition = new Vector3(0f, 0.2f, 0.5f);
	public Transform point;
	public float frontSideRayPosition = 0.4f;
	public float frontRayAngle = 30f;
	private bool avoiding = false;

	public Text hitCountText;
	public int hitCount = 0;

	public float moveSpeed = 0f;

	private void FixedUpdate()
	{
		Rays();

		hitCountText.text = "Hit Count: " + hitCount;
	}

	private void OnCollisionEnter(Collision collision)
	{
		if(collision.collider.CompareTag("Obstacle") || collision.collider.CompareTag("Agent"))
		{
			hitCount++;
		}
	}
	
	private void Rays()
	{
		RaycastHit hit;
		Vector3 RayStartPos = transform.position;
		RayStartPos += transform.forward * frontRayPosition.z;
		RayStartPos += transform.up * frontRayPosition.y;
		float avoidMultiplier = 0;
		avoiding = false;

		transform.Translate(transform.forward * moveSpeed);
		//front right Ray
		RayStartPos += transform.right * frontSideRayPosition;
		if (Physics.Raycast(RayStartPos, transform.forward, out hit, rayLength))
		{
			if (hit.collider.CompareTag("Obstacle") || hit.collider.CompareTag("Agent"))
			{
				Debug.DrawLine(RayStartPos, hit.point);
				avoiding = true;
				avoidMultiplier -= 1f;
			}

		}

		//front right angle Ray
		else if (Physics.Raycast(RayStartPos, Quaternion.AngleAxis(frontRayAngle, transform.up) * transform.forward, out hit, rayLength))
		{
			if (hit.collider.CompareTag("Obstacle") || hit.collider.CompareTag("Agent"))
			{
				Debug.DrawLine(RayStartPos, hit.point);
				avoiding = true;
				avoidMultiplier -= 0.5f;
			}
		}

		//front left Ray
		RayStartPos -= transform.right * frontSideRayPosition * 2;
		if (Physics.Raycast(RayStartPos, transform.forward, out hit, rayLength))
		{
			if (hit.collider.CompareTag("Obstacle") || hit.collider.CompareTag("Agent"))
			{
				Debug.DrawLine(RayStartPos, hit.point);
				avoiding = true;
				avoidMultiplier += 1f;
			}
		}

		//front left angle Ray
		else if (Physics.Raycast(RayStartPos, Quaternion.AngleAxis(-frontRayAngle, transform.up) * transform.forward, out hit, rayLength))
		{
			if (hit.collider.CompareTag("Obstacle") || hit.collider.CompareTag("Agent"))
			{
				Debug.DrawLine(RayStartPos, hit.point);
				avoiding = true;
				avoidMultiplier += 0.5f;
			}
		}

		//front center Ray
		if (avoidMultiplier == 0)
		{
			if (Physics.Raycast(RayStartPos, transform.forward, out hit, frontRayLegnth))
			{
				if (hit.collider.CompareTag("Obstacle") || hit.collider.CompareTag("Agent"))
				{
					Debug.DrawLine(RayStartPos, hit.point);
					avoiding = true;
					if (hit.normal.x < 0)
					{
						avoidMultiplier = -2;
					}
					else
					{
						avoidMultiplier = 2;
					}
				}
			}
		}

		if(avoidMultiplier > 2)
		{
			Debug.Log("Wall");
		}
		if (avoiding)
		{
			transform.rotation = Quaternion.AngleAxis(maxSteerAngle * avoidMultiplier, transform.up);
		}

	}

}
