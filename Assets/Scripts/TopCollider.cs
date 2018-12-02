using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopCollider : MonoBehaviour {

	private Player parentPlayer;
	private HingeJoint2D parentJoint;
	private Rigidbody2D parentrb2d;

	void Start() {
		parentPlayer = transform.parent.GetComponent<Player>();
		parentJoint = parentPlayer.GetComponent<HingeJoint2D>();
		parentrb2d = parentPlayer.GetComponent<Rigidbody2D>();
	}

	void OnTriggerStay2D(Collider2D collision) {
		bool otherIsHolding = false;
		GameObject other = collision.gameObject;
		if (other.CompareTag("Player")) {
			if(other.GetComponent<HingeJoint2D>().enabled) {
				otherIsHolding = true;
				parentPlayer.objectGrabbedIsPlayer = true;
			}
			else if(parentPlayer.isHolding) {
				parentPlayer.ReleaseGrab();
			}
		}
		if (otherIsHolding || other.CompareTag("Grabbable")) {
			if(!otherIsHolding) parentPlayer.objectGrabbedIsPlayer = false;
			if (Input.GetKey(KeyCode.W)) {
				parentPlayer.isHolding = true;
				parentJoint.enabled = true;
				parentrb2d.freezeRotation = false;
				parentrb2d.gravityScale = 5.0f;
				Rigidbody2D otherRB = other.GetComponent<Rigidbody2D>();
				if(otherRB != null) {
					parentJoint.connectedBody = otherRB;
					parentPlayer.objectGrabbedIsStatic = false;
				}
				else {
					parentJoint.connectedBody = null;
					parentPlayer.objectGrabbedIsStatic = true;
				}
			}
		}
	}

	//void OnTriggerExit2D(Collider2D collision) {
	//	if (!collision.isTrigger) {
	//		parentPlayer.isHolding = false;
	//	}
	//}
}
