#pragma strict

private var PointsTotal : float = 0;
public var AirMultiplier : float = 1;
public var MovesMultiplier : float = 1;
public var SpeedMultiplier : float = 1;
public var ResetMultiplier : float = 1;

private var tracking : boolean;

private var mask_out_player = ~(1 << 9);

function Start () {
	PointsTotal = 0;
}

function FixedUpdate () {
	//Debug.DrawRay(transform.position, -Vector3.up*3);
	var on_ground = Physics.Raycast(transform.position, -Vector3.up, 3, mask_out_player);
	if (!on_ground) {
		if (tracking == false) StartTrackingAir();
		if (tracking == true) TrackAir();
	}
	if (on_ground && (tracking == true)) {
		StopTrackingAir();
	}

	// speed

	PointsTotal += SpeedMultiplier * Time.deltaTime * rigidbody.velocity.magnitude;

	Debug.Log(PointsTotal);
}

function StartTrackingAir() {
	tracking = true;
}

function TrackAir() {
	var hit : RaycastHit;
	Physics.Raycast(transform.position, -Vector3.up, hit);
	PointsTotal += AirMultiplier * Time.deltaTime * hit.distance;
	PointsTotal += MovesMultiplier * Time.deltaTime * rigidbody.angularVelocity.magnitude;
}

function StopTrackingAir() {
	tracking = false;
}

function EndLevel() {
	PlayerPrefs.SetFloat("LastLevelPoints", PointsTotal);
	PlayerPrefs.Save();
}