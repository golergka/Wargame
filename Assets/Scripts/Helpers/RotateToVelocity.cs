using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]

public class RotateToVelocity : MonoBehaviour {

	void FixedUpdate() {

		if (rigidbody.isKinematic)
			return;

		if (rigidbody.velocity.magnitude == 0)
			return;

		rigidbody.MoveRotation( Quaternion.LookRotation( rigidbody.velocity ));

	}

}
