using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * INTERFACE IBoundaryExitHandler
 * ------------------------------
 * Classes implementing this interface have special rules
 * about what should happen to them when the exit the boundary
 * ------------------------------
 */ 

public interface IBoundaryExitHandler
{
	void OnBoundaryExit ();
}
