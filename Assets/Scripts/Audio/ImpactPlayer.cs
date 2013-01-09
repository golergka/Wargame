using UnityEngine;
using System.Collections;

public class ImpactPlayer : MonoBehaviour {

	public AudioClip impactSound;
	public float maxVelocity = 1f;

	// Use this for initialization
	void Start () {

		if (audio == null)
			gameObject.AddComponent<AudioSource>();
	
	}

	void OnCollisionEnter(Collision collision) {

		if (impactSound == null) {
			Debug.LogWarning("No sound defined!");
			return;
		}

		audio.PlayOneShot(impactSound, collision.relativeVelocity.magnitude / maxVelocity);
		audio.pitch = Random.Range(0.8f,1.2f);

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
