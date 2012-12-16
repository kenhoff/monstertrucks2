using UnityEngine;
using System.Collections;

public class SaveInputScript : MonoBehaviour {
	public GameObject changeNameInput;
	public GameObject changeNameLabel;
void OnClick()
	{
		PlayerPrefs.SetString("Player Name", changeNameLabel.GetComponent<UIBaseLabel>().text);
		changeNameInput.SetActive(false);
	}
}
