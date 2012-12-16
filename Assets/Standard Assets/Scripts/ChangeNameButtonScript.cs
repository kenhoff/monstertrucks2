using UnityEngine; 
using System.Collections;

public class ChangeNameButtonScript : MonoBehaviour {
	
public AudioClip Soundhover;
public AudioClip ClickSound;
public string title;
public string changeString;
public bool guiOn;

void OnStart(){
		changeString = PlayerPrefs.GetString(title);
	}

public void OnMouseEnter ()
{
    audio.PlayOneShot(Soundhover); 
	renderer.material.color = Color.red;
}

public void OnMouseExit ()
{
	renderer.material.color = Color.white;
}

public void OnMouseUp ()
	{
	renderer.material.color = Color.white;
		
    audio.PlayOneShot(ClickSound);
		guiOn = true;
		GameObject.Find("Main Camera").GetComponent<MouseLook>().enabled = false;
	
}

void OnGUI(){
		if(guiOn){
			 if (Event.current.Equals(Event.KeyboardEvent("return"))) {
				if(changeString.Length == 0)
					changeString = PlayerPrefs.GetString(title);
				PlayerPrefs.SetString(title, changeString);
				GameObject.Find("Main Camera").GetComponent<MouseLook>().enabled = true;
				guiOn = false;
			}
			if (Event.current.Equals(Event.KeyboardEvent("end"))) {
				GameObject.Find("Main Camera").GetComponent<MouseLook>().enabled = true;
				guiOn = false;
			}
			if (Event.current.Equals(Event.KeyboardEvent("home"))) {
				
//Player Specific Info
PlayerPrefs.SetString("Player Name", "Player");
PlayerPrefs.SetInt("Cash", 0);
PlayerPrefs.SetInt("Highest Score - Trick", 0);
PlayerPrefs.SetInt("Highest Score - Round", 0);
PlayerPrefs.SetInt("Total Score", 0);
PlayerPrefs.SetString("Driving Level", "Amateur");
		
//Player Truck Info
PlayerPrefs.SetString("Monster Truck Name", "Truck");
PlayerPrefs.SetInt("Speed", 27);
PlayerPrefs.SetInt("Handling", 27);
PlayerPrefs.SetInt("Durability", 27);
PlayerPrefs.SetInt("Weight", 42);
//Need to enumerate for color of truck
PlayerPrefs.SetInt("TruckColor", 0);
				GameObject.Find("Main Camera").GetComponent<MouseLook>().enabled = true;
				guiOn = false;
			}
				
			GUI.SetNextControlName("TextField");
			changeString = GUI.TextField(new Rect(10,10,200,20),changeString, 32);
			GUI.FocusControl("TextField");
		}
}
}