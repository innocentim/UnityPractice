using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {
	enum Orientation {
		LEFT,
		RIGHT,
	};

	private Orientation orientation;

	// Use this for initialization
	void Start () {
		orientation = Orientation.RIGHT;
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
		UpdateAnimation ();
	}

	void UpdateTransientRotate() {
		transform.rotation = Quaternion.Slerp (
			transform.rotation,
			orientation == Orientation.RIGHT ? Quaternion.Euler(Vector3.up * 90f) : Quaternion.Euler(Vector3.up * 270f),
			Time.deltaTime * 10);
	}

	void UpdateAnimation() {
	}
}
