using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * CLASS Gunner : Minion
 * ---------------------
 * A type of minion that shoots straight at the player.
 * When hit with a power up bullet, this enemy immediately
 * shoots a high-velocity round straight at the player
 * ---------------------
 */ 

public class Gunner : Minion 
{
	[SerializeField]
	private MinionGun gun;
	private Transform target;	// Transform of the target this unit aims at
	[SerializeField]
	private float powerShotSpeed;	// Speed of the bullet the enemy launches when hit with a power up bullet
    [SerializeField]
    private ActivationEffects plusEffects;  // Provides additional effects methods for when the gunner shoots

    public Transform Target { get { return target; } }

	protected void Start ()
	{
		target = FindObjectOfType<Player> ().transform;
		gun.InitializePool ();
	}

	// Immediately fire a high-velocity round at the player
	public override void ChargeUp (int chargeGiven)
	{
        base.ChargeUp(chargeGiven);

		// Stop the attack routine
		StopCoroutine ("AttackRoutine");

		// Immediately fire a power shot at the player
		FireAtTarget (powerShotSpeed);

		// Resume the attack routine
		StartCoroutine ("AttackRoutine");

        plusEffects.Act();
        plusEffects.ActSound();
	}

	public override void Activate (bool active)
	{
		base.Activate (active);

		// If activating...
		if (active) {
			//...restart the attack routine
			StopCoroutine ("AttackRoutine");
			StartCoroutine ("AttackRoutine");
		}
		// Otherwise, if deactivating, stop the attack coroutine
		else {
			StopCoroutine ("AttackRoutine");
		} // END if-else
	} // END method

	private IEnumerator AttackRoutine ()
	{
		float waitTime;
        WaitForSeconds warning = new WaitForSeconds(gun.WarningTime);

		while (true) {
			// Find a random wait time within the constraints and use it in the wait statement
			waitTime = Random.Range (gun.BaseFireRate.min, gun.BaseFireRate.max + 1);
			yield return new WaitForSeconds (waitTime);

            plusEffects.PrepareAct();

			yield return warning;
			// ANOTHER EFFECT YAY

			// Fire at the target with default bullet velocity
			FireAtTarget (gun.Gun.DefaultSpeed);
            plusEffects.Act();
		} // END while
	} // END coroutine

	// Launches a bullet straight at the target
	// with the specified speed
	private void FireAtTarget (float speed)
	{
		Vector2 toTarget;	// A vector with its tip at the target and its tail at this object

		// Get the to target vector and the local origin by re-scaling it
		toTarget = (Vector2)(target.position - transform.position);
		gun.Launch (toTarget, speed);
	}
} // END class