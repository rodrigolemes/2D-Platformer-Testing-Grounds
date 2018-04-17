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
	public float wallStickTime = .25f;
	float timeToWallUnstick;
	public Vector2 wallJumpClimb;
	public Vector2 wallJumpOff;
	public Vector2 wallLeap;
	
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
		Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		int wallDirX = (controller.collisions.left) ? -1 : 1;

		float targetVelocityX = input.x * moveSpeed;
		velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below)?accelerationTimeGrounded:accelerationTimeAirborne); 

		bool wallSliding = false;
		if ((controller.collisions.right || controller.collisions.left) && !controller.collisions.below && velocity.y < 0) {
			wallSliding = true;

			if (velocity.y < -wallSlidingSpeedMax) {
				velocity.y = -wallSlidingSpeedMax;
			}

			if (timeToWallUnstick > 0) {
				velocityXSmoothing = 0;
				velocity.x = 0;

				if (input.x != wallDirX && input.x != 0) {
					timeToWallUnstick -= Time.deltaTime;
				} 
				else {
					timeToWallUnstick = wallStickTime;
				}
			}
			else {
				timeToWallUnstick = wallStickTime;
			}
		}

		if (controller.collisions.above || controller.collisions.below) {
			velocity.y = 0;
		}


		if (Input.GetKeyDown(KeyCode.Space)) {
			if (wallSliding) {
				if (wallDirX == input.x) { //jumping in the same direction of the wall you're climbing
					velocity.x = - wallDirX * wallJumpClimb.x;
					velocity.y = wallJumpClimb.y;
				}
				else if (input.x == 0) { //no input, just jumping
					velocity.x = -wallDirX * wallJumpOff.x;
					velocity.y = wallJumpOff.y;
				}
				else { //input opposite to our wall direction
					velocity.x = -wallDirX * wallLeap.x;
					velocity.y = wallLeap.y;
				}
			}
			if (controller.collisions.below) {
				velocity.y = jumpVelocity;
			}
		}
		//my code, will it break something?
		if (Input.GetKeyUp(KeyCode.Space) && velocity.y > 0) {
			velocity.y = 0;
		}

		velocity.y += gravity * Time.deltaTime;
		controller.Move(velocity * Time.deltaTime);
	}
}
