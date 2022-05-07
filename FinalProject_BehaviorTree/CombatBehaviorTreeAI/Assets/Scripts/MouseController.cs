using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    public float mouseSensitivity = 100f;

    private Transform playerBody;

    float xRotation = 0f;
    public float damage = 10;
    public float range = 100f;
    public float fireRate = 15f;
    private float nextTimeToFire = 0f;

    private Camera fpsCam;
    // Start is called before the first frame update
    void Start()
    {
        playerBody = FindObjectOfType<PlayerController>().transform;
        fpsCam = FindObjectOfType<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        MouseMovement();

        if(Input.GetKey(KeyCode.Mouse0) && Time.time >= nextTimeToFire)
		{
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
		}
    }

    void MouseMovement()
	{
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    void Shoot()
	{
        RaycastHit hit;
        Debug.DrawRay(fpsCam.transform.position, fpsCam.transform.forward * range, Color.red);

        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward * range, out hit))
        {
            AIController target = hit.transform.GetComponent<AIController>();

            if(target != null)
			{
                target.TakeDamage(damage);
			}
		}
	}
}
