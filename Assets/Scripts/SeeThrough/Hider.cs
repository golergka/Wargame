using UnityEngine;
using System.Collections;

public class Hider : MonoBehaviour {

	Hideable shamed; // the hideable we're hiding right now

	void HideNew(Hideable shy) {

		if (shy == null)
			return;

		shy.Hide();
		shamed = shy;

	}
	
	// Update is called once per frame
	void Update () {

		RaycastHit hit;

		int layerMask = ~0;
		if ( !Physics.Linecast(Camera.main.transform.position, transform.position, out hit, layerMask) )
			return;

		Debug.DrawLine(transform.position, hit.point);

		Hideable shy = hit.transform.GetComponent<Hideable>();

		if (shamed == null) {
			
			HideNew(shy);

		} else {

			if (shy == shamed)
				return; // we already hid it

			// we've got new one instead of old one!
			shamed.Unhide();
			shamed = null;

			HideNew(shy);

		}
	
	}

}
