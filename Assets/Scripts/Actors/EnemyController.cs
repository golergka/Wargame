using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Legs))]
[RequireComponent(typeof(Attack))]
[RequireComponent(typeof(AgroList))]

public class EnemyController : MonoBehaviour {

	private Legs legs;
	private Attack attack;
	private AgroList agroList;
	
	void Awake() {

		legs = GetComponent<Legs>();
		attack = GetComponent<Attack>();

		agroList = GetComponent<AgroList>();
		agroList.NewAgroLeader += OnNewAgroLeader;
		

	}

	public void OnNewAgroLeader(AgroList agroList, Health agroLeader) {

		if (agroList != this.agroList) {
			Debug.LogError("Wrong agroList: " + agroList + " expected agroList: " + this.agroList);
			return;
		}

		attack.TryAppointTarget(agroLeader.transform);
		legs.Follow(agroLeader.transform);

	}

}
