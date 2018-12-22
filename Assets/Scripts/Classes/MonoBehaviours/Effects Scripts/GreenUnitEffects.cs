using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * CLASS GreenUnitEffects : MonoBehaviour
 * --------------------------------------
 * Provides effects specific to the green unit
 * --------------------------------------
 */ 

public class GreenUnitEffects : MonoBehaviour
{
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private AudioSource audioSource;    // Audio source for this object
    [SerializeField]
    private GameObject root;    // Root object for all effects objects
    [SerializeField]
    private GameObject fullChargeParticle;
    [SerializeField]
    private ColorMod damageOverlay;
    [SerializeField]
    private GameObject chargeOverlay;
    [SerializeField]
    private Image blackoutPanel;    // Panel that blacks out the screen when the green unit dies
    [SerializeField]
    private float effectTime;   // Time for which charging and damaging effects linger on-screen
    [SerializeField]
    private float deathEffectDuration;  // Duration of the death effect until the green unit disappears
    [SerializeField]
    private float fullChargeEffectDuration; // Duration of the full charge effect
    [SerializeField]
    private AudioClip shortCrack;   // Brief cracking sound played while unit is dying
    [SerializeField]
    private AudioClip longCrack;    // Longer cracking sound played while unit is dying
    [SerializeField]
    private AudioClip fullChargeClip;   // Sound that plays when the spirit is fully charged
    [SerializeField]
    private AudioClip ascendClip;   // Audio plays over the full charge animation
    [SerializeField]
    private List<AudioClip> damageClips;    // Clip randomly played when the unit is damaged
    [SerializeField]
    private List<AudioClip> deathClips; // Clips played when the unit dies. First is male, second female

    public GameObject Root { get { return root; } }

    public void Idle ()
    {
        anim.SetTrigger("idle");
        DisableOverlays();
    }
    public void TakeDamage ()
    {
        anim.SetTrigger("takeDamage");
        damageOverlay.Transition(false);
        SoundPlayer.PlayRandomEffect(damageClips, audioSource);
        Invoke("Idle", effectTime);
    }
    public void ChargeUp ()
    {
        anim.SetTrigger("chargeUp");
        chargeOverlay.SetActive(true);
        Invoke("Idle", effectTime);
    }
    public void OnFullCharge ()
    {
        CancelInvoke();
        fullChargeParticle.SetActive(true);
        SoundPlayer.PlaySoundEffect(fullChargeClip, audioSource);
        Invoke("DisableOverlays", effectTime);
        Invoke("FullChargeAnim", fullChargeEffectDuration);
    }
    public void FullChargeAnim ()
    {
        SoundPlayer.PlaySoundEffect(ascendClip, audioSource);
        anim.SetTrigger("onFullCharge");
        chargeOverlay.SetActive(false);
        Invoke("DisableVisual", 1f);
    }
    public void Die ()
    {
        blackoutPanel.enabled = true;
        CancelInvoke();
        anim.SetTrigger("die");
        DisableOverlays();
        SoundPlayer.PlayRandomEffect(deathClips, audioSource);
        Invoke("DisableVisual", deathEffectDuration);
    }
    public void DisableOverlays ()
    {
        chargeOverlay.SetActive(false);
    }
    public void DisableVisual ()
    {
        root.SetActive(false);
    }
    public void ShortCrack()
    {
        SoundPlayer.PlaySoundEffect(shortCrack, audioSource);
    }
    public void LongCrack()
    {
        SoundPlayer.PlaySoundEffect(longCrack, audioSource);
    }
}