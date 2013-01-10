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

		if (targetMarker == null) {
			Debug.LogWarning("No target marker assigned!");
			return;
		}

		_targetMarkerInstance = (Transform) Instantiate(targetMarker, transform.position, transform.rotation);
		_targetMarkerInstance.parent = transform;
		_targetMarkerInstance.gameObject.SetActive(true);
		
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

#region Party targets

	public Transform targetMarker;
	Transform _targetMarkerInstance;

	private Transform _partyTarget;
	public Transform partyTarget {

		get { return _partyTarget; }

		private set {

			if (_partyTarget != null) {

				Health oldHealth = _partyTarget.GetComponent<Health>();
			
				if (oldHealth != null) {

					oldHealth.ZeroHealth -= OnZeroHealth;

					HealthHUD hud = oldHealth.hud;

					if (hud != null) {

						if (!hud.shownByDefault)
							hud.gameObject.SetActive(false);

					} else {
						Debug.LogWarning("Couldn't find health hud!");
					}

				}

			}

			_partyTarget = value;

			if (_partyTarget != null) {

				_targetMarkerInstance.gameObject.SetActive(true);
				_targetMarkerInstance.parent = _partyTarget;
				_targetMarkerInstance.localScale = new Vector3(1f,1f,1f);
				_targetMarkerInstance.localPosition = new Vector3(0f,0f,0f);

				Health newHealth = _partyTarget.GetComponent<Health>();

				if (newHealth != null) {

					newHealth.ZeroHealth += OnZeroHealth;

					HealthHUD hud = newHealth.hud;

					if (hud != null) {

						hud.gameObject.SetActive(true);

					} else {
						Debug.LogWarning("Couldn't find healthHUD!");
					}

				}

			} else {

				if (_targetMarkerInstance != null) {
					_targetMarkerInstance.gameObject.SetActive(false);
					_targetMarkerInstance.parent = this.transform;
				}

			}

		}

	}

	Vector3? partyTargetPosition;

	public void OnZeroHealth(Health health) {

		Stop();

	}

#endregion

#region Party actions

	public void Stop() {

		partyTarget = null;

		leader.Stop();

		EnforceFollowLeader();

	}

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

		Stop();

		partyTargetPosition = targetPosition;
		leader.Move(targetPosition);

	}

#endregion
	
}
