using UnityEngine;
using System.Collections;

public class HealthHUD : MonoBehaviour {

	public float xSize = 0.1f;

	Health health;

	public bool shownByDefault { get; private set; }

	private bool _shown;
	public bool shown {
		get {

			return ( _shown || shownByDefault);

		}

		set {

			_shown = value;

		}

	}

	public void Init(Health health) {

		this.health = health;
		health.hud = this;
		shownByDefault = health.showHUD;

	}

	void Start() {

		if (health == null) {
			Debug.LogWarning("No health attached!");
			gameObject.SetActive(false);
		}

	}
	
	// Update is called once per frame
	void Update () {

		float healthPercentage = (float) health.healthPoints / (float) health.maxHealthPoints;

		if (!shown && healthPercentage == 1f) {

			guiTexture.color = Color.clear;

		} else {

			Vector3 position = Camera.main.WorldToViewportPoint(health.transform.position);
			position.x -= (1 - healthPercentage) * xSize/2;
			transform.position = position;
			Vector3 scale = transform.localScale;
			scale.x = healthPercentage * xSize;
			transform.localScale = scale;

			guiTexture.color = Color.Lerp( Color.red, Color.green, healthPercentage );

		}
	
	}

}