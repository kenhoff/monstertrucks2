#pragma strict

public var force = 10000;

function Start () {

}

function FixedUpdate () {
	if (Input.GetKey(KeyCode.I)) {
		rigidbody.AddTorque(Vector3.right * force);
	}
	if (Input.GetKey(KeyCode.K)) {
		rigidbody.AddTorque(-Vector3.right * force);
	}
	if (Input.GetKey(KeyCode.J)) {
		rigidbody.AddTorque(Vector3.forward * force);
	}
	if (Input.GetKey(KeyCode.L)) {
		rigidbody.AddTorque(-Vector3.forward * force);
	}
}