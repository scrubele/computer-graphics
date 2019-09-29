using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car: MonoBehaviour {
	public CharacterController controller;
	public Vector3 inputVector;
	public float verticalVelocity;
	public float moveSpeed;
	public float jumpSpeed;
	public float jumpForce;
	public bool isGrounded;
	public float gravity;

	void Start() {
		print("I am a car");
		moveSpeed = 5f;
		gravity = 1.0f;
		jumpForce = 1.0f;
		controller = this.GetComponent < CharacterController > ();
	}

	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		if (hit.gameObject.tag == ("Ground") && isGrounded == false) {
			isGrounded = true;
		}
	}

	private void Update() {
		if (controller.isGrounded) {
			verticalVelocity -= gravity * Time.deltaTime;
			if (Input.GetKeyDown(KeyCode.Space)) {
				verticalVelocity = jumpForce;
			}
		}
		else {
			verticalVelocity -= gravity * Time.deltaTime;
		}

		inputVector = new Vector3(Input.GetAxis("Horizontal"), verticalVelocity, Input.GetAxis("Vertical"));
		inputVector *= moveSpeed;
		transform.LookAt(transform.position + new Vector3(inputVector.x, 0, inputVector.z));
		controller.Move(inputVector * Time.deltaTime);

	}
}