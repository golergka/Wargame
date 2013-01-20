using System;
using UnityEngine;
using System.Collections;

interface IAttackListener {

	// отсылается только когда AutoAttack теряет цель, выбранную специально
	void OnLostTarget();
	void OnApplyDamage();

}

[RequireComponent(typeof(ParameterManager))]
public class Attack : MonoBehaviour {

	private Health _target;

	string TargetToString() {

		if (_target == null)
			return "null";

		return _target.ToString();

	}

	protected Health target {

		get { return _target; }

		set {

			if (_target == value)
				return;

			_target = value;

		}

	}

	public const string ATTACK_PERIOD_KEY = "Attack period";
	public const string ATTACK_RANGE_KEY  = "Attack range";

	public float period {

		get {

			return parameterManager.GetParameterValue<float>(ATTACK_PERIOD_KEY);

		}

	}
	public float range {

		get {

			return parameterManager.GetParameterValue<float>(ATTACK_RANGE_KEY);

		}

	}

	MonoBehaviour _responsible = null;
	public MonoBehaviour responsible {
		get {

			if (_responsible == null)
				return this;
			else
				return _responsible;

		}

		set { _responsible = value; }

	}

	ParameterManager parameterManager;

	void Awake() {

		parameterManager = GetComponent<ParameterManager>();

	}

#region Messaging

	private Component[] attackListeners;

	// Use this for initialization
	void Start () {

		attackListeners = GetComponents(typeof(IAttackListener));
		Vision vision = GetComponent<Vision>();
		if (vision != null)
			vision.LostVisible += OnLostTarget;
	
	}

	void SendLostTargetMessage() {

		foreach(Component listener in attackListeners)
			((IAttackListener)listener).OnLostTarget();

	}

#endregion

#region Public interface

	// check if it's a suitable target
	public bool CheckTarget(Health target) {

		if (target == null) {

			Debug.LogWarning("Received null target");
			return false;

		}

		// TODO: Implement
		return true;

	}

	public bool TryAppointTarget(Transform target) {

		if (target == null)
			return false;

		Health health = target.GetComponent<Health>();

		if (health == null)
			return false;

		if (CheckTarget(health)) {

			this.target = health;
			return true;

		} else {

			return false;

		}

	}

	public void DropTarget() {

		target = null;

	}

#endregion
	
#region Performing attack

	float lastAttackTime = 0;

	public const string DAMAGE_KEY = "Damage";

	int damage {

		get {

			return parameterManager.GetParameterValue<int>(DAMAGE_KEY);

		}

	}

	protected virtual void ApplyDamage() {

		target.InflictDamage( damage, responsible );
		foreach(Component listener in attackListeners)
				((IAttackListener)listener).OnApplyDamage();

	}

	public void OnLostTarget(Vision vision, Visible observee) {

		Health target = observee.GetComponent<Health>();
		if ( target != null && this.target == target ) {

			target = null;
			SendLostTargetMessage();

		}

	}

	// Update is called once per frame
	void Update () {

		if (target == null)
			return;

		float timePassed = Time.time - lastAttackTime;

		if ( timePassed < period )
			return;

		float targetDistance = Vector3.Distance(transform.position, target.transform.position);

		if ( targetDistance > range )
			return;

		ApplyDamage();
		lastAttackTime = Time.time;
	
	}

#endregion

}
