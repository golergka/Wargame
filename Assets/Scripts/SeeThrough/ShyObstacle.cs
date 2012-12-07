using UnityEngine;
using System.Collections;

// This script is attached to obstacles that can obscure the heroes.

public class ShyObstacle : MonoBehaviour {

	public float hideSpeed = 1f;
	public float hiddenAlpha = 0.2f;
	public float revealedAlpha = 1f;

	private int layer;

	private bool _hidden = false;
	public bool hidden {
		get { return _hidden; }
		set {

			if (_hidden == value)
				return;

			if (value) {
				layer = gameObject.layer;
				gameObject.layer = 2; // Ignore Raycast
			} else {
				gameObject.layer = layer;
			}

			_hidden = value;

		}
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
