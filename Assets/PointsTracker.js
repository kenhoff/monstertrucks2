#pragma strict

private var PointsTotal : float = 0;
public var AirMultiplier : float = 1;

private var tracking : boolean;
private var AngleTraveled : Vector3 = Vector3.zero;

private var mask_out_player = ~(1 << 9);

function Start () {

	
	Debug.Log(mask_out_player);

}

function FixedUpdate () {
	Debug.DrawRay(transform.position, -Vector3.up*3);
	var on_ground = Physics.Raycast(transform.position, -Vector3.up, 3, mask_out_player);
	if (!on_ground) {
		PointsTotal += AirMultiplier * Time.deltaTime;
		if (tracking == false) StartTrackingTrick();
		if (tracking == true) TrackTrick();
	}
	if (on_ground && (tracking == true)) {
		StopTrackingTrick();
	}
	//Debug.Log(PointsTotal);
}

function StartTrackingTrick() {
	AngleTraveled = Vector3.zero;
	tracking = true;
	Debug.Log("Left Ground: Pitch: " + transform.eulerAngles.x + ", Roll: " + transform.eulerAngles.z + ", Heading: " + transform.eulerAngles.y);
}

function TrackTrick() {
	AngleTraveled += rigidbody.angularVelocity * Time.deltaTime;
}

function StopTrackingTrick() {
	//Debug.Log(AngleTraveled);
	Debug.Log("Hit Ground: Pitch: " + transform.eulerAngles.x + ", Roll: " + transform.eulerAngles.z + ", Heading: " + transform.eulerAngles.y);
	AngleTraveled = Vector3.zero;
	tracking = false;
}