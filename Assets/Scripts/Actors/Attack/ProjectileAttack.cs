using UnityEngine;
using System.Collections;

public class ProjectileAttack : Attack {

	const float FORTYFIVEDEGREES = 0.785398163f; // google, muthafucka
	const float APPROX_SPEED = 0.2f;

	public Transform projectile;
	public float spawnDistance = 1f; // distance above the transform to spawn the projectile
	public float throwForce;
	public bool upwardTrajectory = true;

	protected override void ApplyDamage() {

		if (projectile == null) {
			Debug.LogWarning("Projectile not set up!");
			return;
		}		

		Transform projectileInstance =
		(Transform) Instantiate(projectile, transform.position + new Vector3(0f, spawnDistance, 0f), transform.rotation);

		AgroResponsible.MakeResponsible(projectileInstance, this);

		if (projectileInstance.rigidbody == null) {
			Debug.LogWarning("Projectile doesn't have rigidbody!");
			return;
		}

		if (Physics.gravity.x != 0 || Physics.gravity.z != 0) {
			Debug.LogWarning("This algorithm was created for vertical gravity only.");
			return;
		}

		Vector3 targetPosition = target.transform.position;

		TransformVelocity targetVelocity = target.GetComponent<TransformVelocity>();

		if (targetVelocity != null) {

			float approxTime = (targetPosition - projectileInstance.transform.position).magnitude / ( APPROX_SPEED * throwForce );
			Vector3 delta = targetVelocity.velocity * approxTime;

			Debug.DrawLine(target.transform.position, target.transform.position + delta, Color.white, 1f, false );

			targetPosition += delta;

		}

		// Algorythm copied from the almighty Wikipedia
		// http://en.wikipedia.org/wiki/Trajectory_of_a_projectile#Angle_required_to_hit_coordinate_.28x.2Cy.29
		// Please sent a donation if you're reading this source code. Wikipedia is like super-awesome. Really.

		// TODO: add target current speed to estimate it's position at the time of impact
		// Will have to do a little trigonometry one day, won't we?
		Vector3 horizontalDirection = targetPosition - projectileInstance.transform.position;
		horizontalDirection.y = 0; // it has to be normalized later, because we'll need it's magnitude

		float x = horizontalDirection.magnitude; // horizontal direction isn't normalized yet
		float y = targetPosition.y - projectileInstance.position.y;

		float g = Physics.gravity.y;
		float toRoot = Mathf.Pow(throwForce,4) - g * (g*x*x + 2*y*throwForce*throwForce);

		float angle;

		if (toRoot < 0) { // checking if we have a solution at all

			Debug.Log("Distance too great, trying 45 degrees");
			angle = FORTYFIVEDEGREES;

		} else {

			// TODO: choose the root depending on whether we can shoot directly
			// Now we just choose the bigger root (the plus in the middle) because that way we're more likely to clear the fence
			// However, if there's no fence, we probably should choose the smaller root so that projectile hits target faster
			if (upwardTrajectory)
				angle = Mathf.Atan2(throwForce*throwForce + Mathf.Sqrt(toRoot), g*x);
			else
				angle = Mathf.Atan2(throwForce*throwForce - Mathf.Sqrt(toRoot), g*x);

		}

		horizontalDirection.Normalize();

		Vector3 throwForceVector = new Vector3(
			horizontalDirection.x * throwForce * Mathf.Abs( Mathf.Cos(angle) ),
			throwForce * Mathf.Sin(angle),
			horizontalDirection.z * throwForce * Mathf.Abs( Mathf.Cos(angle) )
			);

		projectileInstance.rigidbody.AddForce(throwForceVector , ForceMode.Impulse );

	}

}
