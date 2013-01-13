using System;
using UnityEngine;
using System.Collections;

interface IAttackListener {

	// отсылается только когда AutoAttack теряет цель, выбранную специально
	void OnLostTarget();
	void OnApplyDamage();

}

public class Attack : MonoBehaviour, IVisionListener {

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

	public float period = 1f;
	public float range = 5f;
	public int damage = 10;

#region Messaging

	private Component[] attackListeners;

	// Use this for initialization
	void Start () {

		attackListeners = GetComponents(typeof(IAttackListener));
	
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

	protected virtual void ApplyDamage() {

		target.InflictDamage( damage );
		foreach(Component listener in attackListeners)
				((IAttackListener)listener).OnApplyDamage();

	}

	public void OnNoticed(Visible observee) {

		// Я сделан из мяса.

	}

	public void OnLost(Visible observee) {

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
