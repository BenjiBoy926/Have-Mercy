using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * CLASS Cruiser : Minion
 * ----------------------
 * Defines a type of minion that shoots straight ahead.  When powered up,
 * it immediately fires a higher-velocity round straight ahead
 * ----------------------
 */ 

public class Cruiser : Minion 
{
	[SerializeField]
	private MinionGun gun;	// Contains details about the minions gun
	[SerializeField]
	private float powerShotSpeed;	// Speed of the enemy's power shot
    [SerializeField]
    private ActivationEffects plusEffects; // Override of the minion effects script provides additional effects for cruiser shooting 

	protected void Start ()
	{
		gun.InitializePool ();
	}

	// Set shield to one
	public override void ChargeUp (int chargeGained)
	{
        base.ChargeUp(chargeGained);

		// Suspend attack
		StopCoroutine ("AttackRoutine");

		// Immediately launch a bullet that travels faster
		gun.Launch (powerShotSpeed);

		// Resume attack
		StartCoroutine ("AttackRoutine");

        // Use script to create an effect
        plusEffects.Act();
        plusEffects.ActSound();
	}
		
	public override void Activate (bool active)
	{
		base.Activate (active);

		// If activating, restart the attack coroutine
		if (active) {
			StopCoroutine ("AttackRoutine");
			StartCoroutine ("AttackRoutine");
		}
		// Otherwise, if deactivating, stop the attack coroutine
		else {
			StopCoroutine ("AttackRoutine");
		} // END if
	} // END method

	// An infinite loop that causes the cruiser to continue shooting as long as it is alive
	private IEnumerator AttackRoutine ()
	{
		float waitTime;	// Initial wait time for the coroutine
        WaitForSeconds warning = new WaitForSeconds(gun.WarningTime);

		while (true) {
			// Randomize wait time and use it for the wait command
			waitTime = Random.Range (gun.BaseFireRate.min, gun.BaseFireRate.max + 1);
			yield return new WaitForSeconds (waitTime);

            // Creates anticipation animation for gun to shoot
            plusEffects.PrepareAct();

			yield return warning;

            // Shoot the gun using its default values,
            // and produce an effect
			gun.Launch ();
            plusEffects.Act();
		} // END while
	} // END routine
} // END class