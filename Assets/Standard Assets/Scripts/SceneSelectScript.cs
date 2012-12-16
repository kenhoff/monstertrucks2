using UnityEngine; 
using System.Collections;

public class SceneSelectScript : MonoBehaviour {

public string levelToLoad;
public AudioClip Soundhover;
public AudioClip ClickSound;
public bool QuitButton = false;

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
    if(QuitButton)
    {
       Application.Quit();   
    }

    else
    {
      Application.LoadLevel(levelToLoad);   
    }
    }
}