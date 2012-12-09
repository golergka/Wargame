using UnityEngine;
using System.Collections;

public class DisableSpawner : MonoBehaviour {

	public Transform afterLife;

	void OnDisable() {

		if (!Application.isPlaying)
			return;

		if (afterLife != null)
			Instantiate(afterLife, transform.position, transform.rotation);

	}

}
