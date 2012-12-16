#pragma strict

function Start () {

}

public var CameraPosition : Transform;
public var truck : Transform;
public var smooth = 5.0;
private var camera_floor = 2;

function Update () {
    transform.position = Vector3.Lerp (transform.position, CameraPosition.position, Time.deltaTime * smooth);
    if (transform.position.y < camera_floor) {
    	transform.position.y = camera_floor;
    }
    transform.LookAt(truck);
}