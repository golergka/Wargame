using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Legs))]

public class PlayerController : MonoBehaviour {
	
	#region Hero management

	public static List<PlayerController> characters = new List<PlayerController>();
	public static PlayerController selectedCharacter;

	public void Select() {
		
		selectedCharacter = this;
		
	}
	
	public int selectPriority;
	
	#endregion
	
	new public string name { get { return gameObject.name; } }
	
	void Awake() {
		
		characters.Add (this);
		legs = GetComponent<Legs>();
		
		if (selectedCharacter == null) {
			selectedCharacter = this;
		} else if (selectPriority > selectedCharacter.selectPriority) {
			selectedCharacter = this;
		}
		
	}
	
	#region Movement Interface
	
	private Legs legs;
	
	// right now, it's just Legs proxy
	// however, there are likely player-specific checks to be implemented on this level
	
	public void MoveToPosition(Vector3 position) {
		legs.MoveToPosition(position);
	}
	
	public void FollowTarget(Transform actor) {
		legs.FollowTarget(actor);
	}
	
	public void Stop() {
		legs.Stop ();
	}
	
	#endregion
	
}
