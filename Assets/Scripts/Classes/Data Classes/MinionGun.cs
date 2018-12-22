using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * CLASS MinionGun
 * ---------------
 * Organizes data about a minion's gun, such as its min-max fire rate
 * and the time for which the player is warned about when the gun
 * is about to fire
 * ---------------
 */ 

[System.Serializable]
public class MinionGun 
{
	[SerializeField]
	private Launcher gun;	// Launcher used to launch the bullets
	[SerializeField]
	private IntConstraint baseFireRate;	// Min and max time between shots
	[SerializeField]
	private float warningTime;	// Time for which minion uses an effect to warn the player they're about to shoot
	[SerializeField]
	private GameObject bulletPrefab;	// The "pooler" script instantiates several copies of these for the gun to launch
	[SerializeField]
	private int bulletsInPool;	// Number of bullets in the object pool for this gun

	public Launcher Gun { get { return gun; } }
	public IntConstraint BaseFireRate { get { return baseFireRate; } }
	public float WarningTime { get { return warningTime; } }

	public void InitializePool ()
	{
		List<GameObject> pool;
		List<Launchable> scriptsInPool;	// Launchable scripts attached to each game object in the pool

		// Create a pool of instances of the bullet prefab specified,
		// get their launchable scripts and assign them to the object pool
		// on the launcher script
		pool = ObjectPoolHandler.InstantiatePool (bulletPrefab, bulletsInPool);
		ObjectPoolHandler.EnablePool (pool, false);
		scriptsInPool = ObjectPoolHandler.GetComponentsInPool<Launchable> (pool);
		gun.AssignObjectPool (scriptsInPool);
	} // END method

	// Launch a bullet using the presets on launcher
	public void Launch ()
	{
		gun.LaunchProjectileFromPool (
			gun.DefaultObjectPool,
			gun.DefaultLocalOrigin + (Vector2)gun.transform.localPosition, 
			gun.DefaultDirection, 
			gun.DefaultSpeed);
	}

	// Launch a bullet with a specific speed
	public void Launch (float newSpeed)
	{
		gun.LaunchProjectileFromPool (
			gun.DefaultObjectPool,
			gun.DefaultLocalOrigin + (Vector2)gun.transform.localPosition, 
			gun.DefaultDirection, 
			newSpeed);
	}

	// Launch a bullet from the launcher, overwriting the default speed and direction
	public void Launch (Vector2 newDirection, float newSpeed)
	{
		gun.LaunchProjectileFromPool (
			gun.DefaultObjectPool,
			gun.DefaultLocalOrigin + (Vector2)gun.transform.localPosition, 
			newDirection, 
			newSpeed);
	}
}