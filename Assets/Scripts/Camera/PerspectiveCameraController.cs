using UnityEngine;
using System.Collections;

public class PerspectiveCameraController : CameraController {

	public float snapDistance;
	public float smoothSpeed;
	public float maxSpeed = 1f;
	public float snapTimeout;
	public float lookAhead = 1f;
	public Vector3 cameraOffset = new Vector3(0f,25f,-45f);
	public Vector3 cameraTargetOffset = new Vector3(0,0,0);
	
	private float lastSnapTime = 0f;
	
	protected override Vector3 targetCamera {
		
		get {
			
			return (cameraOffset + target.position);
			
		}
		
	}
	
	public override void Snap() {
		
		transform.position = targetCamera;
		lastSnapTime = Time.time;
		
	}
	
	protected override void ApplyCameraPosition() {
		
		if ( Vector3.Distance(transform.position, targetCamera ) < snapDistance &&
			Time.time - lastSnapTime > snapTimeout ) {
			
			Snap ();
			
		} else {
			
			Vector3 movement = targetCamera - transform.position;
			movement *= Mathf.Min(maxSpeed, movement.magnitude * smoothSpeed * Time.deltaTime);
			transform.position += movement;
			
		}

		transform.LookAt(target.position + cameraTargetOffset);
		
	}
	
}
