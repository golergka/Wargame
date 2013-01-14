using UnityEngine;
using System.Collections;

public abstract class CameraController : MonoBehaviour {

	public Transform target {
		get {
			if (PartyController.instance != null)
				return PartyController.instance.leader.transform;
			else
				return null;
		}
	}

#region Camera placement

	protected abstract Vector3 targetCamera { get; }

	public abstract void Snap();

	protected abstract void ApplyCameraPosition();

#endregion

#region Singleton

	public static CameraController instance;

	void Awake() {
		
		instance = this;
		
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

		ApplyCameraPosition();

	}

#endregion

#region Audio listener

	public float listenerLift = 1f;	

	private GameObject listener;

	void Start() {

		listener = new GameObject("Audio listener");
		listener.transform.parent = this.transform;
		listener.AddComponent<AudioListener>();

	}

#endregion

}
