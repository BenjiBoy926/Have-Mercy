using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * CLASS PowerUpSpawner : MonoBehaviour
 * ------------------------------------
 * Randomly spawns power ups from the top of the screen floating down
 * ------------------------------------
 */ 

public class PowerUpSpawner : MonoBehaviour 
{
	[SerializeField]
	private Spawner spawner;	// Data that controls how power ups are spawned
    [SerializeField]
    private bool launchAtFirst; // True if a power up is spawned at the beginning of the level
	[SerializeField]
	private float warningTime;	// Time for which player is warned that a power up is incoming
    [SerializeField]
    private float afterEffectTime;  // Time after spawning the power up that the light effect lingers before deactivating
    [SerializeField]
    private Animator lightFromAbove;    // Animator that creates the light from above effect
    [SerializeField]
    private AudioSource audioSource;    // Plays the audio for the power up spawner
    [SerializeField]
    private AudioClip powerUpAppear;    // Clip is played when the power up appears

	private void Start ()
	{
		spawner.InitializePools ();
		StartCoroutine ("SpawnRoutine");
	}

	private IEnumerator SpawnRoutine ()
	{
		Vector2 origin;	// Origin selected for the object to launch
		float originInterpolater;	// Constant used to interpolate between origin coordinates for the spawner
		List<Launchable> chosenPool;	// Chosen pool from a list of launcher lists from which an object will be spawned
		int poolIndex;	// Random index in the list within a list of launchables
		float waitTime;
        WaitForSeconds warning = new WaitForSeconds(warningTime);
        WaitForSeconds afterEffect = new WaitForSeconds(afterEffectTime);

        // If launch at first is true, launch a power up immediately before entering the infinit spawning loop
        if (launchAtFirst)
        {
            // Find a point linearly between two points at which to spawn the next object
            originInterpolater = Random.Range(0f, 1f);
            origin = Vector2.Lerp(spawner.LocalOriginA, spawner.LocalOriginB, originInterpolater);

            // Cause the light from above to appear at the place where the power up will be spawned
            lightFromAbove.transform.localPosition = origin;
            lightFromAbove.SetTrigger("appear");

            // Plays sound effect
            SoundPlayer.PlaySoundEffect(powerUpAppear, audioSource);

            yield return warning;

            // Randomly select one of the pools of launchables from which to spawn the next object
            poolIndex = Random.Range(0, spawner.Pools.Count);
            chosenPool = spawner.Pools[poolIndex].Launchables;

            // Launch an object from the selected pool
            spawner.LaunchFromPool(chosenPool, origin);

            // Plays sound effect
            SoundPlayer.PlaySoundEffect(powerUpAppear, audioSource);

            // Make the light effect disappear after a short time
            yield return afterEffect;
            lightFromAbove.SetTrigger("disappear");
        }

		while (!(GameManager.Instance.GameOver)) {
			// Randomly select a wait time and wait for that amount of time
			waitTime = Random.Range (spawner.FireRate.min, spawner.FireRate.max);
			yield return new WaitForSeconds (waitTime);

			// Find a point linearly between two points at which to spawn the next object
			originInterpolater = Random.Range (0f, 1f);
			origin = Vector2.Lerp (spawner.LocalOriginA, spawner.LocalOriginB, originInterpolater);

            // Cause the light from above to appear at the place where the power up will be spawned
            lightFromAbove.transform.localPosition = origin;
            lightFromAbove.SetTrigger("appear");

            // Plays sound effect
            SoundPlayer.PlaySoundEffect(powerUpAppear, audioSource);

			yield return warning;

			// Randomly select one of the pools of launchables from which to spawn the next object
			poolIndex = Random.Range (0, spawner.Pools.Count);
			chosenPool = spawner.Pools [poolIndex].Launchables;

            // Launch an object from the selected pool
			spawner.LaunchFromPool (chosenPool, origin);

            // Plays sound effect
            SoundPlayer.PlaySoundEffect(powerUpAppear, audioSource);

            // Make the light effect disappear after a short time
            yield return afterEffect;
            lightFromAbove.SetTrigger("disappear");
		} // END while
	} // END coroutine
}
