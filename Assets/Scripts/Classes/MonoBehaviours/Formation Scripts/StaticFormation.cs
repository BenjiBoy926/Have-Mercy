using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * CLASS StaticFormation : MonoBehaviour
 * -------------------------------------
 * Holds a group of formation objects in a pre-defined position
 * If any override their formation scripts, this script
 * will bring them back to their preset position after a short time
 * -------------------------------------
 */ 

public class StaticFormation : MonoBehaviour 
{
	[SerializeField]
	private Vector2 origin;	// Position of the formation objects are measured from here, not true the origin
	[SerializeField]
	private float checkTime;	// Time between each check the script performs to see if objects are in their correct place
	[SerializeField]
	private List<FormationData> formationData;	// A list of pairs of formation objects and their local positions

	private void Start ()
	{
		FormationData.InitializeFormationData (ref formationData);
		DefaultFormationPositions ();
		StartCoroutine ("CheckPositions");
	} // END method

	// Move formation objects to default positions, if they aren't overriden by something else
	private void DefaultFormationPositions ()
	{
		foreach (FormationData data in formationData) {
			if (!(data.overridden)) {
				data.SetToPosition (origin);
			} // END if
		} // END foreach
	} // END method

	// Default formation positions as often as specified
	private IEnumerator CheckPositions ()
	{
        WaitForSeconds check = new WaitForSeconds(checkTime);

		// Loop as long as the game hasn't ended to keep the
		// objects at their specified positions
		while (!(GameManager.Instance.GameOver)) {
			DefaultFormationPositions ();
			yield return check;
		} // END while
	} // END coroutine
}