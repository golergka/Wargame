using UnityEngine;
using System.Collections.Generic;

public class AgroResponsible : MonoBehaviour {

	const int MAX_SEEKER_ITERATIONS = 100;

	public GameObject responsible { set; private get; }

	static public GameObject GetResponsible(GameObject accused) {

		// AgroResponsible agroResponsible = responsible.GetComponent<AgroResponsible>();

		// if (agroResponsible == null)
		// 	return accused;

		Stack<GameObject> responsibility = new Stack<GameObject>();
		responsibility.Push(accused);

		for(int i=0; i<MAX_SEEKER_ITERATIONS; i++) {

			GameObject oldSuspect = responsibility.Peek();

			AgroResponsible agroResponsible = oldSuspect.GetComponent<AgroResponsible>();

			if (agroResponsible == null)
				return oldSuspect;

			GameObject newSuspect = agroResponsible.responsible;

			if (newSuspect == null) {
				Debug.LogWarning("AgroResponsible shouldn't be used with empty responsible field");
				return oldSuspect;
			}

			if (responsibility.Contains(newSuspect)) {
				Debug.LogError("Cycle responsibility chain detected: " + responsibility.ToString() );
				return oldSuspect;
			}

			responsibility.Push(newSuspect);

		}

		Debug.LogError("Exceeded max seeker iterations!");
		return null;

	}

	static public GameObject GetResponsible(Component accused) {
		return GetResponsible(accused.gameObject);
	}

	static public void MakeResponsible(GameObject innocent, GameObject responsible) {

		AgroResponsible agroResponsible = innocent.GetComponent<AgroResponsible>();

		if (agroResponsible == null)
			agroResponsible = innocent.AddComponent<AgroResponsible>();

		agroResponsible.responsible = GetResponsible(responsible);

	}

	static public void MakeResponsible(Component innocent, GameObject responsible) {
		MakeResponsible(innocent.gameObject, responsible);
	}	

	static public void MakeResponsible(GameObject innocent, Component responsible) {
		MakeResponsible(innocent, responsible.gameObject);
	}

	static public void MakeResponsible(Component innocent, Component responsible) {
		MakeResponsible(innocent.gameObject, responsible.gameObject);
	}

}
