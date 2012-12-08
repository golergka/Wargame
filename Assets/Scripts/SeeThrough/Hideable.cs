using UnityEngine;
using System.Collections;

// This script is attached to obstacles that can obscure the heroes.

public class Hideable : MonoBehaviour {

	public float hideSpeed = 1f;
	public float hiddenAlpha = 0.2f;
	public float revealedAlpha = 1f;

	private int layer;

	private int hideCount = 0; // how many objects are hiding use now
	
	public void Hide() {
	
		hideCount++;

		if (hideCount == 1) { // just hid
			layer = gameObject.layer;
			gameObject.layer = 2; // ignore raycast
		}

	}

	public void Unhide() {
		hideCount--;

		if (hideCount < 0) {
		
			Debug.LogError("hideCount below zero!");

		} else if ( hideCount == 0 ) { // just unhid

			gameObject.layer = layer;

		}

	}

	public bool hidden {
		get { return (hideCount > 0); }
	}

	private Renderer[] renderers;
	private float _currentAlpha = 1f; // we're assuming that all the materials are not-transparent by default
	private float currentAlpha {
		get { return _currentAlpha; }
		set {

			foreach(Renderer r in renderers) {

				Material[] materials = r.materials;
				foreach(Material m in materials) {
					Color mColor = m.color;
					mColor.a = value;
					m.color = mColor;
				}
				r.materials = materials;

			}

			_currentAlpha = value;

		}
	}

	void Start() {

		// we need to control all of the children. Prefabs can be complicated, uknow
		renderers = GetComponentsInChildren<Renderer>(true);

	}
	
	// Update is called once per frame
	void Update () {

		if (hidden) {

			if (currentAlpha > hiddenAlpha)
				currentAlpha -= hideSpeed * Time.deltaTime;

		} else {

			if (currentAlpha < revealedAlpha)
				currentAlpha += hideSpeed * Time.deltaTime;

		}
	
	}

}
