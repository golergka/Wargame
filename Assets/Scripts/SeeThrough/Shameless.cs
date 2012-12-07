using UnityEngine;
using System.Collections;

public class Shameless : MonoBehaviour {

	ShyObstacle shamed;
	
	// Update is called once per frame
	void Update () {

		RaycastHit hit;

		int layerMask = ~0;
		if ( !Physics.Linecast(Camera.main.transform.position, transform.position, out hit, layerMask) )
			return;

		ShyObstacle shy = hit.transform.GetComponent<ShyObstacle>();

		if (shamed == null) {
			
			if (shy == null)
				return;

		} else {

			if (shy == shamed)
				return;

			shamed.hidden = false;

		}

		shamed = shy;
		shy.hidden = true;
	
	}

}
