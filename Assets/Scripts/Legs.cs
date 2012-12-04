using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]

public class Legs : MonoBehaviour {
	
	#region Public setup

	public float speed = 1f;
	
	public float targetReach = 0.5f;
	public float targetClose = 1f;
	public float targetFollow = 3f;
	
	#endregion
	
	#region Private state
	
	private Vector3 targetPosition;
	private Transform targetTransform;
	
	private enum LegsState {
		
		Idle,
		MovingToPosition,
		PursuingTransform,
		FollowingTransform,
		
	}
	
	private LegsState legsState = LegsState.Idle;
	
	#endregion
	
	#region Public interface
	
	public void Move(Vector3 position) {
		
		legsState = LegsState.MovingToPosition;
		targetPosition = position;
		
	}
	
	public void Pursue(Transform actor) {
		
		legsState = LegsState.PursuingTransform;
		targetTransform = actor;
		
	}
	
	public void Follow(Transform actor) {
		
		legsState = LegsState.FollowingTransform;
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
		
		float close = targetClose;
		if (legsState == LegsState.FollowingTransform) {
			close += targetFollow;
		}
		
		if (distance < close)
			desiredVelocity *= distance / close;
		
		characterController.SimpleMove (desiredVelocity);
		
	}
	
	private bool ReachedTarget() {
		
		switch (legsState) {
			
		case LegsState.MovingToPosition:
			return (Vector3.Distance(transform.position, targetPosition) < targetReach);
			
		case LegsState.PursuingTransform:
			return (Vector3.Distance(transform.position, targetTransform.position) < targetReach);
			
		default: // LegsState.FollowingTransform case
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
		
		if (legsState == LegsState.MovingToPosition )
			MoveTowards(targetPosition);
		else
			MoveTowards(targetTransform.position);
		
	}
	
	#endregion
	
}
