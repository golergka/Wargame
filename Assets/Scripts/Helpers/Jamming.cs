using UnityEngine;
using System.Collections.Generic;

public class Jamming : MonoBehaviour {

	bool jammed = false;

	public List<Transform> exceptions = new List<Transform>();

	void OnCollisionEnter(Collision collision) {

		if (jammed)
			return;

		if (exceptions.Contains(collision.transform))
			return;

		jammed = true;

		if (rigidbody != null)
			rigidbody.isKinematic = true;

		transform.parent = collision.transform;

	}

	void OnTriggerEnter(Collider other) {

		if (!collider.isTrigger)
			return;

		if (jammed)
			return;

		if (exceptions.Contains(other.transform))
			return;

		jammed = true;

		if (rigidbody != null)
			rigidbody.isKinematic = true;

		transform.parent = other.transform;

	}

}
