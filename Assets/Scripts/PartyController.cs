using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PartyController : MonoBehaviour {
	
#region Singleton

	static public PartyController instance;

	void Awake() {
		instance = this;
	}

#endregion

#region Hero list

	public List<HeroController> heroes;

	void Start() {
		
		heroes = new List<HeroController> ( (HeroController[] ) FindObjectsOfType(typeof(HeroController)) );
		Select(heroes[0]);
		
	}

#endregion
	
#region Leader

	void EnforceFollowLeader() {

		if (followLeader) {
				
			foreach(HeroController hero in heroes)
				if( hero != _leader)
					hero.Follow(leader.transform);
			
		} else {
			
			foreach(HeroController hero in heroes)
				if ( hero != _leader)
					hero.Stop ();
			
		}

	}

	private HeroController _leader;
	public HeroController leader {
		get {
			return _leader;
		}
		private set {
			_leader = value;
			EnforceFollowLeader();
		}
	}
	
	private bool _followLeader;
	public bool followLeader {
		
		get { return _followLeader; }
		
		set {
			
			if (_followLeader == value)
				return;
			
			_followLeader = value;
			
			EnforceFollowLeader();
			
		}
		
	}
	
	public void Select(HeroController hero) {
		
		if (_leader == hero)
			return;
		
		leader = hero;

		// Resetting the target
		partyTarget = null;
		partyTargetPosition = null;
		
	}

#endregion

#region Party actions

	Transform partyTarget;
	Vector3? partyTargetPosition;

	// For now, it's really simple
	public void Attack(Transform target) {

		partyTarget = target;

		if (_followLeader) {
			foreach(HeroController hero in heroes)
				hero.Attack(target);
		} else
			leader.Attack(target);

	}

	public void Move(Vector3 targetPosition) {

		partyTargetPosition = targetPosition;

		leader.Move(targetPosition);

	}

#endregion
	
}
