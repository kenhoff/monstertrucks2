using UnityEngine; 
using System.Collections;

public class UpgradeScript : MonoBehaviour {
	
public AudioClip Soundhover;
public AudioClip ClickSound;
public GameObject[] disable;
public GameObject[] enable;


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
GarageStateValues garagestatevalue = GameObject.Find ("GarageState").GetComponent<GarageStateValues> ();
	disable = garagestatevalue.disableStack.Peek() as GameObject[];
	enable = garagestatevalue.enableStack.Peek() as GameObject[];
	renderer.material.color = Color.white;
		
    audio.PlayOneShot(ClickSound);
	
	foreach ( GameObject i in disable){
			i.SetActive(false);
		}
	foreach ( GameObject i in enable){
			Debug.Log(i.name.ToString());
			i.SetActive(true);
			//add enable gameobjects[] to "back" disable gameobjects[]
		}
}

}