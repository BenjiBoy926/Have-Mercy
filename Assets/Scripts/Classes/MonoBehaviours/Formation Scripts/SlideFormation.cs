using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * CLASS SlideFormation : MonoBehaviour
 * ------------------------------------
 * Given a list of formation objects, the slide formation script
 * repeately slides them between a series of points, then moves
 * them back the other way
 * ------------------------------------
 */ 

public class SlideFormation : MonoBehaviour 
{
	[SerializeField]
	private Vector2 origin;
	[SerializeField]
	private float timeBetweenPoints;	// Time it takes for the objects to slide between the specified points
	private int currentPoint = 0;	// Current point in the list that the objects are being sent to
	[SerializeField]
	private List<FormationData> formationData;
	[SerializeField]
	private List<FormationPoint> points;	// Points between which the formation objects will be sliding

	private void Start ()
	{
        timeBetweenPoints = Mathf.Clamp(timeBetweenPoints, 1f, Mathf.Infinity);
		FormationData.InitializeFormationData (ref formationData);
		DefaultFormationPositions ();

		// Only do the formation if there are at least two points
		if (points.Count > 1) {
			StartCoroutine ("MoveInFormation");
		} else {
			Debug.LogWarning ("From " + gameObject.name + ": it is impossible to create a slide formation with only one point.  The formation will be ignored");
		} // END if-else
	} // END method

	//public void InitializePool ()	// Instantiate each object in the formation data in turn, and grab their formation object scripts

	// Move formation objects to default positions, if they aren't overriden by something else
	private void DefaultFormationPositions ()
	{
		foreach (FormationData data in formationData) {
			if (!(data.overridden)) {
				data.SetToPosition (origin);
			} // END if
		} // END foreach
	} // END method

	private IEnumerator MoveInFormation ()
	{
		// True if the formation objects are currently moving from the first formation point in the list
		// to the last, and false if they are currently moving from the last to the first
		bool forwardThroughList = true;

		while (!(GameManager.Instance.GameOver)) {
			// Try to move each formation object to the current formation point
			foreach (FormationData data in formationData) {
				data.TryMoveToPosition (
					points [currentPoint].LocalPoint, 
					origin, 
					timeBetweenPoints);
			} // END foreach

			// Wait for the motion to finish, and any additional time specified
			yield return new WaitForSeconds (timeBetweenPoints + points [currentPoint].WaitTime);

			// If we're proceeding forward through the list...
			if (forwardThroughList) {
				//...increase current point
				currentPoint++;
			}
			// Otherwise, if we're going backwards...
			else {
				//...decrease it
				currentPoint--;
			}

			// If current point has exceeded total points...
			if (currentPoint >= points.Count) {
				//...start at the second to last point and start going backwards
				currentPoint = points.Count - 2;
				forwardThroughList = false;
			}

			// If current point has gone below or to zero...
			if (currentPoint < 0) {
				//...start at the second point and start going forwards
				currentPoint = 1;
				forwardThroughList = true;
			} // END if
		} // END while
	} // END coroutine
}