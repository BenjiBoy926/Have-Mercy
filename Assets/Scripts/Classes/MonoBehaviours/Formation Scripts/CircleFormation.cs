using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * CLASS CircleFormation : MonoBehaviour
 * -------------------------------------
 * Enables a list of formation objects to be coordinated into
 * a roughly circular movement.  The movement is not truly circular,
 * but instead involves the movement of the objects along the edges
 * of a regular n-gon where n = formationObjects.Count * accuracyCoef
 * -------------------------------------
 */ 

public class CircleFormation : MonoBehaviour 
{
	[SerializeField]
	private List<GameObject> prefabs;	// Prefabs to be instantiated into the formation's object pool
	private List<FormationObject> formationObjects;	// Reference to formation object scripts on ships in formation
	[SerializeField]
	private Vector2 center;
	[SerializeField]
	private float radius;
	[SerializeField]
	private CircleDir direction;	// Direction of motion clockwise or counter-clockwise
	[SerializeField]
	private float period;	// Time it takes for the circle to make a complet cycle
	[SerializeField]
	private int accuracyCoef;	// This times the number of formation objects is the number of points into which the circle is subdivided
	// The two-dimensional points relative to the true origin that the formation objects move between
	private List<Vector2> points = new List<Vector2> ();

	private void Start ()
	{
		List<GameObject> instances;	// Instances of the prefabs instantiated

		// Initialize the object pool of objects in the formation
		instances = ObjectPoolHandler.InstantiatePool (prefabs, "Circle Formation Pool");
		ObjectPoolHandler.EnablePool (instances, true);
		formationObjects = ObjectPoolHandler.GetComponentsInPool<FormationObject> (instances);

		// If formation objects were specified, start the formation movement coroutine
		if (formationObjects.Count > 0) {
			accuracyCoef = (int)Mathf.Clamp ((float)accuracyCoef, 1f, 1000f);
			points = MyMath.VerticesOfInscribedNGon (formationObjects.Count * accuracyCoef, radius, center, dir: direction);
			InitializeObjectPositions ();
			StartCoroutine ("MoveInFormation");
		} // END if
	} // END method

	//private void InitializePool ()	// Instantiate each object in the list in turn, and grab their formation object components

	// Loop through each formation object and place them at every "accuracyCoef" 
	// other position in the list of positions
	private void InitializeObjectPositions ()
	{
		for (int index = 0; index < formationObjects.Count; index++) {
			formationObjects [index].transform.localPosition = new Vector3 (
				points [index * accuracyCoef].x,
				points [index * accuracyCoef].y,
				formationObjects [index].transform.localPosition.z);
		} // END for
	} // END method

	private IEnumerator MoveInFormation ()
	{
		// Represents the number of times the formation objects have moved around the circle
		// Resets to 1 after each complete cycle
		int leadingMove = 1;
		float timePerMove = period / (float)points.Count;	// Time it takes for each motion across the polygon to occur 
        WaitForSeconds moveWait = new WaitForSeconds(timePerMove);

		while (!(GameManager.Instance.GameOver)) {
			// Move the units and wait for the motion to complete
			MoveUnits (leadingMove, timePerMove);
			yield return moveWait;

			// Increment moves around the circle
			leadingMove++;

			// If it is more than the total number of points, we know we've 
			// gone full circle, so set the number back to 1
			if (leadingMove > points.Count) {
				leadingMove = 1;
			} // END if
		} // END while
	} // END coroutine

	// Given the number of times the first unit has moved around the circle
	// and the time it takes for the units to move to each point, this method
	// moves each of the objects in the circle
	private void MoveUnits (int currentMove, float time)
	{
		int pointIndex;	// Index in the list of points that each formation object moves to

		// Move any unit that isn't overridden
		for (int index = 0; index < formationObjects.Count; index++) {
			pointIndex = (currentMove + (index * accuracyCoef)) % points.Count;
			formationObjects [index].TryMoveToPoint (points [pointIndex], time);
		} // END for
	} // END method
}

public enum CircleDir
{
	Clockwise,
	CounterClockwise
}