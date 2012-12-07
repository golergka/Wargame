using UnityEngine;
using System.Collections;

public class HealthHUD : MonoBehaviour {

	Health health;

	public void Init(Health health) {

		this.health = health;

	}

	void Start() {

		if (health == null) {
			Debug.LogWarning("No health attached!");
			gameObject.SetActive(false);
		}

	}
	
	// Update is called once per frame
	void Update () {

		transform.position = Camera.main.WorldToViewportPoint(health.transform.position);
		guiTexture.color = Color.Lerp( Color.red, Color.green, health.healthPoints / health.maxHealthPoints );
	
	}
}
