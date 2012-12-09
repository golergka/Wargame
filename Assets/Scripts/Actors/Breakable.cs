using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Health))]
public class Breakable : MonoBehaviour {

	public float damageVelocityRatio = 1f;
	Health health;

	// Use this for initialization
	void Start () {

		health = GetComponent<Health>();
	
	}
	
	void OnCollisionEnter(Collision collision) {

		health.InflictDamage( Mathf.RoundToInt(collision.relativeVelocity.magnitude * damageVelocityRatio) );

	}

}
