using UnityEngine;
using System.Collections;

public class TimedDisabler : MonoBehaviour {

	public float lifeTime = 1f;

	IEnumerator SelfDestruct() {

		yield return new WaitForSeconds(lifeTime);
		gameObject.SetActive(false);

	}

	// Use this for initialization
	void Start () {

		StartCoroutine(SelfDestruct());
	
	}

}
