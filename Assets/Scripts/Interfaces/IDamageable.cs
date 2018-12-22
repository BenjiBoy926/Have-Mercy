using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * INTERFACE IHealthHandler
 * ------------------------
 * Any object implementing this interface can take damage
 * and die with the possibility of resurrecting later
 * ------------------------
 */ 

public interface IDamageable
{
	void TakeDamage (int damageTaken);
}