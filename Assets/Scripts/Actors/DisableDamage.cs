using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Vision))]
public class DisableDamage : MonoBehaviour {

	public int damage = 10;

	void OnDisable() {

		List<Visible> visibles = GetComponent<Vision>().visibles;
		
		foreach(Visible v in visibles) {

			if ( v == null )
				continue;

			Health health = v.GetComponent<Health>();
			if ( health != null )
				health.InflictDamage(damage);

		}

	}

}
