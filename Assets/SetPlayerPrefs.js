#pragma strict

// Durability	Speed	Handling	Weight	Style

// private var attributes = ["Durability", "Speed", "Handling", "Weight", "Style"];
public var Durability : float;
public var Speed : float;
public var Handling : float;
public var Weight : float;
public var Style : float;

function Start () {
	PlayerPrefs.SetFloat("Durability", Durability);
	PlayerPrefs.SetFloat("Speed", Speed);
	PlayerPrefs.SetFloat("Handling", Handling);
	PlayerPrefs.SetFloat("Weight", Weight);
	PlayerPrefs.SetFloat("Style", Style);
	PlayerPrefs.Save();
}

function Update () {

}