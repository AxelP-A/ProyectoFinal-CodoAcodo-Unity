using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public CharacterController2D controller;
	public Animator animator;

	public float runSpeed = 40f;

	[SerializeField] float horizontalMove = 0f;
	bool jump = false;
	bool dash = false;

	float timer = 0.0f;
	[SerializeField]
	float footstepSpeed = 0.3f;
	
	// Update is called once per frame
	void Update () {

		horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
		animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

		if (horizontalMove != 0 && controller.m_Grounded)
        {       
            if (timer > footstepSpeed)
            {
                controller.SelectAndPlayFootstep();
                timer = 0.0f;
            }

            timer += Time.deltaTime;
        }

		if (Input.GetKeyDown(KeyCode.Space))
		{
			jump = true;
		}

		if (Input.GetKeyDown(KeyCode.Mouse1))
		{
			dash = true;
		}
	}

	public void OnFall()
	{
		animator.SetBool("IsJumping", true);
	}

	public void OnLanding()
	{
		animator.SetBool("IsJumping", false);
	}

	void FixedUpdate ()
	{
		controller.Move(horizontalMove * Time.fixedDeltaTime, jump, dash);
		jump = false;
		dash = false;
	}
}