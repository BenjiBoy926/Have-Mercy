using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * INTERFACE IKillable
 * -------------------
 * Objects implementing the IKillable interace can be killed,
 * some with the intention of resurrecting later.  If you want
 * to wipe out an object permanently, you should destroy it
 * rather than killing it
 * -------------------
 */ 

public interface IKillable
{
	void Kill ();
}
