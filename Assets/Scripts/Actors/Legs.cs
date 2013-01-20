using UnityEngine;
using System;
using System.Collections;
using Pathfinding;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(ParameterManager))]
[RequireComponent(typeof(Seeker))]
public class Legs : MonoBehaviour {
	
#region Public setup

	ParameterManager parameterManager;

	public float speed {
		get {
			return parameterManager.GetParameterValue<float>("Speed");
		}
	}
	// reach to use for waypoints and Vector3 targets
	public float targetReach = 0.5f;

	// reach to use for Transform targets
	public float targetTransformReach {
		get {
			return parameterManager.GetParameterValue<float>("Attack range");
		}
	}
	
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
		
		targetPosition = position;
		targetTransform = null;

		// we only reset path if we're transitioning to this state now
		// otherwise, repath can take care of that
		if ( legsState != LegsState.MovingToPosition ) {
			path = null;
			seeker.StartPath(transform.position, targetPosition, OnPathComplete);
		}

		legsState = LegsState.MovingToPosition;
		
	}
	
	public void Pursue(Transform actor) {
		
		legsState = LegsState.PursuingTransform;
		targetTransform = actor;
		path = null;
		seeker.StartPath(transform.position, targetTransform.position, OnPathComplete);
		
	}
	
	public void Follow(Transform actor) {
		
		legsState = LegsState.FollowingTransform;
		targetTransform = actor;
		path = null;
		seeker.StartPath(transform.position, targetTransform.position, OnPathComplete);
		
	}
	
	public void Stop() {
		
		targetTransform = null;
		legsState = LegsState.Idle;
		path = null;
		
	}
	
#endregion
	
#region Move methods
	
	CharacterController characterController;
	Animator animator;
	
	Seeker seeker;
	Path path;
	int currentWaypoint = 0;
	
	void Awake() {
		
		parameterManager = GetComponent<ParameterManager>();
		seeker = GetComponent<Seeker>();
		characterController = GetComponent<CharacterController>();
		animator = GetComponent<Animator>();
		
	}
	
	public void OnPathComplete(Path p) {
	
		StartCoroutine ( WaitToRepath () );
	
		if (!p.error) {
		
			path = p;
			currentWaypoint = 0;
		
		} else {
			Debug.LogWarning("Error with path!");
		}
	
	}
	
	const float REPATH_RATE = 0.5f;
	float lastPathSearch;
	
	public void Repath() {
	
		lastPathSearch = Time.time;
	
		if ( ( legsState == LegsState.Idle ) || !seeker.IsDone() ) {
			return;
		}
		
		if ( legsState == LegsState.MovingToPosition ) {
			seeker.StartPath(transform.position, targetPosition, OnPathComplete);
		} else {
			seeker.StartPath(transform.position, targetTransform.position, OnPathComplete);
		}
	
	}
	
	public IEnumerator WaitToRepath () {
		float timeLeft = REPATH_RATE - (Time.time-lastPathSearch);
		
		yield return new WaitForSeconds (timeLeft);
		Repath ();
	}
	
	private void MoveTowards(Vector3 target) {
		
		Vector3 desiredVelocity = (target - transform.position).normalized;
		desiredVelocity.y = 0;
		desiredVelocity *= speed;
		
		characterController.SimpleMove (desiredVelocity);
		animator.SetFloat("speed", desiredVelocity.magnitude);
		transform.LookAt(target);
		
	}

	[HideInInspector]
	public bool gotPath = false;
	
	void Update() {
	
		if ( legsState == LegsState.Idle )
			return;

		if ( targetTransform != null && Vector3.Distance(transform.position, targetTransform.position) < targetTransformReach ) {

			animator.SetFloat("speed", 0);
			if ( legsState != LegsState.FollowingTransform )
				legsState = LegsState.Idle;

			return;

		}
		
		// moving with path
		if ( path != null ) {

			gotPath = true;

			if ( Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]) < targetReach ) {
			
				currentWaypoint++;

				if ( currentWaypoint >= path.vectorPath.Length ) {
				
					path = null;
					animator.SetFloat("speed", 0);
				
					if (legsState != LegsState.FollowingTransform)
						legsState = LegsState.Idle;
						
					return;

				}
				
			}

			MoveTowards(path.vectorPath[currentWaypoint]);

		} else if ( legsState == LegsState.MovingToPosition ) { // we don't have the path ready yet — moving to target at the straight line

			gotPath = false;

			// checking if we're there
			if ( Vector3.Distance(transform.position, targetPosition) < targetReach ) {

				animator.SetFloat("speed", 0);
				legsState = LegsState.Idle;
				return;

			}

			MoveTowards(targetPosition);
		
		}
		
	}
	
#endregion
	
}
