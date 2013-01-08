using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Vision))]
public class DisableDamage : MonoBehaviour {

	public int damage = 10;

	void OnDisable() {

		Vision vision = GetComponent<Vision>();
		List<Visible> visibles = vision.visibles;
		
		foreach(Visible v in visibles) {

			if ( v == null )
				continue;

			Health health = v.GetComponent<Health>();
			if ( health == null )
				continue;
			
			float sqrDistance = (v.transform.position - transform.position).sqrMagnitude;

			health.InflictDamage( Mathf.RoundToInt( sqrDistance * (float) damage / vision.sqrVisionDistance ) );

		}

	}

}
