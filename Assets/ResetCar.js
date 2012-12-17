//#pragma strict

private var PointScript;

function Start () {
	PointScript = GetComponent("PointsTracker");
}

function Update () {
	if (Input.GetKeyDown(KeyCode.Return) && Physics.Raycast(transform.position, -Vector3.up, 2)) {
		rigidbody.active = false;
		transform.position += Vector3.up * 10;
		transform.eulerAngles = Vector3(0,  transform.eulerAngles.y, 0);
		rigidbody.active = true;
		rigidbody.velocity = rigidbody.velocity * 0;
		rigidbody.angularVelocity = rigidbody.angularVelocity* 0;
		PointScript.AddReset();
	}
	
	// Debug.Log(Physics.Raycast(transform.position, down, 10));
}