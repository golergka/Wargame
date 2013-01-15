using UnityEngine;
using System.Collections;

public class RotationLock : MonoBehaviour {

	Quaternion startRotation;

	void Start() {

		startRotation = transform.rotation;

	}
	
	// Update is called once per frame
	void Update () {

		transform.rotation = startRotation;
	
	}
}
