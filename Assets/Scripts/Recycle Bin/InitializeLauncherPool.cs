using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * CLASS InitializeLauncherPool : MonoBehaviour
 * --------------------------------------------
 * Initialize the object pool of a launcher by finding the parent of the
 * objects and getting the components in its children
 * --------------------------------------------
 */ 

public class InitializeLauncherPool : MonoBehaviour 
{
	[SerializeField]
	private Launcher launcher;
	[SerializeField]
	private string poolParentName;	// The name of the object that is the parent of the objects to be used in the Launcher's object pool

	private void Start ()
	{
		GameObject poolParent = GameObject.Find (poolParentName);

		if (poolParent != null) {
			// Get launchable components in the children of the parent object found
			Launchable[] poolArray = poolParent.GetComponentsInChildren <Launchable> ();
			List<Launchable> pool = new List<Launchable> ();

			// Check if we found any objects in the array of objects
			if (poolArray.Length > 0) {
				// Add each element of the array to the list
				foreach (Launchable launchable in poolArray) {
					pool.Add (launchable);
				}
				// Assign the list to the launcher's object pool
				launcher.AssignObjectPool (pool);
			} else {
				Debug.LogWarning ("No launchable scripts were found in the children of " + poolParent.name);
			} // END inner if-else
		} else {
			Debug.LogWarning ("Initializer on " + gameObject.name + " couldn't find any object named " + poolParentName);
		} // END outer if-else
	} // END method
} // END class
