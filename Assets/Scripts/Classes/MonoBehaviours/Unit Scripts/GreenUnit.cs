using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * CLASS GreenUnit : MonoBehaviour
 * -------------------------------
 * Defines the central unit of the game.  If it dies,
 * you have to start over.  You win the level by fully
 * charging up this unit
 * -------------------------------
 */ 

public class GreenUnit : MonoBehaviour, IDamageable, IKillable, IChargeable, IActivateable 
{
	private Player player;	// Reference to the player in the scene
    [SerializeField]
	private Collider2D col;	// Circle collider on this object
    [SerializeField]
    private Mover2D mover;  // Script that moves the unit
	[SerializeField]
	private int maxCharge;
	private int charge = 0;	// Current charge of the unit
	[SerializeField]
	private int maxHealth;
	private int health;	// Current health of the unit
	private bool isActive;	// True if the object is currently active
    [SerializeField]
    private GreenUnitEffects maleEffects;   // Script manages effects for male sprite
    [SerializeField]
    private GreenUnitEffects femaleEffects; // Script manages effects for female sprite
    private GreenUnitEffects effects;   // Script manages the effects of the green unit

	public bool IsActive { get { return isActive; } }
	// Property returns true if the unit is fully charged
	public bool fullyCharged
	{
		get {
			return charge >= maxCharge;
		} // END get
	} // END property
    // True if the unit is dead, false otherwise
    public bool isDead
    {
        get
        {
            return health <= 0;
        }
    }

	private void Start ()
	{
		player = FindObjectOfType<Player> ();
		health = maxHealth;

		Activate (true);
		GameManager.Instance.Subscribe (this);

        // Decide randomly if the current unit
        // will be male or female
        int rand = Random.Range(0, 2);
        if (rand == 0)
        {
            effects = maleEffects;
        }
        else
        {
            effects = femaleEffects;
        }
        effects.Root.SetActive(true);
	}

	// Add charge to the unit by amount specified
	public void ChargeUp (int chargeGiven)
	{
		charge += chargeGiven;
        effects.ChargeUp();

		// If charge is maxed out, call corresponding method
		if (fullyCharged) {
			OnFullCharge ();
		} // END if
	} // END method

	private void OnFullCharge ()
	{
		int powerUpChosen;	// Power up chosen to power up the player with
		bool lastUnit;	// True if this is the last green unit to become fully charged

		// Deactivate and make the game manager check the status of the active green units
		Activate (false);
		lastUnit = GameManager.Instance.CheckGreenUnitCharge ();

		// If this was not the last unit to charge up...
		if (!lastUnit) {
			//...give the player a random power up
			powerUpChosen = Random.Range (0, 3);
			//player.PowerUp ((PowerUpType)powerUpChosen);
		} // END if

        // Create the full charge effect
        effects.OnFullCharge();
	}

	// The green unit takes damage independent of the strength of the object that hit it
	public void TakeDamage (int damageTaken)
	{
		health--;
	
		// If health is depleted, kill it
		if (health <= 0) {
			Kill ();
		}
        // Otherwise, if the unit didn't die...
        else
        {
            //...invoke the damage effect
            effects.TakeDamage();
        }
	} // END method

	public void Kill ()
	{
		Activate (false);
        effects.Die();
		GameManager.Instance.EndGame (EndType.GreenUnitKilled);
	}

	// Enable the collider on the object
	public void Activate (bool active)
	{
		isActive = active;
		col.enabled = active;

        // If we are deactivating, stop moving
        if (!active)
        {
            mover.Stop();
        }
	}
}
