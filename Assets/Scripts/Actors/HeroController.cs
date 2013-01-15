using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Legs))]
//[RequireComponent(typeof(Attack))]

public class HeroController : MonoBehaviour {

#region General setup
	
	new public string name { get { return gameObject.name; } }

	private Legs legs;
	private Attack attack;
	
	void Awake() {

		legs = GetComponent<Legs>();
		attack = GetComponent<Attack>();
		
	}

#endregion
	
#region Public hero controlling

	// right now, it's just Legs proxy
	// however, there are likely player-specific checks to be implemented on this level

	// Attacking

	public void Attack(Transform target) {

		attack.TryAppointTarget(target);
		legs.Follow(target);

	}

	// Moving
	
	public void Move(Vector3 position) {

		legs.Move(position);
		attack.DropTarget();

	}
	
	public void Pursue(Transform actor) {

		legs.Pursue(actor);
		attack.DropTarget();

	}
	
	public void Follow(Transform actor) {

		legs.Follow(actor);
		attack.DropTarget();

	}
	
	public void Stop() {

		legs.Stop ();
		attack.DropTarget();

	}
	
#endregion
	
}
