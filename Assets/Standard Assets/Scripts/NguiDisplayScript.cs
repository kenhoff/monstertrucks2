using UnityEngine;
using System.Collections;

public class NguiDisplayScript : MonoBehaviour {
public string title;
public bool stringOrInt;
	
void Update(){
    if(stringOrInt){
			this.GetComponent<TextMesh>().text = title + ": "+ PlayerPrefs.GetString(title);
		}
	else{
			this.GetComponent<TextMesh>().text = title + ": "+ PlayerPrefs.GetInt(title);
		}
	}
}

