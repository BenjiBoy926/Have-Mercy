using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * CLASS GunnerEffects : ActivationEffects
 * ---------------------------------------
 * Enables the eye of the gunner to look at the player
 * ---------------------------------------
 */ 

public class GunnerEffects : ActivationEffects
{
    [SerializeField]
    private Gunner gunnerScript;    // Reference to the script managing the gunner
    [SerializeField]
    private Transform eye;  // Eyeball of the gunner that follows the target

    void Update()
    {
        LookAtTarget();
    }

    // Rotates the eye to look at the target specified on the gunner script
	private void LookAtTarget ()
    {
        float zAngle;   // Z-rotation of the eye
        Vector2 toTarget;   // Vector with its tail at this object and the tip at the target

        // Get the angle between the down vector and the vector pointing at the target
        toTarget = (Vector2)(gunnerScript.Target.position - transform.position);
        zAngle = Vector2.Angle(Vector2.down, toTarget);

        // If the target is to the left of this object, make the angle negative
        if (toTarget.x < 0f)
        {
            zAngle *= -1f;
        }

        // Set the z-rotation of the eye
        eye.rotation = Quaternion.Euler(0f, 0f, zAngle);
    }
}
