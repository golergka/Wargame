using UnityEngine;
using System.Collections;

public class ProjectileAttack : Attack {

	public Transform projectile;
	public float spawnDistance = 0.1f;
	public float throwForce;

	protected override void ApplyDamage() {

		if (projectile == null) {
			Debug.LogWarning("Projectile not set up!");
			return;
		}

		Vector3 direction = target.transform.position - transform.position;
		direction.Normalize();

		Transform projectileInstance =
		(Transform) Instantiate(projectile, transform.position + new Vector3(0f, spawnDistance, 0f), transform.rotation);

		if (projectileInstance.rigidbody == null) {
			Debug.LogWarning("Projectile doesn't have rigidbody!");
			return;
		}

		if (Physics.gravity.x != 0 || Physics.gravity.z != 0) {
			Debug.LogError("This algorithm was created for vertical gravity only.");
			return;
		}

		// Algorythm copied from the almighty Wikipedia
		// http://en.wikipedia.org/wiki/Trajectory_of_a_projectile#Angle_of_reach
		// Please sent a donation if you're reading this source code

		// Vector3 targetPosition = target.transform.position;
		// Vector3 startPosition = projectileInstance.position;

		// Vector3 delta = targetPosition - startPosition;

		// float height = startPosition.y - targetPosition.y;
		// float g = -Physics.gravity.y;
		
		// float d = ( Vector2(startPosition.x, startPosition.y) - Vector2(targetPosition.x, targetPosition.y) ).magnitude;
		// float angle = Mathf.Asin(g*d/(throwForce*throwForce));

		projectileInstance.rigidbody.AddForce(direction * throwForce + new Vector3(0f,3f,0f), ForceMode.Impulse );

	}

}
