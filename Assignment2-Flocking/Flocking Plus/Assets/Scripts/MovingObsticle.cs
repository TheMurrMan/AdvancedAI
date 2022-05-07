using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObsticle : MonoBehaviour
{
    float moveSpeed = 3f;
    bool moveRight = true;
    bool moveUp = true;

    public bool isMovingRightToLeft; 
    //public Transform pointA;
    //public Transform pointB;
    // Update is called once per frame
    void Update()
    {
        if (isMovingRightToLeft)
        {
            moveRightToLeft();
        }

        else
            moveUpAndDown();
    }

    void moveRightToLeft()
	{
        if (transform.position.x > 10)
        {
            moveRight = false;
        }

        if (transform.position.x < -10)
        {
            moveRight = true;
        }

        if (moveRight)
        {
            transform.position = new Vector2(transform.position.x + moveSpeed * Time.deltaTime, transform.position.y);
        }

        else
        {
            transform.position = new Vector2(transform.position.x - moveSpeed * Time.deltaTime, transform.position.y);
        }
    }

    void moveUpAndDown()
    {
        if (transform.position.y > 4)
        {
            moveUp = false;
        }

        if (transform.position.y < -4)
        {
            moveUp = true;
        }

        if (moveUp)
        {
            transform.position = new Vector2(transform.position.x , transform.position.y + moveSpeed * Time.deltaTime);
        }

        else
        {
            transform.position = new Vector2(transform.position.x, transform.position.y - moveSpeed * Time.deltaTime);
        }
    }
}
