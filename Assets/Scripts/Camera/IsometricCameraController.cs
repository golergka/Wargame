using UnityEngine;
using System.Collections;

public class IsometricCameraController : CameraController {

	public float distance = 100f;

	protected override Vector3 targetCamera {
		get {
			return target.position + (transform.rotation * new Vector3(0,0,-distance));
		}
	}

	public override void Snap() {

		transform.position = targetCamera;

	}

	protected override void ApplyCameraPosition() {
		Snap(); // fuck smooth movement (for now)
	}

	void OnDrawGizmos() {

		if (target == null)
			return;

		Gizmos.color = Color.cyan;
		Gizmos.DrawLine(target.position, targetCamera);

	}

}
