using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * INTERFACE IActivateable
 * -----------------------
 * Classes that implement the IActivateable interface have unique rules
 * that define what it means for it to be active or inactive
 * -----------------------
 */ 

public interface IActivateable
{
	bool IsActive { get; }
	void Activate (bool active);
}
