using UnityEngine;
using System.Collections;

public class TargetableTransform : MonoBehaviour {

	void OnMouseUpAsButton() {

		PartyController.instance.Attack(this.transform);

	}

}
