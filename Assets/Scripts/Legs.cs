using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]

public class Legs : MonoBehaviour {
	
	#region Public setup

	public float speed = 1f;
	
	public float targetReach = 0.5f;
	public float targetClose = 1f;
	
	#endregion
	
	#region Private state
	
	private Vector3 targetPosition;
	private Transform targetTransform;
	
	private enum LegsState {
		
		Idle,
		MovingToPosition,
		FollowingTarget,
		
	}
	
	private LegsState legsState = LegsState.Idle;
	
	#endregion
	
	#region Public interface
	
	public void MoveToPosition(Vector3 position) {
		
		legsState = LegsState.MovingToPosition;
		targetPosition = position;
		
	}
	
	public void FollowTarget(Transform actor) {
		
		legsState = LegsState.FollowingTarget;
		targetTransform = actor;
		
	}
	
	public void Stop() {
		
		legsState = LegsState.Idle;
		
	}
	
	#endregion
	
	#region Move methods
	
	private CharacterController characterController;
	
	void Awake() {
		
		characterController = GetComponent<CharacterController>();
		
	}
	
	private void MoveTowards(Vector3 target) {
		
		Vector3 desiredVelocity = (target - transform.position).normalized;
		desiredVelocity.y = 0;
		desiredVelocity *= speed;
		
		// slowing down as we approach target closely
		float distance = Vector3.Distance(transform.position, target);
		if (distance < targetClose)
			desiredVelocity *= distance / targetClose;
		
		characterController.SimpleMove (desiredVelocity);
		
	}
	
	private bool ReachedTarget() {
		
		switch (legsState) {
			
		case LegsState.MovingToPosition:
			return (Vector3.Distance(transform.position, targetPosition) < targetReach);
			
		case LegsState.FollowingTarget:
			return (Vector3.Distance(transform.position, targetTransform.position) < targetReach);
			
		default:
			Debug.LogError("Unexpected legs state!");
			return false;
			
		}
		
	}
	
	void Update() {
		
		if (legsState == LegsState.Idle)
			return;
		
		if ( ReachedTarget() ) {
			legsState = LegsState.Idle;
			return;
		}
		
		switch (legsState) {
			
		case LegsState.MovingToPosition:
			MoveTowards(targetPosition);
			break;
			
		case LegsState.FollowingTarget:
			MoveTowards(targetTransform.position);
			break;
			
		}
		
	}
	
	#endregion
	
}
