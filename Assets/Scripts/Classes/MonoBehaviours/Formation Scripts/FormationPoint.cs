using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * CLASS FormationObject
 * ---------------------
 * Defines a two-dimensional point in a plane
 * paired with a wait time
 * ---------------------
 */ 
[System.Serializable]
public class FormationPoint 
{
	[SerializeField]
	private Vector2 localPoint;	// Point of the formation relative to the formation's origin
	[SerializeField]
	private float waitTime;	// Time for which formation objects will wait at this point

	public Vector2 LocalPoint { get { return localPoint; } }
	public float WaitTime { get { return waitTime; } }

	public FormationPoint (Vector2 point, float time)
	{
		localPoint = point;
		waitTime = time;
	} // END constructor
}