using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * CLASS MinionEffects : MonoBehaviour
 * -----------------------------------
 * Base class that manages effects with the same names and behaviours
 * across all minions
 * -----------------------------------
 */ 
public class MinionEffects : MonoBehaviour, IBoundaryExitHandler, IActivateable 
{
	[SerializeField]
	private GameObject root;	// Root game object for all sprites, particles, etc.
	[SerializeField]
	protected Animator anim;	// Animator for the object
    [SerializeField]
    protected AudioSource audioSource;  // Audio source for the minion
	[SerializeField]
	private GameObject damageOverlay;	// Flashes when object takes damage
	[SerializeField]
	private GameObject chargeUpOverlay; // Flashes when object is charged up
    [SerializeField]
    private GameObject deathParticle;   // Particle effect appears when the minion dies
    [SerializeField]
	private GameObject prepReEnterParticle;	// Particles appear while minion is preparing to re enter the battlefield
	[SerializeField]
	private GameObject reEnterParticle;	// Particles appear once the minion re enters the battlefield
	[SerializeField]
	private float flashEffectTime;	// Time for which object flashes when taking damage or charging up
	[SerializeField]
	private float deathEffectTime;  // Time for which death effect appears before disappearing
    [SerializeField]
    private AudioClip prepReturnClip; // Clip plays when the minion is preparing to return to the battlefield
    [SerializeField]
    private List<AudioClip> damageClips;    // A random selection is made from these clips when the minion is damaged
    [SerializeField]
    private List<AudioClip> deathClips; // A random selection is made from these clips when the minion dies
    [SerializeField]
    protected List<AudioClip> returnClips; // A random selection is made from these clips when the minion returns from battle, or when they launch an attack

    private bool isActive;  // True if the effects of the minion are active

    public bool IsActive { get { return isActive; } }

	// Randomly restart the idle effect after a certain amount of time
	// so minions that are the same have idle animations offset
	private void Start ()
	{
		float rand = Random.Range (0f, 2f);
		Invoke ("Idle", rand);
	}

	// Enable/disable visual by simply enabling/disabling the
	// root game object of all the sprites, particles, etc.
	private void SetVisual (bool enabled)
	{
		root.SetActive (enabled);
	}

	// Used animator to put object into the idle state
	public void Idle ()
	{
		anim.SetTrigger ("idle");
	}

	// Activate the damage overlay, and make it deactivate after a certain amount of time
	public void TakeDamage ()
	{
		damageOverlay.SetActive (true);
		chargeUpOverlay.SetActive (false);
		Invoke ("DisableOverlays", flashEffectTime);
        SoundPlayer.PlayRandomEffect(damageClips, audioSource);
	}

	// Use animator to cause death animation
	public void Die ()
	{
		anim.SetTrigger ("die");
        deathParticle.SetActive(true);
		Invoke ("DisableVisual", deathEffectTime);
        SoundPlayer.PlayRandomEffect(deathClips, audioSource);
    }

	// Activate the charge up overlay, and make it deactivate after preset time
	public virtual void ChargeUp ()
	{
		chargeUpOverlay.SetActive (true);
		damageOverlay.SetActive (false);
		Invoke ("DisableOverlays", flashEffectTime);
	}

	// Use animation coupled with particle effect
	public virtual void PrepareReEntry (float prepTime)
	{
        SetVisual(true);
		anim.SetTrigger ("return");
        deathParticle.SetActive(false);
        reEnterParticle.SetActive(false);
        prepReEnterParticle.SetActive (true);
        SoundPlayer.PlaySoundEffect(prepReturnClip, audioSource);
		Invoke ("ReEnter", prepTime);
	}

	// Disable prepare particle and activate re-enter particle
	public void ReEnter ()
	{
		prepReEnterParticle.SetActive (false);
		reEnterParticle.SetActive (true);
        SoundPlayer.PlayRandomEffect(returnClips, audioSource);
        Idle();
	}

	// Invoked to disable overlays after a certain amount of time
	private void DisableOverlays ()
	{
		damageOverlay.SetActive (false);
		chargeUpOverlay.SetActive (false);
	}

	// Invoked to disable visual after a certain amount of time 
	private void DisableVisual ()
	{
		Activate(false);
	}

    public void OnBoundaryExit ()
    {
        Activate(false);
    }

    public virtual void Activate (bool active)
    {
        isActive = active;
        SetVisual(active);
        DisableOverlays();
        deathParticle.SetActive(false);
        reEnterParticle.SetActive(false);
        prepReEnterParticle.SetActive(false);
    }
}