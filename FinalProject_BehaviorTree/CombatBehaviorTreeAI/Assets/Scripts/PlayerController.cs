using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private CharacterController cc;
    public float playerSpeed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    private float playerHealth = 100;
    private float maxHealth = 100;

    public float healthRestoreRate;
    Slider healthBar;
    public Transform groundCheck;
    public float groundDist = 0.4f;
    public LayerMask groundMask;
    Vector3 velocity;

    bool isGrounded = false;
    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        healthBar = GetComponentInChildren<Slider>();

        healthBar.value = playerHealth;
        healthBar.maxValue = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        UpdateHealth();

        if(playerHealth <= maxHealth)
		{
            playerHealth += Time.deltaTime * healthRestoreRate;
        }
        
    }

    private void UpdateHealth()
	{
        healthBar.value = playerHealth;
	}
	private void MovePlayer()
	{
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDist, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        cc.Move(move * playerSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        cc.Move(velocity * Time.deltaTime);
    }

	public void TakeDamage(int damage)
	{
        playerHealth -= damage;
	}
}
