using UnityEngine;
using System.Collections;

public class TimedDisabler : MonoBehaviour {

	public float lifeTime = 1f;

	float startTime;

	// Use this for initialization
	void Start () {

		startTime = Time.time;
	
	}

	void Update() {

		// This is better than coroutine becase it allowes to change lifeTime in runtime
		if (Time.time - startTime >= lifeTime)
			gameObject.SetActive(false);

	}

}
