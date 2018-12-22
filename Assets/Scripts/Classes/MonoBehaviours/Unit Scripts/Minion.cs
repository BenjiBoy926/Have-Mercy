using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * CLASS Minion : MonoBehaviour, IHealthHandler, IChargeable, IActivateable
 * ------------------------------------------------------------------------
 * Base class for all enemy minion types; standalone ships, spawned by a
 * carrier, or installed on a boss
 * ------------------------------------------------------------------------
 */ 

public class Minion : MonoBehaviour, IDamageable, IKillable, IChargeable, IActivateable, IBoundaryExitHandler
{
	[SerializeField]
	protected MinionType type;	// The minion's type
	private Collider2D col;
	[SerializeField]
	private int maxHealth;	// Max health of the minion
	private int health;	// Current health of the minion
	[SerializeField]
	private int rammingStrength;	// Damage dealt to the player if the enemy rams them
	[SerializeField]
	private float returnWarningTime;	// Time before minion returns that they signal their return
	private bool isActive;	// True if the minion is active
    [SerializeField]
	protected MinionEffects effects;	// Script manages higher-level visual effects of the minion
    protected Mover2D mover;    // Reference to the script, if any, that moves the minion

	public bool IsActive { get { return isActive; } }

	protected virtual void Awake ()
	{
        mover = GetComponent <Mover2D>();
		col = GetComponent <Collider2D> ();
		Activate (type != MinionType.Spawned);
        effects.Activate(type != MinionType.Spawned);

		// Let the game manager know that this minion entered the battlefield
		GameManager.Instance.Subscribe (this);
	}

	// Charge up method declared in the abstract, since minion types
	// perform very different functions when charged up
	public virtual void ChargeUp (int chargeGiven)
	{
        effects.ChargeUp();
	}

	// Reduces health by amount specified
	public virtual void TakeDamage (int damageTaken)
	{
		health -= damageTaken;
        effects.TakeDamage();

		// If health is depleted, kill the minion
		if (health <= 0) {
			Kill ();
		} // END if
	} // END method

	// Deactivate the object and create an effect
	public virtual void Kill ()
	{
		Activate (false);
        effects.Die();
		TryResurrect ();
        TryStop ();
	} // END method

	// Activate the object by enabling/disabling the
	// physical presence of the minion
	public virtual void Activate (bool active)
	{
		isActive = active;
		col.enabled = active;

		// If the object is reactivating, reset health
		if (active) {
			health = maxHealth;
		}
		// If the object is deactivating, stop all coroutines 
		else {
			StopAllCoroutines ();
		} // END if
	} // END method

	// Disable the object when it exits the boundary
	public void OnBoundaryExit ()
	{
		Activate (false);
		TryResurrect ();
	}

	// Resurrect the minion if they aren't a spawned type
	private void TryResurrect ()
	{
		if (type != MinionType.Spawned) {
			StopCoroutine ("ReturnToBattle");
			StartCoroutine ("ReturnToBattle", GameManager.Instance.RespawnTime);
		} // END if
	} // END method

	// Make the minion damage the player if they come into contact with it
	private void OnTriggerEnter2D (Collider2D other)
	{
		// Attempt to get a player script on the object and store the result
		Player attemptPlayer = other.GetComponent <Player> ();

		// If we got a reference to a player, make them take damage
		if (attemptPlayer != null) {
			attemptPlayer.TakeDamage (rammingStrength);
		} // END if
	} // END method

	// Cause minion to return to battle after set amount of time
	private IEnumerator ReturnToBattle (float respawnTime)
	{
		yield return new WaitForSeconds (respawnTime);
        effects.PrepareReEntry(returnWarningTime);
		yield return new WaitForSeconds (returnWarningTime);
		Activate (true);
        effects.ReEnter();
	}

    // If the minion has a mover, stop it
    private void TryStop ()
    {
        if (mover != null)
        {
            mover.Stop();
        }
    }
}

public enum MinionType
{
	Standalone,
	Spawned,
	Installation
}