using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * CLASS PowerUpEffects : MonoBehaviour
 * ------------------------------------
 * Produces visual-audio effects for any power up
 * ------------------------------------
 */

public class PowerUpEffects : MonoBehaviour, IActivateable
{
    [SerializeField]
    private GameObject root;    // Root game object for all visual effects
    [SerializeField]
    private AudioSource audioSource;    // Plays audio clips for the power up
    [SerializeField]
    private GameObject chargeUpParticle;    // Particle effect displayed when the power up is charged up
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private float deathTime;    // Time for which the death animation plays before disabling the object
    [SerializeField]
    protected float chargeUpTime; // Time for which charge up animation plays before disabling the object
    [SerializeField]
    private AudioClip chargeClip;   // Audio clip plays when the power up is collected
    [SerializeField]
    private AudioClip deathClip;    // Audio clip plays if the power up is destroyed
    [SerializeField]
    private AudioClip beepClip; // Audio clip plays to warn the player the power up is about to be destroyed
    private bool isActive;

    public bool IsActive { get { return isActive; } }

    public void Idle ()
    {
        anim.SetTrigger("idle");
    }
    public virtual void ChargeUp ()
    {
        SoundPlayer.PlaySoundEffect(chargeClip, audioSource);
        anim.SetTrigger("chargeUp");
        chargeUpParticle.SetActive(true);
        Invoke("Disable", chargeUpTime);
    }
    public void PrepDie (float prepTime)
    {
        anim.SetTrigger("prepDie");
        CancelInvoke();
        Invoke("Die", prepTime);
    }
    public void Die ()
    {
        SoundPlayer.PlaySoundEffect(deathClip, audioSource);
        anim.SetTrigger("die");
        Invoke("Disable", deathTime);
    }
    public void Activate (bool active)
    {
        Idle();
        isActive = active;
        root.SetActive(active);
        chargeUpParticle.SetActive(false);
    }
    private void Disable ()
    {
        Activate(false);
    }
    // Play the beeping sound
    public void Beep()
    {
        SoundPlayer.PlaySoundEffect(beepClip, audioSource);
    }
}
