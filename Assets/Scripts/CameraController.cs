using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public float snapDistance;
	public float smoothSpeed;
	public float maxSpeed = 1f;
	public float snapTimeout;
	public float lookAhead = 1f;
	public Vector3 cameraOffset = new Vector3(0f,25f,-45f);
	
	public Transform target {
		get {
			return PartyController.instance.leader.transform;
		}
	}
	
	
	private float lastSnapTime = 0f;
	
	public static CameraController instance;
	
	void Awake() {
		
		instance = this;
		
	}
	
	private Vector3 targetCamera {
		
		get {
			
			return (cameraOffset + target.position);
			
		}
		
	}
	
	public void Snap() {
		
		transform.position = targetCamera;
		lastSnapTime = Time.time;
		
	}
	
	public void Apply() {
		
		if ( Vector3.Distance(transform.position, targetCamera ) < snapDistance &&
			Time.time - lastSnapTime > snapTimeout ) {
			
			Snap ();
			
		} else {
			
			Vector3 movement = targetCamera - transform.position;
			movement *= Mathf.Min(maxSpeed, movement.magnitude * smoothSpeed * Time.deltaTime);
			transform.position += movement;
			
		}
		
	}
	
	void LateUpdate() {
		
		Apply();
		
	}
	
}
