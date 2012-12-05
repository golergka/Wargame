using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Legs))]

public class HeroController : MonoBehaviour {
	
	new public string name { get { return gameObject.name; } }
	
	void Awake() {

		legs = GetComponent<Legs>();
		
	}
	
	#region Movement Interface
	
	private Legs legs;
	
	// right now, it's just Legs proxy
	// however, there are likely player-specific checks to be implemented on this level
	
	public void Move(Vector3 position) {
		legs.Move(position);
	}
	
	public void Pursue(Transform actor) {
		legs.Pursue(actor);
	}
	
	public void Follow(Transform actor) {
		legs.Follow(actor);
	}
	
	public void Stop() {
		legs.Stop ();
	}
	
	#endregion
	
}
