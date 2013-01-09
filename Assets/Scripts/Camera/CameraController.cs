using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public float snapDistance;
	public float smoothSpeed;
	public float maxSpeed = 1f;
	public float snapTimeout;
	public float lookAhead = 1f;
	public Vector3 cameraOffset = new Vector3(0f,25f,-45f);
	public Vector3 cameraTargetOffset = new Vector3(0,0,0);
	public float listenerLift = 1f;
	
	public Transform target {
		get {
			return PartyController.instance.leader.transform;
		}
	}
	
	
	private float lastSnapTime = 0f;
	
	public static CameraController instance;

	private GameObject listener;
	
	void Awake() {
		
		instance = this;
		
	}

	void Start() {

		listener = new GameObject("Audio listener");
		listener.transform.parent = this.transform;
		listener.AddComponent<AudioListener>();

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

		transform.LookAt(target.position + cameraTargetOffset);
		
	}
	
	void LateUpdate() {

		Ray ray = this.camera.ViewportPointToRay( new Vector3(0.5f, 0.5f, 0f) );
		RaycastHit hit;

		int layerMask = 1 << 11; // terrain layer ONLY

		if ( Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask) ) {

			Vector3 position = hit.point;
			position.y += listenerLift;
			listener.transform.position = position;

		}
		
		Apply();
		
	}
	
}
