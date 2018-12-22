using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * CLASS PlayerEffects : MonoBehaviour
 * -----------------------------------
 * Uses an animator to create visual effects for the player
 * -----------------------------------
 */ 

public class PlayerEffects : MonoBehaviour
{
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private Animator shieldAnim;    // Animator for the shield sprite
    [SerializeField]
    private Animator fireAnim;  // Animator on the fire that activates when player is powered up
    [SerializeField]
    private AudioSource audioSource;    // Audio source for the player
    [SerializeField]
    private GameObject windParticle;    // Wind particle effect when the player's fire rate is increased
    [SerializeField]
    private GameObject genericPowerUpParticle;  // Particle that activates when the player is powered up
    [SerializeField]
    private GameObject deathParticle;   // Particle system produces an effect when the player dies
    [SerializeField]
    private float deathBurstDelay;  // Delay between death animation and death particle burst
    [SerializeField]
    private GameObject mainSprites; // Root object for the main sprites of the player
    [SerializeField]
    private ColorMod damagePanel;   // Script that manages panel that flashes when the player is damaged
    [SerializeField]
    private float winDelay; // Delay between winning the stage and playing the victory animation
    [SerializeField]
    private AudioClip damageClip;   // Audio plays when player is damaged
    [SerializeField]
    private AudioClip deathClip;    // Audio plays when player dies
    [SerializeField]
    private AudioClip ascendClip;   // Audio plays when player ascends after winning the game
    [SerializeField]
    private List<AudioClip> attackClips;    // Played when the player attacks

    // Use corresponding animation triggers
    public void RedShot ()
    {
        anim.SetTrigger("redShot");
        SoundPlayer.PlayRandomEffect(attackClips, audioSource);
    }
    public void GreenShot ()
    {
        anim.SetTrigger("greenShot");
        SoundPlayer.PlayRandomEffect(attackClips, audioSource);
    }

    // Use animation trigger and cause particle effect after time
    public void Die ()
    {
        SoundPlayer.PlaySoundEffect(damageClip, audioSource);
        damagePanel.Transition(false);
        anim.SetTrigger("die");
        Invoke("DeathBurst", deathBurstDelay);
    }

    // Disable main sprites and create particle effect
    private void DeathBurst ()
    {
        SoundPlayer.PlaySoundEffect(deathClip, audioSource);
        anim.SetTrigger("exit");
        mainSprites.SetActive(false);
        deathParticle.SetActive(true);
    }

    // Play an animation when the player wins, after a short time
    public void OnWin ()
    {
        Invoke("WinAnim", winDelay);
    }
    private void WinAnim ()
    {
        SoundPlayer.PlaySoundEffect(ascendClip, audioSource);
        anim.SetTrigger("victory");
    }

    // Produce different effect depending on power up type
    public void PowerUp (PowerUpType type, float powerUpTime)
    {
        switch (type)
        {
            case PowerUpType.Shield:
                shieldAnim.SetTrigger("activate");
                Invoke("DeactivateShield", powerUpTime);
                break;
            case PowerUpType.Power:
                fireAnim.SetTrigger("appear");
                Invoke("DeactivateFlame", powerUpTime);
                break;
            case PowerUpType.FireRate:
                windParticle.SetActive(true);
                Invoke("DeactivateWind", powerUpTime);
                break;
        }

        if (type == PowerUpType.Shield || 
            type == PowerUpType.Power || 
            type == PowerUpType.FireRate)
        {
            genericPowerUpParticle.SetActive(false);
            genericPowerUpParticle.SetActive(true);
        }
    }
    // Calls trigger to create effect when shield is stricken
    public void ShieldStruck ()
    {
        shieldAnim.SetTrigger("strike");
    }
    public void DeactivateShield ()
    {
        shieldAnim.SetTrigger("deactivate");
    }
    // Use trigger to make flame vanish
    public void DeactivateFlame ()
    {
        fireAnim.SetTrigger("disappear");
    }
    // Disable wind particles
    public void DeactivateWind ()
    {
        windParticle.SetActive(false);
    }
}
