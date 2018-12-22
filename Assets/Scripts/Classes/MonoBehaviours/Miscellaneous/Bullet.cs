using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * CLASS DamagingObject : MonoBehaviour
 * ------------------------------------
 * Defines a type of object that deals damage to any damageable object
 * it comes into contact with
 * ------------------------------------
 */ 

public class Bullet : Launchable, IActivateable, IBoundaryExitHandler 
{
	[SerializeField]
	private BulletType type;
	[SerializeField]
	private int strength;	// Strength of the bullet
    [SerializeField]
    private Collider2D col; // Collider on the bullet
    [SerializeField]
    private SpriteRenderer mainSprite;  // Main sprite displaying the bullet
    [SerializeField]
    private BulletEffects effects;  // Manages visual-audio effects for the bullet

    private bool isActive;
    public bool IsActive { get { return isActive; } }

	// Allows the strength of the bullet to be assigned
	public void Initialize (int newStrength)
	{
		strength = newStrength;
	}

	// Damage or charge up the object collided with
	private void OnTriggerEnter2D (Collider2D other)
	{
		// If this is a damaging bullet, try to damage something
		if (type == BulletType.Damaging) {
			// Attempt to grab a script that implements IDamageable on the object we collided with
			IDamageable damageable;
			damageable = other.GetComponent <IDamageable> ();
		
			// If we found a damageable object, damage it and disable the bullet
			if (damageable != null) {
				damageable.TakeDamage (strength);
                effects.Collision();

                // Erase the presence of the bullet,
                // then officially deactivate it once the particle is finished displaying
                Present(false);
                Invoke("Disable", effects.ParticleTime);
            } // END if
		}
		// Otherwise, if this is a charging bullet, try charging something
		else {
			// Attempt to grab a script that implements IChargeable on the object we collided with
			IChargeable chargeable;
			chargeable = other.GetComponent <IChargeable> ();

			// If we got a chargeable script, charge it up and disable the bullet
			if (chargeable != null) {
				chargeable.ChargeUp (strength);
                effects.Collision();

                // Erase the presence of the bullet,
                // then officially deactivate it once the particle is finished displaying
                Present(false);
                Invoke("Disable", effects.ParticleTime);
            } // END if
		} // END if-else
	} // END method

	// If a bullet exits the boundary, disable it
	public void OnBoundaryExit ()
	{
        Activate(false);
	}

    public void Activate (bool active)
    {
        isActive = active;
        Present(active);

        if (!active)
        {
            effects.Activate(false);
        }
    }

    // Enable/disable the visual and physical presence of the bullet
    public void Present (bool isPresent)
    {
        col.enabled = isPresent;
        mainSprite.enabled = isPresent;

        if (!isPresent)
        {
            mover.Stop();
        }
    }

    public void Disable ()
    {
        Activate(false);
    }
}

public enum BulletType
{
	Damaging,
	Charging
}