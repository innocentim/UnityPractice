using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {
	enum Orientation {
		LEFT,
		RIGHT,
	};

	public float maximumSpeed;
	public float jumpFactor;
	private Orientation orientation;
	private float lastSpeedInput = 0f;
	private bool onTheGround {
		get {
			return -0.01 < transform.position.y && transform.position.y < 0.01;
		}
	}

	// Use this for initialization
	void Start () {
		orientation = Orientation.RIGHT;
		foreach (AnimationState state in animation) {
			state.wrapMode = WrapMode.Loop;
			state.weight = 0;
			state.enabled = true;
		}
	}

	// Update is called once per frame
	void Update () {
		HandleInput ();
		UpdateTransientRotate ();
		UpdateTranslation ();
		UpdateAnimation ();
		KeepAboveGround ();
	}

	void HandleInput() {
		if (Input.GetAxis("Horizontal") > 0) {
			orientation = Orientation.RIGHT;
		} else if (Input.GetAxis("Horizontal") < 0) {
			orientation = Orientation.LEFT;
		}
		if (Input.GetButtonDown ("Jump")) {
			HandleJump();
		}
	}

	void HandleJump() {
		if (onTheGround) {
			rigidbody.velocity += Vector3.up * jumpFactor;
		}
	}

	void UpdateTransientRotate() {
		transform.rotation = Quaternion.Slerp (
			transform.rotation,
			Quaternion.Euler(Vector3.up * (orientation == Orientation.RIGHT ? 90f : 270f)),
			Time.deltaTime * 10);
	}

	void UpdateTranslation() {
		if (onTheGround) {
			rigidbody.velocity += Vector3.right * (Input.GetAxis("Horizontal") - lastSpeedInput) * maximumSpeed;
			lastSpeedInput = Input.GetAxis("Horizontal");
		}
	}

	void UpdateAnimation() {
		animation["idle"].weight = 1 - Mathf.Abs (rigidbody.velocity.x);
		animation["run"].weight = Mathf.Abs (rigidbody.velocity.x);
	}

	void KeepAboveGround() {
		if (transform.position.y < 0 && rigidbody.velocity.y < 0) {
			var v = Vector3.zero;
			v.x = transform.position.x;
			v.z = transform.position.z;
			transform.position = v;
			v.x = rigidbody.velocity.x;
			v.z = rigidbody.velocity.z;
			rigidbody.velocity = v;
		}
	}
}
