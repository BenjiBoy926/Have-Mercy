using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * CLASS BogeyEffects : MinionEffects
 * ----------------------------------
 * Modifies functions in the MinionEffects class to create additional
 * effects peculiar to the Bogey unit
 * ----------------------------------
 */ 

public class BogeyEffects : MinionEffects 
{
    [SerializeField]
    private GameObject trailObj;    // Object with a trail particle effect 
    
	public override void ChargeUp ()
	{
		base.ChargeUp ();
        trailObj.SetActive(true);
		anim.SetTrigger ("chargeUp");
        SoundPlayer.PlayRandomEffect(returnClips, audioSource);
	}

    public override void PrepareReEntry (float prepTime)
    {
        base.PrepareReEntry(prepTime);
        trailObj.SetActive(false);
    }

    public override void Activate (bool active)
    {
        base.Activate(active);
        trailObj.SetActive(false);
    }
}
