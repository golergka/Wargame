using UnityEngine;
using System.Collections;

public class TargetableTerrain : MonoBehaviour {
	
	void Go() {
	
		Debug.Log("Go!");
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		
		if (Physics.Raycast(ray, out hit)) {
			
			PartyController.instance.leader.Move(hit.point);
			
		}
		
	}
	
	void OnMouseDown() {
		
		Go ();
		
	}
	
	void OnMouseDrag() {
		
		Go ();
		
	}
	
}
