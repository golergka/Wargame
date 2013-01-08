using UnityEngine;
using System.Collections;

public class DisableSpawner : MonoBehaviour {

	public Transform afterLife;

	bool quitting = false;

	void OnApplicationQuit() {
		quitting = true;
	}

	void OnDisable() {

		if (quitting)
			return;

		if (!Application.isPlaying)
			return;

		if (afterLife != null)
			Instantiate(afterLife, transform.position, transform.rotation);

	}

}
