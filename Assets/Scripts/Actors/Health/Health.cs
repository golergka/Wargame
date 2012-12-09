using System;
using UnityEngine;
using System.Collections;

interface IHealthChangeListener {

	void OnTakeDamage(int damageAmount);
	void OnTakeHealing(int healingAmount);

}

interface IHealthStateListener {

	void OnFullHealth();
	void OnZeroHealth();

}

[Serializable]
public class HealthProperties {

	public int maxHealthPoints = 100;
	public bool showHUD = true;

}

public class Health : MonoBehaviour {

	// Public only for editor use
	public HealthProperties properties;

	public int healthPoints { get; private set; }

	public int maxHealthPoints {

		get { return properties.maxHealthPoints; }

		set {

			if (properties.maxHealthPoints == value)
				return;

			properties.maxHealthPoints = value;

			if ( healthPoints >= value ) {
				
				healthPoints = value;
				foreach(Component listener in healthStateListeners)
					((IHealthStateListener)listener).OnFullHealth();

			}

		}
	}

	private Component[] healthChangeListeners;
	private Component[] healthStateListeners;
	void Start () {

		healthPoints = maxHealthPoints;
		healthChangeListeners = GetComponents(typeof(IHealthChangeListener));
		healthStateListeners = GetComponents(typeof(IHealthStateListener));
	
	}

	public void InflictDamage(int damageAmount) {

		if ( damageAmount == 0 ) {

			Debug.LogWarning("Received 0 damage!");
			return;
			
		}

		bool zeroHealth = false;

		if ( damageAmount >= healthPoints ) {

			healthPoints = 0;
			zeroHealth = true;
			gameObject.SetActive(false);

		} else {

			healthPoints -= damageAmount;

		}

		// Temporary off as nobody uses that
		foreach(Component listener in healthChangeListeners)
			((IHealthChangeListener)listener).OnTakeDamage(damageAmount);

		if (zeroHealth)
			foreach(Component listener in healthStateListeners)
				((IHealthStateListener)listener).OnZeroHealth();

	}

	public void InflictHealing(int healingAmount) {

		if ( healingAmount == 0 ) {

			Debug.LogWarning("Received 0 healing!");
			return;

		}

		if ( healingAmount >= maxHealthPoints - healthPoints ) {

			healthPoints = maxHealthPoints;
			foreach(Component listener in healthStateListeners)
				((IHealthStateListener)listener).OnFullHealth();

		}

		foreach(Component listener in healthChangeListeners)
			((IHealthChangeListener)listener).OnTakeHealing(healingAmount);

	}

}
