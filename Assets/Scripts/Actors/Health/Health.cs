using System;
using UnityEngine;
using System.Collections;

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

				if (FullHealth != null)
					FullHealth(this);

			}

		}
	}

	public event Action<Health, int> TakeDamage;
	public event Action<Health, int> TakeHealing;
	public event Action<Health> FullHealth;
	public event Action<Health> ZeroHealth;

	void Start () {

		healthPoints = maxHealthPoints;
	
	}

	public void InflictDamage(int damageAmount) {

		if ( damageAmount == 0 ) {

			Debug.LogWarning("Received 0 damage!");
			return;
			
		}

		bool died = false;

		if ( damageAmount >= healthPoints ) {

			healthPoints = 0;
			died = true;
			gameObject.SetActive(false);

		} else {

			healthPoints -= damageAmount;

		}

		if (TakeDamage != null)
			TakeDamage(this, damageAmount);

		if (died)
			if (ZeroHealth != null)
				ZeroHealth(this);

	}

	public void InflictHealing(int healingAmount) {

		if ( healingAmount == 0 ) {

			Debug.LogWarning("Received 0 healing!");
			return;

		}

		if ( healingAmount >= maxHealthPoints - healthPoints ) {

			healthPoints = maxHealthPoints;

			if (FullHealth != null)
				FullHealth(this);

		}

		if (TakeHealing != null)
			TakeHealing(this, healingAmount);

	}

}
