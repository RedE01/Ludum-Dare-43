
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public float walkingSpeed;
	public float jumpHeight;
	[HideInInspector]
	public bool isGrounded;
	[HideInInspector]
	public bool isHolding;
	[HideInInspector]
	public bool objectGrabbedIsStatic = false;
	[HideInInspector]
	public bool objectGrabbedIsPlayer = false;

	private PlayerHandler playerHandler;
	private bool isSelected;
	private float speed;
	private Rigidbody2D rb2d;
	private int playerId;
	private SpriteRenderer selectArrowRenderer;
	private float yVel;
	private HingeJoint2D joint;
	private float movement = 0.0f;
	private float lastPush = 0.0f;
	private float movementSign = 0.0f;
	private bool collidingWithWall = false;

	void Start() {
		speed = walkingSpeed;
		rb2d = GetComponent<Rigidbody2D>();
		playerHandler = GameObject.FindGameObjectWithTag("PlayerHandler").GetComponent<PlayerHandler>();
		selectArrowRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
		joint = GetComponent<HingeJoint2D>();
		joint.enabled = false;

		playerId = playerHandler.AddToPlayerList(this);
	}

	void FixedUpdate() {
		if (isSelected) {
			movement = Input.GetAxis("Horizontal") * speed;
		}
		else{
			movement = 0;
		}
		if ((isGrounded || isHolding) && yVel < 0) yVel = 0;

		movementSign = Mathf.Sign(movement);
		float swingSpeed = objectGrabbedIsPlayer ? 1.75f : 4.0f;

		if (!isHolding || (isHolding && objectGrabbedIsStatic && isSelected)) {
			if ((isHolding && objectGrabbedIsStatic && isSelected)) movement *= 0.55f;

			rb2d.velocity = new Vector2(movement, yVel);
		}
		else if (movementSign != lastPush && isSelected && Mathf.Abs(movement) > 0.5f) {
			lastPush = movementSign;
			rb2d.AddForce(Vector2.right * movementSign * swingSpeed, ForceMode2D.Impulse);
		}
	}

	void Update() {
		if (isSelected) {
			if (Input.GetKeyDown(KeyCode.Space) && isGrounded) {
				yVel = jumpHeight;
				isGrounded = false;
			}

			if (isHolding && !Input.GetKey(KeyCode.W)) {
				ReleaseGrab();
				yVel += jumpHeight * 0.8f;
			}
		}
		if (!isGrounded || !isHolding) {
			yVel -= 35.0f * Time.deltaTime;
		}
	}

	public void MakeSelected(bool enable) {
		isSelected = enable;

		selectArrowRenderer.enabled = enable;
	}

	void OnCollisionStay2D(Collision2D collision) {
		if (isSelected && !collision.gameObject.CompareTag("Player")) {
			Vector2 dir = collision.contacts[0].point - (Vector2)transform.position;

			if (dir.normalized.y > 0.90f) {
				yVel = yVel > 0.0f ? 0.0f : yVel;
			}
		}
		if(isSelected && collision.gameObject.CompareTag("Grabbable") && !isHolding) {
			Vector2 dir = collision.contacts[0].point - (Vector2)transform.position;

			if (dir.normalized.x >= 0.99f) {
				movement = 0.0f;
			}
		}
	}

	void OnMouseDown() {
		playerHandler.requestSelected(playerId);
	}

	public void ReleaseGrab() {
		isHolding = false;
		joint.enabled = false;
		rb2d.freezeRotation = true;
		rb2d.gravityScale = 0.0f;
		transform.rotation = Quaternion.Euler(Vector3.zero);
		lastPush = 0.0f;
	}
}
