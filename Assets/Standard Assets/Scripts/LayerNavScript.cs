using UnityEngine;
using System.Collections;

public class LayerNavScript : MonoBehaviour {
	public GameObject[] enable;
	public GameObject[] disable;
	
		public void OnMouseDown()
	{
	GarageStateValues garagestatevalue = GameObject.Find ("GarageState").GetComponent<GarageStateValues> ();
		garagestatevalue.enableStack.Push(enable);
		garagestatevalue.disableStack.Push(disable);
				
	}
}