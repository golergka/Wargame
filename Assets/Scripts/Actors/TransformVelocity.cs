using UnityEngine;
using System.Collections;

public class TransformVelocity : MonoBehaviour {

	const float MIN_DELTA_TIME = 0.5f;

	public Vector3 velocity { get; private set; }

	Vector3 previousPosition = new Vector3(0f,0f,0f);
	float previousTime = 0f;
	
	// Update is called once per frame
	void Update () {

		if (Time.time - previousTime > MIN_DELTA_TIME) {

			velocity = (transform.position - previousPosition) * (Time.time - previousTime);

			previousTime = Time.time;
			previousPosition = transform.position;

		}
	
	}

	void OnDrawGizmosSelected() {

		Gizmos.color = Color.white;
		Gizmos.DrawLine(transform.position, transform.position + velocity);

	}

}
