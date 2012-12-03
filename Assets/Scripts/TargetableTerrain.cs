using UnityEngine;
using System.Collections;

public class TargetableTerrain : MonoBehaviour {
	
	void Go() {
		
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		
		if (Physics.Raycast(ray, out hit)) {
			
			PlayerController.selectedCharacter.MoveToPosition(hit.point);
			
		}
		
	}
	
	void OnMouseDown() {
		
		Go ();
		
	}
	
	void OnMouseDrag() {
		
		Go ();
		
	}
	
}
