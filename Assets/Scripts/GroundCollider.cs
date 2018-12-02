using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCollider : MonoBehaviour {

	private Player parentPlayer;

	void Start() {
		parentPlayer = transform.parent.GetComponent<Player>();
	}

	void OnTriggerStay2D(Collider2D collision) {
		if (!collision.isTrigger) {
			parentPlayer.isGrounded = true;
		}
	}

	void OnTriggerExit2D(Collider2D collision) {
		if (!collision.isTrigger) {
			parentPlayer.isGrounded = false;
		}
	}
}
