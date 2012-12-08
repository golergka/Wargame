using UnityEngine;
using System.Collections;

public class TargetableTerrain : MonoBehaviour {

	public Transform mousePointer;

	void Start() {

		if ( mousePointer == null )
			Debug.LogWarning("No mouse pointer object defined!");

	}

	void Update() {

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		int layerMask = 1 << 11; // terrain layer ONLY

		if ( Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask) ) {

			mousePointer.transform.position = hit.point;

		}

	}
	
	void Go() {
			
		PartyController.instance.leader.Move(mousePointer.transform.position);
		
	}
	
	void OnMouseDown() {
		
		Go ();
		
	}
	
	void OnMouseDrag() {
		
		Go ();
		
	}
	
}
