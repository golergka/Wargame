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

	private HeroController _leader;
	public HeroController leader { get { return _leader; } }
	
	private bool _followLeader;
	public bool followLeader {
		
		get { return _followLeader; }
		
		set {
			
			if (_followLeader == value)
				return;
			
			_followLeader = value;
			
			if (value) {
				
				foreach(HeroController hero in heroes)
					if( hero != _leader)
						hero.Follow(leader.transform);
				
			} else {
				
				foreach(HeroController hero in heroes)
					if ( hero != _leader)
						hero.Stop ();
				
			}
			
		}
		
	}
	
	public void Select(HeroController hero) {
		
		if (_leader == hero)
			return;
		
		_leader = hero;
		
		if (followLeader) { 
			
			// this looks retarted, but that's because it's really a property
			followLeader = true;
			
		}
		
	}

#endregion

#region Party actions

	// For now, it's really simple
	public void Attack(Transform target) {

		foreach(HeroController hero in heroes)
			hero.Attack(target);

	}

#endregion
	
}
