using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(ParameterManager))]
public class Jamming : MonoBehaviour {

	bool jammed = false;

	public List<Transform> exceptions = new List<Transform>();

	ParameterManager parameterManager;

	int damage {
		get {
			return parameterManager.GetParameterValue<int>(Attack.DAMAGE_KEY);
		}
	}

	void Awake() {

		parameterManager = GetComponent<ParameterManager>();

	}

	void Jam(Transform other) {

		if (jammed)
			return;

		if (exceptions.Contains(other))
			return;

		if (other.collider.isTrigger)
			return;

		jammed = true;

		if (rigidbody != null)
			rigidbody.isKinematic = true;

		Health health = other.GetComponent<Health>();
		if (health != null)
			health.InflictDamage(damage, this);

		transform.parent = other;

	}

	void OnCollisionEnter(Collision collision) {

		Jam(collision.transform);

	}

	void OnTriggerEnter(Collider other) {

		Jam(other.transform);

	}

}
