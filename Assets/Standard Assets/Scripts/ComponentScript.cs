using UnityEngine;
using System.Collections;

public class ComponentScript : MonoBehaviour {

public int partCost;
public int partSpeed;
public int partHandling;
public int partDurability;
public int partWeight;
public string name;
public bool purchased;
public bool active;

public AudioClip Soundhover;
public AudioClip ClickSound;
public AudioClip CashMachine;
public AudioClip Rejected;
	
ComponentScript[] componentScriptChildren;

public void Start()
	{
		CleanUp();
	}

public void OnMouseEnter ()
{
    audio.PlayOneShot(Soundhover); 
	renderer.material.color = Color.red;
}

public void CleanUp ()
{
		if(PlayerPrefs.GetInt(name+" Active") == 1)
			renderer.material.color = Color.green;
		else if(PlayerPrefs.GetInt(name+" Purchased") == 1)
			renderer.material.color = Color.blue;
		else
			renderer.material.color = Color.white;
}
	
public void OnMouseExit ()
	{
		CleanUp();
	}
	
public void OnMouseUp ()
	{
		componentScriptChildren = transform.parent.parent.GetComponentsInChildren<ComponentScript>();
			if(PlayerPrefs.GetInt(name+" Active") == 0)
		{ 
			if(PlayerPrefs.GetInt(name+" Purchased") == 0)
				
			{
				//Should throw in an "Are you Sure"
				if(PlayerPrefs.GetInt("Cash") >= partCost)
				{
					PlayerPrefs.SetInt("Cash", PlayerPrefs.GetInt("Cash") - partCost);
					foreach (ComponentScript i in componentScriptChildren)
					{
						if(PlayerPrefs.GetInt(i.name+" Active") == 1)
						{
							PlayerPrefs.SetInt( i.name+" Active", 0);
							PlayerPrefs.SetInt("Speed", PlayerPrefs.GetInt("Speed") - i.partSpeed);
							PlayerPrefs.SetInt("Handling", PlayerPrefs.GetInt("Handling") - i.partHandling);
							PlayerPrefs.SetInt("Durability", PlayerPrefs.GetInt("Durability") - i.partDurability);
							PlayerPrefs.SetInt("Weight", PlayerPrefs.GetInt("Weight") - i.partWeight);
						}
					}
					PlayerPrefs.SetInt(name+" Purchased", 1);
					PlayerPrefs.SetInt(name+" Active", 1);
					PlayerPrefs.SetInt("Speed", PlayerPrefs.GetInt("Speed") + partSpeed);
					PlayerPrefs.SetInt("Handling", PlayerPrefs.GetInt("Handling") + partHandling);
					PlayerPrefs.SetInt("Durability", PlayerPrefs.GetInt("Durability") + partDurability);
					PlayerPrefs.SetInt("Weight", PlayerPrefs.GetInt("Weight") + partWeight);
					audio.PlayOneShot(CashMachine);
				}
				else
				{
					Debug.Log("Not enough Money");
					audio.PlayOneShot(Rejected);
				}
			}
			else
			{
					foreach (ComponentScript i in componentScriptChildren)
					{
						if(PlayerPrefs.GetInt(i.name+" Active") == 1)
						{
							PlayerPrefs.SetInt( i.name+" Active", 0);
							PlayerPrefs.SetInt("Speed", PlayerPrefs.GetInt("Speed") - i.partSpeed);
							PlayerPrefs.SetInt("Handling", PlayerPrefs.GetInt("Handling") - i.partHandling);
							PlayerPrefs.SetInt("Durability", PlayerPrefs.GetInt("Durability") - i.partDurability);
							PlayerPrefs.SetInt("Weight", PlayerPrefs.GetInt("Weight") - i.partWeight);
						}
					}
					PlayerPrefs.SetInt(name+" Active", 1);
					PlayerPrefs.SetInt("Speed", PlayerPrefs.GetInt("Speed") + partSpeed);
					PlayerPrefs.SetInt("Handling", PlayerPrefs.GetInt("Handling") + partHandling);
					PlayerPrefs.SetInt("Durability", PlayerPrefs.GetInt("Durability") + partDurability);
					PlayerPrefs.SetInt("Weight", PlayerPrefs.GetInt("Weight") + partWeight);
					audio.PlayOneShot(ClickSound);
			}
		}
		else
			PlayerPrefs.SetInt(name+" Active", 0);
		foreach (ComponentScript i in componentScriptChildren)
			i.CleanUp();
}
}