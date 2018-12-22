using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * CLASS GunData
 * -------------
 * Organizes a set of data about a particular bullet type
 * -------------
 */ 

[System.Serializable]
public class PlayerGun 
{
	[SerializeField]
	private KeyCode button;	// Button pressed to launch bullets from this gun
	private float timer;	// Used to check if this gun is ready to fire again
	[SerializeField]
	private GameObject bulletPrefab;	// Several copies of this are instantiated for the gun's pool of bullets
	private List<Bullet> bullets;	// List of bullet scripts on each bullet prefab instantiated
	// A list of bullets cast as launchables
	private List<Launchable> bulletsAsLaunchable = new List<Launchable> ();
	private float fireRate;
	[SerializeField]
	private float baseFireRate;	// Base fire rate of the gun when not powered up
	[SerializeField]
	private float fireRateMultiplier;	// Fire rate of the gun is multiplied by this number when powered up
	[SerializeField]
	private float bulletSpeed;
	[SerializeField]
	private int baseStrength;	// Base strength for bullets not powered up
	[SerializeField]
	private int strengthMultiplier;	// Strength of the bullets is multiplied by this amount when powered up

	public KeyCode Button { get { return button; } }
	public List<Launchable> BulletsAsLaunchable { get { return bulletsAsLaunchable; } }
	public float FireRate { get { return fireRate; } }
	public float BulletSpeed { get { return bulletSpeed; } }

	// True if the local button for this gun is being pressed
	public bool buttonPressed {
		get {
			return Input.GetKey (button);
		} // END get
	} // END property

	// Returns true if the gun is ready to fire
	public bool fireReady {
		get {
			return timer < Time.time;
		} // END get
	} // END property

	// Initialize the list of launchables
	public void InitializePool (int bulletsInPool, string poolName)
	{
		List<GameObject> bulletPool;

		// Use the pooler to instantiate a pool of bullet prefabs
		// Get Bullet components from the pool instantiated
		bulletPool = ObjectPoolHandler.InstantiatePool (bulletPrefab, bulletsInPool, poolName);
		ObjectPoolHandler.EnablePool (bulletPool, false);
		bullets = ObjectPoolHandler.GetComponentsInPool<Bullet> (bulletPool);

		// Create list of launchables by simply up-casting
		// each bullet script in the list
		foreach (Bullet bul in bullets) {
			bulletsAsLaunchable.Add ((Launchable)bul);
		} // END foreach
	} // END method

	// Launch a bullet from the launcher specified
	public void LaunchBullet (Launcher launcher)
	{
		launcher.LaunchProjectileFromPool (
			bulletsAsLaunchable,
			launcher.DefaultLocalOrigin + (Vector2)launcher.transform.position,
			launcher.DefaultDirection,
			bulletSpeed);
		timer = fireRate + Time.time;
	} // END method

	// Assign powers of the bullets, powered up or not
	public void AssignPower (bool poweredUp)
	{
		int multiplier;

		// Set multiplier based on whether bullets are powered up
		if (!poweredUp) {
			multiplier = 1;
		} else {
			multiplier = strengthMultiplier;
		}

		// Set the powers on each of the bullets times local multiplier
		foreach (Bullet bul in bullets) {
			bul.Initialize (baseStrength * multiplier);
		} // END foreach
	} // END method

	// Assign the curren
	public void AssignFireRate (bool poweredUp)
	{
		float multiplier;

		// If the fire rate has not been powered up, make the multiplier one
		if (!poweredUp) {
			multiplier = 1f;
		}
		// Otherwise, make it the powered up multiplier
		else {
			multiplier = fireRateMultiplier;
		} // END if-else

		fireRate = baseFireRate * multiplier;
	} // END method
}
