using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * CLASS Spawner
 * -------------
 * Organizes the information for how a carrier spawns minions
 * Spawners have an object pool within an object pool so they can
 * spawn multiple objects as well as multiple object types
 * -------------
 */ 

[System.Serializable]
public class Spawner 
{
	[SerializeField]
	private Launcher launcher;
	private List<LaunchableList> pools = new List<LaunchableList> ();	// A list of launchable lists
	[SerializeField]
	private List<GameObject> prefabs;	// These prefabs are each instantiated into a different pool
	[SerializeField]
	private int instancesPerPrefab;	// Number of instances of each prefab that will be intantiated
	[SerializeField]
	private IntConstraint fireRate;

	// Origin of the launchable is a linear interpolation
	// between these two vectors
	[SerializeField]
	private Vector2 localOriginA;
	[SerializeField]
	private Vector2 localOriginB;

	public List<LaunchableList> Pools { get { return pools; } }
	public IntConstraint FireRate { get { return fireRate; } }
	public Vector2 LocalOriginA { get { return localOriginA; } }
	public Vector2 LocalOriginB { get { return localOriginB; } }

	// Initialize the pools by initializing a pool of each game object in "prefabs" 
	// and adding a list of their launchables to each list in the "pools" list
	public void InitializePools ()
	{
		List<GameObject> instances;	// List of instances of the prefabs instantiated
		List<Launchable> instanceScripts;	// List of launchable scripts on the instances of the prefabs

		// Loop through each object in the list of prefabs
		// and instantiate a pool of each one
		foreach (GameObject prefab in prefabs) {
            // Instantiate the pool
            instances = ObjectPoolHandler.InstantiatePool(prefab, instancesPerPrefab, "Spawnables");

            // Disable all objects in the spawned pools
            ObjectPoolHandler.EnablePool(instances, false);

			// Grab the launchable scripts on the pool
			instanceScripts = ObjectPoolHandler.GetComponentsInPool<Launchable> (instances);

			// Add a launchable list to the local list of launchable lists
			pools.Add (new LaunchableList (instanceScripts));
		}
	}

	// Launch an object using the default information from the launcher,
	// specifying a new origin
	public void LaunchFromPool (List<Launchable> newPool, Vector2 newOrigin)
	{
		launcher.LaunchProjectileFromPool (
			newPool,
			newOrigin + (Vector2)launcher.transform.position,
			launcher.DefaultDirection,
			launcher.DefaultSpeed);
	} // END method

	// Launch a single, specific launchable
	public void LaunchSingle (Launchable toLaunch, Vector2 newOrigin)
	{
		launcher.LaunchProjectile (toLaunch, newOrigin,
			launcher.DefaultDirection,
			launcher.DefaultSpeed);
	}
} // END class

/*
 * CLASS LaunchableList
 * --------------------
 * A simple class that allows a list of launchables to be serialized
 * Serializing a list of LaunchableList effectively allows you to serialize
 * a list within a list of launchables
 * --------------------
 */ 

[System.Serializable]
public class LaunchableList
{
	[SerializeField]
	private List<Launchable> launchables;
	public List<Launchable> Launchables { get { return launchables; } }

	// Construct an instance of LaunchableList using the given list of launchables
	public LaunchableList (List<Launchable> theLaunchables)
	{
		launchables = theLaunchables;
	}
}