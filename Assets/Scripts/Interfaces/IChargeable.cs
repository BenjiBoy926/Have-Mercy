using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * INTERFACE IChargeable
 * ---------------------
 * Objects implementing the IChargeable interface can
 * be charged up by charging bullets
 * ---------------------
 */ 

public interface IChargeable 
{
	void ChargeUp (int chargeGiven);
}
