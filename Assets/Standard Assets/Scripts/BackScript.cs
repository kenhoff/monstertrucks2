using UnityEngine; 
using System.Collections;

public class BackScript : MonoBehaviour {
	
public string levelToLoad;
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
    audio.PlayOneShot(ClickSound);
	GarageStateValues garagestatevalue = GameObject.Find ("GarageState").GetComponent<GarageStateValues> ();
		if(garagestatevalue.enableStack.Count == 0)
		{
			Application.LoadLevel(levelToLoad);
		}
		else{
	disable = garagestatevalue.enableStack.Pop() as GameObject[];
	enable = garagestatevalue.disableStack.Pop() as GameObject[];
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
}