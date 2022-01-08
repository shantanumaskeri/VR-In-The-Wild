using UnityEngine;
using System.Collections;

public class Player_CharacterController : MonoBehaviour 
{

	public float speed = 8.0f;
	public float gravity = 20.0f;

	private Vector3 moveDirection = Vector3.zero;
	private CharacterController controller;
	private Camera camera;
	private bool isLookingDown;
	private bool isLookingUp;
	private bool isWalking;

	private void Start ()
	{
		controller = GetComponent<CharacterController>();
		camera = Camera.main;

		isLookingDown = false;
		isLookingUp = false;
		isWalking = true;
	}

	private void FixedUpdate () 
	{
		if (camera.transform.localRotation.x < -0.2f)
		{
			isLookingUp = true;
		}
		else if (camera.transform.localRotation.x > 0.2f)
		{
			isLookingDown = true;
		}
		else
		{
			isLookingUp = false;
			isLookingDown = false;
		}

		if (!isLookingDown && !isLookingUp)
		{
			isWalking = true;
		}
		else
		{
			isWalking = false;
		}

		if (isWalking)
		{
			if (controller.isGrounded) 
			{
				moveDirection = transform.TransformDirection (camera.transform.forward);
				moveDirection *= speed;
			}

			moveDirection.y -= gravity * Time.deltaTime;
			controller.Move (moveDirection * Time.deltaTime);
		}
	}

	private void OnControllerColliderHit (ControllerColliderHit hit)
	{
		if (hit.collider.gameObject.tag == "NPC")
		{
			speed = 0.0f;
		}
		else
		{
			speed = 8.0f;
		}
	}
}