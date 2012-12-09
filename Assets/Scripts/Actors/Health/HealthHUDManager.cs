using UnityEngine;
using System.Collections;

public class HealthHUDManager : MonoBehaviour {

	public GameObject healthHUDPrefab;

	// Use this for initialization
	void Start () {

		if (healthHUDPrefab == null) {
			Debug.LogWarning("Specify health HUD prefab");
			return;
		}

		Health[] healths = FindObjectsOfType(typeof(Health)) as Health[];
		foreach(Health h in healths) {

			if (!h.properties.showHUD)
				continue;

			GameObject hudObject = Object.Instantiate(healthHUDPrefab, new Vector3(0.5f,0.5f,1f), transform.rotation) as GameObject;
			hudObject.name = h.name + " Health HUD";
			hudObject.transform.parent = transform;
			HealthHUD hud = hudObject.AddComponent<HealthHUD>();
			hud.Init(h);

		}
	
	}

}
