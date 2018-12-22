using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * CLASS Bogey : Minion
 * --------------------
 * A type of minion without any weapons.  When charged up,
 * it attempts to fly straight at the player and ram into it
 * --------------------
 */ 

public class Bogey : Minion 
{
	private Transform target;	// Position of the object this bogey targets when powered up
	[SerializeField]
	private float suicideSpeed;	// Speed at which the bogey charges at the player when charged up
	private FormationObject formation;	// Formation script on this object, if it has one

	protected void Start ()
	{
		target = FindObjectOfType<Player> ().transform;
		formation = GetComponent <FormationObject> ();
	}

    // If the bogey dies, return control of its formation
    // to the base formation script
    public override void Kill ()
    {
        base.Kill();
        AttemptFormationOverride(false);
    }

	// When charged up, the Bogey flies straight at the player in attempt to ram into them
	public override void ChargeUp (int chargeGiven)
	{
        base.ChargeUp(chargeGiven);

		Vector2 toTarget;	// A vector with its tip at the target and tail at this object

		// Attempt to override a formation script on this object
		AttemptFormationOverride (true);

		// Move towards the target with the preset speed
		toTarget = (Vector2)(target.position - transform.position);
		mover.MoveTowards (toTarget, suicideSpeed);
	}

	// Attempt to get a formation script on this object and override it
	public void AttemptFormationOverride (bool toOverride)
	{
		// If this is a standalone bogey, access its formation script
		if (type == MinionType.Standalone) {
			// If we have a formation script, override it
			if (formation != null) {
				formation.Override (toOverride);
			}
			// Otherwise, log a warning - all standalone bogeys should have a formation object script
			else {
				Debug.LogWarning (gameObject.name + " is marked \"Standalone\" but it doesn't have a FormationObject script");
			} // END inner if
		} // END outer if
	} // END method
}
