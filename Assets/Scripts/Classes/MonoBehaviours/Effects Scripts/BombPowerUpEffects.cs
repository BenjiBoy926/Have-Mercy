using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * CLASS BombPowerUpEffects : PowerUpEffects
 * -----------------------------------------
 * Enables a power up effects script to produce and explosion effect
 * at the position of the power up
 * -----------------------------------------
 */ 
public class BombPowerUpEffects : PowerUpEffects
{
    [SerializeField]
    private Animator explosionAnim; // Animator that creates the explosion effect

    // Override of the base charge up adds an explosive animation effect
    public override void ChargeUp ()
    {
        base.ChargeUp();
        explosionAnim.SetTrigger("appear");
        Invoke("DisableExplosion", chargeUpTime - 0.1f);
    }
    // Make explosion disappear with animator
    private void DisableExplosion ()
    {
        explosionAnim.SetTrigger("disappear");
    }
}
