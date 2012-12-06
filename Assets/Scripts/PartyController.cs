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

	public List<HeroController> heroes;
	
	private HeroController _leader;
	public HeroController leader { get { return _leader; } }
	
	public void Select(HeroController hero) {
		
		if (_leader == hero)
			return;
		
		_leader = hero;
		
		if (followLeader) { 
			
			// this looks retarted, but that's because it's really a property
			followLeader = false;
			followLeader = true;
			
		}
		
	}
	
	void Start() {
		
		heroes = new List<HeroController> ( (HeroController[] ) FindObjectsOfType(typeof(HeroController)) );
		Select(heroes[0]);
		
	}
	
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
	
}
