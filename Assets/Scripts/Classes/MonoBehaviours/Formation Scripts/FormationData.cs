using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * CLASS FormationData
 * -------------------
 * Pairs a formation object with a local position
 * in a two-dimensional plane
 * -------------------
 */ 
[System.Serializable]
public class FormationData 
{
	[SerializeField]
	private GameObject prefab;	// Game object with the formation script that will be moved in the formation
	private FormationObject formation;	// Script of the object in the formation
	[SerializeField]
	private Vector2 localPos;	// Position of this object relative to some origin

	public GameObject Prefab { get { return prefab; } }

	// True if the formation associated with this data
	// is overridden and false otherwise
	public bool overridden {
		get {
			return formation.FormationOverridden;
		} // END get
	} // END property

	// Assign the formation script for this data
	public void AssignFormationScript (FormationObject script)
	{
		formation = script;
	}

	// Set the object to its local position, specifying the origin
	public void SetToPosition (Vector2 origin = default(Vector2))
	{
		formation.transform.localPosition = new Vector3 (
			origin.x + localPos.x,
			origin.y + localPos.y,
			formation.transform.localPosition.z);
	} // END method

	// If the formation object isn't overridden, move to the position specified 
	// relative to the specified origin for the given time 
	public void TryMoveToPosition (Vector2 point, Vector2 origin, float time)
	{
		formation.TryMoveToPoint (point + origin + localPos, time);
	} // END method

	// Static method takes a list of formation data as a refrence parameter,
	// instantiates the objects specified, and assigns their scripts back into each class in the list
	public static void InitializeFormationData (ref List<FormationData> data)
	{
		List<GameObject> prefabs = new List<GameObject> ();	// List of prefabs to be instantiated
		List<GameObject> instances;	// List of prefab clones instantiated
		List<FormationObject> formationScripts;	// Formation object scripts on instances

		// Fill a list of game objects using the prefabs in the formation data
		foreach (FormationData bit in data) {
			prefabs.Add (bit.Prefab);
		} // END foreach

		// Instantiate the prefabs and get their formation object scripts
		instances = ObjectPoolHandler.InstantiatePool (prefabs, "Slide Formation Pool");
		ObjectPoolHandler.EnablePool (instances, true);
		formationScripts = ObjectPoolHandler.GetComponentsInPool <FormationObject> (instances);

		// Assign the formation scripts on each instance in the list of formation data
		for (int index = 0; index < data.Count; index++) {
			data [index].AssignFormationScript (formationScripts [index]);
		} // END for
	}
}