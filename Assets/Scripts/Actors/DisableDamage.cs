using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DisableDamage : MonoBehaviour {

	public int damage = 10;
	public float delay = 0f;

	// used if delay is above zero

	public float range = 3f;

	void OnDisable() {

		if (delay <= 0) {

			Vision vision = GetComponent<Vision>();

			if (vision == null) {
				Debug.LogWarning("Vision is required if delay is 0");
				return;
			}

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

		} else {

			GameObject destroyer = new GameObject(gameObject.name + " [destroyer]");
			destroyer.transform.position = transform.position;
			
			Vision destroyerVision = destroyer.AddComponent<Vision>();
			destroyerVision.visionDistance = range;

			TimedDisabler destroyerDisabler = destroyer.AddComponent<TimedDisabler>();
			destroyerDisabler.lifeTime = delay;

			DisableDamage destroyerDamager = destroyer.AddComponent<DisableDamage>();
			destroyerDamager.damage = this.damage;

		}

	}

}
