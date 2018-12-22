using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * CLASS Carrier : Minion
 * ----------------------
 * Defines a minion that can regularly spawn other minions
 * from a central pool of minions.  When charged up, the carrier
 * obtains a shield that resists two bullets
 * ----------------------
 */ 

public class Carrier : Minion 
{
	[SerializeField]
	private Spawner spawner;
	[SerializeField]
	private float warningTime;	// Time before spawning the next enemy that an animation plays to warn the player
	private bool invincible;	// True if the carrier is currently invincible
	[SerializeField]
	private float invincibleTime;	// Time for which the carrier is invincible when charged up
    [SerializeField]
    private CarrierEffects plusEffects;  // Provides additional effects for the carrier's spawning movement

    public float InvincibleTime { get { return invincibleTime; } }

	protected void Start ()
	{
		spawner.InitializePools ();
	}

	// When the carrier is charged up, it gains a strength that resists two bullets
	public override void ChargeUp (int chargeGiven)
	{
        base.ChargeUp(chargeGiven);

		// Make invincible true, 
		// then make it false again after a set amount of time 
		CancelInvoke ();
		invincible = true;
		Invoke ("DefaultInvincibility", invincibleTime);

        plusEffects.ChargeUp();
	} // END method

	// Carrier only takes damage if they are not invincible
	public override void TakeDamage (int damageTaken)
	{
		if (!invincible) {
			base.TakeDamage (damageTaken);
		} else {
            plusEffects.ShieldHit();
		} // END if-else
	} // END method

	public override void Activate (bool active)
	{
		base.Activate (active);

		// If activating, restart the attack coroutine
		if (active) {
			invincible = false;

			StopCoroutine ("SpawnRoutine");
			StartCoroutine ("SpawnRoutine");
		}
		// Otherwise, if deactivating, stop the attack coroutine
		else {
			StopCoroutine ("SpawnRoutine");
		} // END if
	} // END method

	private IEnumerator SpawnRoutine ()
	{
		Vector2 origin;	// Origin selected for the object to launch
		float originInterpolater;	// Constant used to interpolate between origin coordinates for the spawner
		List<Launchable> chosenPool;	// Chosen pool from a list of launcher lists from which an object will be spawned
		int poolIndex;	// Random index in the list within a list of launchables
		float waitTime;
        WaitForSeconds warning = new WaitForSeconds(warningTime);

		while (true) {
			// Randomly select a wait time and wait for that amount of time
			waitTime = Random.Range (spawner.FireRate.min, spawner.FireRate.max + 1);
			yield return new WaitForSeconds (waitTime);

            // Create an effect to warn the player
            plusEffects.PrepareAct();

			yield return warning;

			// Find a point linearly between two points at which to spawn the next object
			originInterpolater = Random.Range (0f, 1f);
			origin = Vector2.Lerp (spawner.LocalOriginA, spawner.LocalOriginB, originInterpolater);

			// Randomly select one of the pools of launchables from which to spawn the next object
			poolIndex = Random.Range (0, spawner.Pools.Count);
			chosenPool = spawner.Pools [poolIndex].Launchables;
			spawner.LaunchFromPool (chosenPool, origin);

            plusEffects.Act();
		} // END while
	} // END coroutine

	// Make invincibility false
	// Put into a function so that it can be invoked
	private void DefaultInvincibility ()
	{
		invincible = false;
	}
}
