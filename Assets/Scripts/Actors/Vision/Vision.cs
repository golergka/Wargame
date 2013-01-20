// Here you define either you want to check distances in 3d, or in 2d (without the Vector3.y)

#define VISION_2D
// #define VISION_3D

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(ParameterManager))]
public class Vision : MonoBehaviour {

	public const string VISION_DISTANCE_KEY = "Vision distance";

	public List<Visible> VisiblesInSight() {

		List<Visible> result = new List<Visible>();
		foreach(Visible v in visiblesInSight)
			if (v != null)
				result.Add(v);

		return result;

	}

	public bool IsVisibleInSight(Visible visible) {

		foreach(Visible v in visiblesInSight)
			if (v == visible)
				return true;

		return false;

	}

	public bool IsInSight(MonoBehaviour other) {

		Visible visible = other.GetComponent<Visible>();

		if (visible != null)
			return IsVisibleInSight(visible);
		else
			return true; // objects without visibles are always visible

	}

	// Vision distance. We keep square distnace to optimize length checks
	public float sqrVisionDistance { get { return visionDistance*visionDistance; } }
	
	public float visionDistance {

		get {
			return parameterManager.GetParameterValue<float>(VISION_DISTANCE_KEY);
		}

	}

	ParameterManager parameterManager;

	void Awake() {

		parameterManager = GetComponent<ParameterManager>();

	}

#region Messaging

	public event Action<Vision, Visible> NoticedVisible; // sender, observee
	public event Action<Vision, Visible> LostVisible; // sender, observee

#endregion

#region Visibles and invisibles

	// We keep arrays of both visibles and invisbles in sight
	private const int VISION_LIMIT = 100; // Vision limit
	private Visible[] visiblesInSight   = new Visible[VISION_LIMIT];
	private Visible[] invisiblesInSight = new Visible[VISION_LIMIT];
	// It may be better to switch to Lists in some situations.

	public List<Visible> visibles {
		get {
			return new List<Visible>(visiblesInSight);
		}
	}

	// Basic array management, nothing particularly interesting here.

	private void AddVisible(Visible visible) {

		for(int i = 0; i<VISION_LIMIT; i++) {
		
			if (visiblesInSight[i] == null) {
				visiblesInSight[i] = visible;
				return;
			}

		}
		
		Debug.LogError("No place for new visible!");

	}

	private void RemoveVisible(Visible visible) {

		for(int i = 0; i<VISION_LIMIT; i++) {

			if (visiblesInSight[i] == visible) {
				visiblesInSight[i] = null;
				return;
			}

		}

		Debug.LogError("Visible to remove not found!");

	}

	private void AddInvisible(Visible visible) {

		for(int i = 0; i<VISION_LIMIT; i++) {
			
			if (invisiblesInSight[i] == null) {
				invisiblesInSight[i] = visible;
				return;
			}

		}

		Debug.LogError("No place for new invisible!");

	}

	private void RemoveInvisible(Visible visible) {

		for(int i = 0; i<VISION_LIMIT; i++) {

			if (invisiblesInSight[i] == visible) {
				invisiblesInSight[i] = null;
				return;
			}

		}

		Debug.LogError("Invisible to remove not found!");

	}

#endregion

	public void ChangedVisibility(Visible visible) {

		if (visible.visible) {

			RemoveInvisible(visible);
			AddVisible(visible);

			if (NoticedVisible != null)
				NoticedVisible(this, visible);

		} else {

			RemoveVisible(visible);
			AddInvisible(visible);
			
			if (LostVisible != null)
				LostVisible(this, visible);

		}

	}

	// Here we check if the Visible is in our range.
	// It's quite easy to change from 2d to 3d mechanics with a single define.
	// 2d is supposed to be faster. I didn't test it yet.
	// It's premature optimization. It's the root of all evil. I'm the fucking devil.
	private bool VisibleInRange(Visible observee) {

#if VISION_2D

	Vector2 observee2d = new Vector2(observee.transform.position.x, observee.transform.position.z);
	Vector2 position2d = new Vector2(transform.position.x, transform.position.z);
	Vector2 difference = observee2d - position2d;
	return (difference.sqrMagnitude < sqrVisionDistance);

#endif

#if VISION_3D

	Vector3 difference = observee.transform.position - transform.position;
	return (difference.sqrMagnitude < sqrVisionDistance);

#endif

	}

	//Checking all the ranges
	void Update() {

		// The point of neighbors is to keep track of visibles that could potentially end up in our vision range.
		// The neigbors probably contain all Visibles and Insivibles in sight.
		// Except for those who ran away very quickly. (Teleported, for example).
		List<Visible> neighbors = VisibleGrid.instance.GetNeighbors(transform.position);

		// We check all the visibles in sight — are they still in range
		for(int i=0; i<VISION_LIMIT; i++) {
			
			Visible visible = visiblesInSight[i];

			if (visible == null)
				continue;

			// If the visible isn't in our range anymore, we remove it
			if ( !VisibleInRange(visible) ) {

				visiblesInSight[i] = null;
				visible.inRangeOfVisions.Remove(this);

				if (LostVisible != null)
					LostVisible(this, visible);

			}

			// Since we already checked this one, might as well remove it from the neigbors
			if (neighbors.Contains(visible))
				neighbors.Remove(visible);

		}

		// Now invisibles
		for(int i=0; i<VISION_LIMIT; i++) {
			
			Visible visible = invisiblesInSight[i];

			if (visible == null)
				continue;

			if ( !VisibleInRange(visible) ) {

				invisiblesInSight[i] = null;
				visible.inRangeOfVisions.Remove(this);
				// Here's the difference between the code above and this — we don't send messages about losing it from sight.
				// Since it was already invisible.

			}

			if (neighbors.Contains(visible))
				neighbors.Remove(visible);

		}

		// Checking the rest of the neighbours
		foreach(Visible visible in neighbors) {

			if ( VisibleInRange(visible) ) {

				visible.inRangeOfVisions.Add(this);

				if (visible.visible) {

					AddVisible(visible);
					
					if (NoticedVisible != null)
						NoticedVisible(this, visible);

				} else {

					AddInvisible(visible);

				}

			}

		}

	}

	void OnDrawGizmos() {

		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(transform.position, visionDistance);

		Gizmos.color = Color.red;
		foreach(Visible visible in invisiblesInSight)
			if (visible != null)
				Gizmos.DrawLine(transform.position, visible.transform.position);

		Gizmos.color = Color.green;
		foreach(Visible visible in visiblesInSight)
			if (visible != null)
				Gizmos.DrawLine(transform.position, visible.transform.position);

	}

}
