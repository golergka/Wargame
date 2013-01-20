using System;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParameterManager))]
public class Health : MonoBehaviour {

	public int healthPoints { get; private set; }

	public bool showHUD = true;

	public const string MAX_HEALTH_POINTS_KEY = "Max health points";

	public int maxHealthPoints {

		get { return parameterManager.GetParameterValue<int>(MAX_HEALTH_POINTS_KEY); }

	}

	ParameterManager parameterManager;

	void Awake() {

		parameterManager = GetComponent<ParameterManager>();

		Parameter<int> maxHealthPointsParameter = parameterManager.GetParameter<int>(MAX_HEALTH_POINTS_KEY);

		if (maxHealthPointsParameter != null)
			maxHealthPointsParameter.ValueChanged += OnMaxHealthPointsChange;

	}

	public void OnMaxHealthPointsChange(Parameter<int> maxHealthPointsParameter, int value) {

		if ( healthPoints >= value ) {
				
			healthPoints = value;

			if (FullHealth != null)
				FullHealth(this);

		}

	}

	[HideInInspector]
	public HealthHUD hud;

	public event Action<Health, int, int, MonoBehaviour> TakeDamage; // damage amount, agro
	public event Action<Health, int> TakeHealing;
	public event Action<Health> FullHealth;
	public event Action<Health> ZeroHealth;

	void Start () {

		healthPoints = maxHealthPoints;
	
	}

	public void InflictDamage(int damageAmount, MonoBehaviour sender, float agroMutliplier = 1f ) {

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
			TakeDamage(this, damageAmount, Mathf.RoundToInt( (float) damageAmount * agroMutliplier ), sender);

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
