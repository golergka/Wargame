using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(ParameterManager))]
public class DisableDamage : MonoBehaviour {

	public int damage {

		get {

			return parameterManager.GetParameterValue<int>(Attack.DAMAGE_KEY);

		}

	}
	public float delay = 0f;

	// used if delay is above zero

	public float range {

		get {

			return parameterManager.GetParameterValue<float>(Attack.ATTACK_RANGE_KEY);

		}

	}

	bool quitting = false;

	ParameterManager parameterManager;

	void Awake() {

		parameterManager = GetComponent<ParameterManager>();

	}

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
				float damagePercentage = sqrDistance / vision.sqrVisionDistance;

				// Debug.DrawLine(transform.position, v.transform.position,
				// 	Color.Lerp(Color.green, Color.red, damagePercentage), 1f, false);

				health.InflictDamage( Mathf.RoundToInt( ( (float) damage ) * damagePercentage ), this );

			}

		} else {

			GameObject destroyer = new GameObject(gameObject.name + " [destroyer]");
			destroyer.transform.position = transform.position;

			AgroList agroList = GetComponent<AgroList>();

			if (agroList == null || agroList.agroLeader == null)
				AgroResponsible.MakeResponsible(destroyer, this);
			else 
				AgroResponsible.MakeResponsible(destroyer, agroList.agroLeader);

			ParameterManager parameterManager = destroyer.AddComponent<ParameterManager>();
			parameterManager.parameters.Add(Vision.VISION_DISTANCE_KEY, new Parameter<float>(range));
			parameterManager.parameters.Add(Attack.ATTACK_RANGE_KEY, new Parameter<float>(range));
			parameterManager.parameters.Add(Attack.DAMAGE_KEY, new Parameter<int>(damage));
			
			destroyer.AddComponent<Vision>();

			TimedDisabler destroyerDisabler = destroyer.AddComponent<TimedDisabler>();
			destroyerDisabler.lifeTime = delay;

			destroyer.AddComponent<DisableDamage>();

		}

	}

}
