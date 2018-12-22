using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * CLASS FormationObject : MonoBehaviour
 * -------------------------------------
 * Descirbes an object that can be placed in a formation
 * by a formation management script
 * -------------------------------------
 */ 

public class FormationObject : MonoBehaviour, IBoundaryExitHandler 
{
	[SerializeField]
	private Mover2D mover;
	// True if the object's motion is being overridden by a script other than the formation management script
	private bool formationOverridden = false;

	public Mover2D Mover { get { return mover; } }
	public bool FormationOverridden { get { return formationOverridden; } }

	// Move to point, only if the formation isn't overridden
	public void TryMoveToPoint (Vector2 point, float time)
	{
		if (!formationOverridden) {
			mover.MoveToPoint (point, time);
		}
	}

	// Put contorl of this script to the corresponding formation manager or to some other script
	public void Override (bool toOverride)
	{
		formationOverridden = toOverride;
	}

	// If a formation object exits the boundary, return control to the formation manager
	public void OnBoundaryExit ()
	{
		formationOverridden = false;
	}
}
