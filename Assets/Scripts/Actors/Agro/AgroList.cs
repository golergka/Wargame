using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class AgroList : MonoBehaviour {

	const int NEW_LEADER_DELTA = 20;
	const int NEW_MEMBER_AGRO = 10;
	const float DECREASE_AGRO_PERIOD = 5f;
	const int DECREASE_AGRO_AMOUNT = 1;

	// may be sorted
	Dictionary<Health, int> unsortedAgroList = new Dictionary<Health, int>();

	int sortedHashCode = 0; // to check if the agro list is sorted
	public Dictionary<Health, int> sortedAgroList { // must be sorted

		get {

			if (unsortedAgroList.GetHashCode() != sortedHashCode) {

				unsortedAgroList = (from entry in unsortedAgroList orderby entry.Value ascending select entry).
				ToDictionary(pair => pair.Key, pair => pair.Value);
				sortedHashCode = unsortedAgroList.GetHashCode();

			}

			return unsortedAgroList;

		}

	}

	public event Action<AgroList, Health> NewAgroLeader;

	private Health _agroLeader;
	public Health agroLeader {

		get {

			CheckAgroLeader();
			return _agroLeader;

		}

		private set {

			if (_agroLeader == value)
				return;

			_agroLeader = value;

			if (NewAgroLeader != null)
				NewAgroLeader(this, value);

		}

	}

	bool CanBeLeader(Health potentialLeader) {

		return (vision == null || vision.IsInSight(potentialLeader) );

	}

	void FindNewAgroLeader() {

		foreach(Health h in sortedAgroList.Keys)
			if ( CanBeLeader(h) ) {
				agroLeader = h;
				return;
			}

		agroLeader = null;

	}

	void CheckAgroLeader() {

		if (!CanBeLeader(_agroLeader))
			FindNewAgroLeader();

	}

	void TryNewLeader(Health potentialLeader) {

		if (
				CanBeLeader(potentialLeader) &&
				(
					agroLeader == null ||
					unsortedAgroList[potentialLeader] > unsortedAgroList[agroLeader] + NEW_LEADER_DELTA
				)
			)
			agroLeader = potentialLeader;

	}

	Vision vision;

	// Use this for initialization
	void Start () {

		Health health = GetComponent<Health>();
		if (health == null) {
			Debug.LogWarning("Health is required component for Agrolist to work!");
			return;
		}

		vision = GetComponent<Vision>();
		if (vision != null) {
			vision.NoticedVisible += OnNoticed;
			vision.LostVisible += OnLost;
		}

		health.TakeDamage += OnTakeDamage;

		InvokeRepeating("DecreaseAgro", DECREASE_AGRO_PERIOD, DECREASE_AGRO_PERIOD);
	
	}

	void OnTakeDamage(Health me, int damageAmount, int agro, MonoBehaviour sender) {

		Health offender = AgroResponsible.GetResponsible(sender).GetComponent<Health>();
		if (offender == null)
			return; // we don't anger at forces of nature that are out of our reach

		if (!unsortedAgroList.ContainsKey(offender))
			unsortedAgroList.Add(offender, agro);
		else
			unsortedAgroList[offender] += agro;

		TryNewLeader(offender);

	}

	void OnNoticed(Vision vision, Visible visible) {

		Health potentialLeader = AgroResponsible.GetResponsible(visible).GetComponent<Health>();

		if ( potentialLeader == null || !FriendOrFoe.IsEnemy(this, visible) )
			return;

		if ( !unsortedAgroList.ContainsKey(potentialLeader) )
			unsortedAgroList.Add(potentialLeader, NEW_MEMBER_AGRO);

		TryNewLeader(potentialLeader);

	}

	void OnLost(Vision vision, Visible visible) {

		CheckAgroLeader();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void DecreaseAgro() {

		lock(unsortedAgroList) {

			Dictionary<Health, int> agroListCopy = new Dictionary<Health, int> (unsortedAgroList);

			foreach( Health h in agroListCopy.Keys )
				unsortedAgroList[h] = Mathf.Max(0, agroListCopy[h] - DECREASE_AGRO_AMOUNT);

		}

	}

	void OnDrawGizmos() {

		if (agroLeader == null)
			return;

		Gizmos.color = Color.red;
		Gizmos.DrawLine(transform.position, agroLeader.transform.position);

	}

}
