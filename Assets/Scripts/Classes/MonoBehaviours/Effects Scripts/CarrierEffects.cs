using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * CLASS CarrierEffects : ActivationEffects
 * ----------------------------------------
 * Creates additional activation-based effects for the carrier unit,
 * such as allowing the invincibility shield to activate
 * ----------------------------------------
 */ 

public class CarrierEffects : ActivationEffects
{
    [SerializeField]
    private Animator shieldAnim;    // Animator for the shield object
    [SerializeField]
    private Carrier script; // Reference to the carrier script on this object

	public override void ChargeUp ()
    {
        base.ChargeUp();
        shieldAnim.SetTrigger("activate");
        Invoke("PowerDown", script.InvincibleTime);
    }

    private void PowerDown ()
    {
        shieldAnim.SetTrigger("deactivate");
    }

    public void ShieldHit ()
    {
        shieldAnim.SetTrigger("strike");
    }
}
