using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DisableDamage : MonoBehaviour {

	public int damage = 10;
	public float delay = 0f;

	// used if delay is above zero

	public float range = 3f;

	bool quitting = false;

	void OnApplicationQuit() {
		quitting = true;
	}

	void OnDisable() {

		if (quitting)
			return;

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

				health.InflictDamage( Mathf.RoundToInt( sqrDistance * (float) damage / vision.sqrVisionDistance ), this );

			}

		} else {

			GameObject destroyer = new GameObject(gameObject.name + " [destroyer]");
			destroyer.transform.position = transform.position;

			AgroList agroList = GetComponent<AgroList>();

			if (agroList == null || agroList.agroLeader == null)
				AgroResponsible.MakeResponsible(destroyer, this);
			else 
				AgroResponsible.MakeResponsible(destroyer, agroList.agroLeader);
			
			Vision destroyerVision = destroyer.AddComponent<Vision>();
			destroyerVision.visionDistance = range;

			TimedDisabler destroyerDisabler = destroyer.AddComponent<TimedDisabler>();
			destroyerDisabler.lifeTime = delay;

			DisableDamage destroyerDamager = destroyer.AddComponent<DisableDamage>();
			destroyerDamager.damage = this.damage;

		}

	}

}
