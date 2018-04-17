using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour {

	public float jumpHeight = 4;
	public float timeToJumpApex = .4f; //of a second
	public float moveSpeed = 6;
	public float accelerationTimeAirborne = .2f;
	public float accelerationTimeGrounded = .1f;
	public float wallSlidingSpeedMax = 3;
	
	private float jumpVelocity;
	private float gravity;
	private Vector3 velocity;
	private Controller2D controller;
	private float velocityXSmoothing;

	void Start () {
		controller = GetComponent<Controller2D>();

		gravity = -(2*jumpHeight)/Mathf.Pow(timeToJumpApex, 2);
		jumpVelocity = Mathf.Abs(gravity * timeToJumpApex);
		print ("Gravity: " + gravity + " Jump velocity: " + jumpVelocity);
	}
	
	// Update is called once per frame
	void Update () {

		bool wallSliding = false;
		if ((controller.collisions.right || controller.collisions.left) && !controller.collisions.below && velocity.y < 0) {
			wallSliding = true;

			if (velocity.y < -wallSlidingSpeedMax) {
				velocity.y = -wallSlidingSpeedMax;
			}
		}

		if (controller.collisions.above || controller.collisions.below) {
			velocity.y = 0;
		}

		Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

		if (Input.GetKeyDown(KeyCode.Space) && controller.collisions.below) {
			velocity.y = jumpVelocity;
		}
		//my code, will it break something?
		if (Input.GetKeyUp(KeyCode.Space) && velocity.y > 0) {
			velocity.y = 0;
		}

		float targetVelocityX = input.x * moveSpeed;
		velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below)?accelerationTimeGrounded:accelerationTimeAirborne); 
		velocity.y += gravity * Time.deltaTime;
		controller.Move(velocity * Time.deltaTime);
	}
}
