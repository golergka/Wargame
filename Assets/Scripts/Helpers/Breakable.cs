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

		int damage = Mathf.RoundToInt(collision.relativeVelocity.magnitude * damageVelocityRatio);

		if (damage > 0) 
			health.InflictDamage( damage, this );

	}

}
