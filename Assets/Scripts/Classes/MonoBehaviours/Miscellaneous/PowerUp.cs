using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * CLASS PowerUp : MonoBehaviour
 * -----------------------------
 * Describes an object that, when hit by a green bullet, powers up the player
 * -----------------------------
 */ 

public class PowerUp : MonoBehaviour, IActivateable, IDamageable, IKillable, IChargeable 
{
	[SerializeField]
	private Mover2D mover;
	[SerializeField]
	private PowerUpType type;	// Type of this power up
	private bool isActive;
    [SerializeField]
	private Collider2D col;	// Collider on this object
	private Player player;	// Reference to the player script active in the scene
	[SerializeField]
	private float warnKillTime; // Time between when the power up hits the limit of the player boundary and when it is destroyed
    [SerializeField]
    private PowerUpEffects effects; // Script manages visual-audio effects

	public bool IsActive { get { return isActive; } }

	private void Start ()
	{
		col = GetComponent<Collider2D> ();
		player = FindObjectOfType<Player> ();
	}

	// Power up the player, then deactivate
	public void ChargeUp (int chargeGiven)
	{
		player.PowerUp (type);
        Activate (false);
        effects.ChargeUp();
    }

	// Kill the power up when it takes damage
	public void TakeDamage (int damageTaken)
	{
		Kill ();
	}

	public void Kill ()
	{
        effects.Die();
		Activate (false);
	}

	public void Activate (bool active)
	{
		isActive = active;
		col.enabled = active;
		CancelInvoke ();

        // If we're being disabled...
        if (!active)
        {
            //...stop moving
            mover.Stop();
        }
	}

	// Causes the power up to be destroyed when it reaches the player boundary
	private void OnTriggerEnter2D (Collider2D other)
	{
		if (other.tag == "Player Boundary") {
			mover.Stop ();
			CancelInvoke ();
			Invoke ("Kill", warnKillTime);

            // Use an effect to warn the player that the power up is about to disappear
            effects.PrepDie(warnKillTime);
		} // END if
	} // END method
}

public enum PowerUpType
{
	Power,
	FireRate,
	Shield,
	Bomb
}