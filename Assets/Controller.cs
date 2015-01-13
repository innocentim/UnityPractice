using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {
	enum Orientation {
		LEFT,
		RIGHT,
	};

	public float speedFactor;
	private Orientation orientation;
	private float moveSpeed {
		get {
			return Input.GetAxis ("Horizontal");
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

	void HandleInput() {
		if (Input.GetAxis("Horizontal") > 0) {
			orientation = Orientation.RIGHT;
		} else if (Input.GetAxis("Horizontal") < 0) {
			orientation = Orientation.LEFT;
		}
	}

	// Update is called once per frame
	void Update () {
		HandleInput ();
		UpdateTransientRotate ();
		UpdateTranslation ();
		UpdateAnimation ();
	}

	void UpdateTransientRotate() {
		transform.rotation = Quaternion.Slerp (
			transform.rotation,
			Quaternion.Euler(Vector3.up * (orientation == Orientation.RIGHT ? 90f : 270f)),
			Time.deltaTime * 10);
	}

	void UpdateTranslation() {
		transform.Translate (Vector3.right * Time.deltaTime * moveSpeed * speedFactor, Space.World);
	}

	void UpdateAnimation() {
		animation["idle"].weight = 1 - Mathf.Abs (moveSpeed);
		animation["run"].weight = Mathf.Abs (moveSpeed);
	}
}
