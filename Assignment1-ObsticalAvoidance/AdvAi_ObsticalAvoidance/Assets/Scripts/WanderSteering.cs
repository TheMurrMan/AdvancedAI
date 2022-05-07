using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderSteering : MonoBehaviour
{
    // Ray Stuff
    public float numberOfRays = 0f;
    public float angle = 0f;
    public float rayLength = 0f;

    // Wander Stuff
    Color hitColor = new Color(128, 0, 128);
    public float moveSpeed = 3.5f;
    public float rotateSpeed = 3f;

    private bool isWandering = false;
    private bool isRotatingLeft = false;
    private bool isRotatingRight = false;
    private bool isWalking = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 deltaPos = Vector3.zero;
        for (int i = 0; i < numberOfRays; ++i)
        {
            Quaternion rotationMod = Quaternion.AngleAxis(i / (numberOfRays - 1) * angle * 2 - angle, transform.up);
            Vector3 dir = transform.rotation * rotationMod * Vector3.forward;

            Ray ray = new Ray(transform.position, dir);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo, rayLength))
            {
                deltaPos -= (1f / numberOfRays) * moveSpeed * dir;
                transform.Rotate(Vector3.up * rotateSpeed);
            }

            else
            {
                deltaPos += (1f / numberOfRays) * moveSpeed * dir;
            }
        }
        transform.Translate(transform.forward * Time.deltaTime * moveSpeed);
        //transform.Rotate(Vector3.up * rotateSpeed);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = hitColor;
        for (int i = 0; i < numberOfRays; ++i)
        {
            Quaternion rotationMod = Quaternion.AngleAxis(i / (numberOfRays - 1) * angle * 2 - angle, transform.up);
            Vector3 dir = transform.rotation * rotationMod * Vector3.forward;
            Gizmos.DrawRay(transform.position, dir * rayLength);
        }
    }


    public void UpdateWander(Vector3 pos)
    {
        if (isWandering == false)
        {
            StartCoroutine(Wander());
        }

        if (isRotatingRight == true)
        {
            transform.Rotate(transform.up * Time.deltaTime * rotateSpeed);
        }

        if (isRotatingLeft == true)
        {
            transform.Rotate(transform.up * Time.deltaTime * -rotateSpeed);
        }

        if (isWalking == true)
        {
            transform.Translate(pos * Time.deltaTime * moveSpeed);
        }
    }

    IEnumerator Wander()
    {
        int rotateTime = Random.Range(1, 3);
        int rotateLeftOrRight = Random.Range(1, 3);
        int walkTime = 2;

        isWandering = true;

        isWalking = true;
        yield return new WaitForSeconds(walkTime);

        isWalking = false;

        if (rotateLeftOrRight == 1)
        {
            isRotatingRight = true;
            yield return new WaitForSeconds(rotateTime);
            isRotatingRight = false;
        }

        if (rotateLeftOrRight == 2)
        {
            isRotatingLeft = true;
            yield return new WaitForSeconds(rotateTime);
            isRotatingLeft = false;
        }

        isWandering = false;
    }
}
