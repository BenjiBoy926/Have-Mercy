using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * CLASS GameManager : MonoBehaviour
 * ---------------------------------
 * Manages major activity in each scene
 * ---------------------------------
 */ 

public class GameManager : MonoBehaviour 
{
	private static GameManager instance;	// Instance of the only game manager active in the scene
	[SerializeField]
	private int currentLevel;
    [SerializeField]
    private int loreIndex;  // If this level is followed by lore, this variables contains the index of the lore
	private bool gameOver = false;	// True if the game has ended
    private Player player;  // Reference to the player in the scene
    private PowerUpSpawner powerUpSpawner;  // Reference to the power up spawner in the scene
    private string tutorialObjTag = "Tutorial";  // Tag of the tutorial objects in the scene
    private GameObject tutorialObject;  // Object, if any, that display how to play info
	private List<Minion> enemies = new List<Minion> ();	// List of enemies in the scene
	private List<GreenUnit> units = new List<GreenUnit> ();	// List of green units in the scene
	[SerializeField]
	private float respawnTime;	// Time it takes for enemies in the stage to respawn
	private float minionDestroyDelay = 1f;  // Delay in seconds after wiping a minion that the minion object is completely destroyed
    [SerializeField]
    private List<float> loadTimes;  // Times for how long to wait before loading next level or reloading this level.  Parallel to "EndType" enum
    [SerializeField]
    private string faderTag;    // Tag of the object that fades the scene
    private ColorMod faderScript;   // Script that creates a fade-in effect for the scene 

	public static GameManager Instance { get { return instance; } }
	public bool GameOver { get { return gameOver; } }
	public float RespawnTime { get { return respawnTime; } }

	private void Awake ()
	{
		instance = this;
        player = FindObjectOfType<Player>();
        powerUpSpawner = FindObjectOfType<PowerUpSpawner>();
        tutorialObject = GameObject.FindGameObjectWithTag(tutorialObjTag);
        faderScript = GameObject.FindGameObjectWithTag(faderTag).GetComponent<ColorMod>();
        faderScript.Transition(false);
	}

	// Ends the game.  Performs a different function depending on 
    // what caused the game to end
	public void EndGame (EndType ending)
	{
		switch (ending)
        {
            case EndType.GreenUnitSaved:
                WipeEnemies(MinionWipeType.PermanentKill);
                player.OnWin();
                DataManager.CompleteLevel(currentLevel);

                // If lore is specified to load next and it hasn't already been viewed, load the lore scene
                if (loreIndex > 0 && !DataManager.Data.LoreViewed[loreIndex - 1])
                {
                    StartCoroutine(LoadSceneAfterTime("Lore" + loreIndex, loadTimes[(int)ending]));
                }
                // Otherwise, load the next level
                else
                {
                    StartCoroutine(LoadSceneAfterTime("Level" + (currentLevel + 1), loadTimes[(int)ending]));   
                }

                break;
            case EndType.GreenUnitKilled:
                // Destroy all enemies and the player
                WipeEnemies(MinionWipeType.Destroy);
                Destroy(player.gameObject);

                // Destroy any living green units
                foreach (GreenUnit greenUnit in units)
                {
                    if (!greenUnit.isDead)
                    {
                        Destroy(greenUnit.gameObject);
                    }
                }

                StartCoroutine(LoadSceneAfterTime(SceneManager.GetActiveScene().name, loadTimes[(int)ending]));
                break;
            case EndType.PlayerKilled:
                StartCoroutine(LoadSceneAfterTime(SceneManager.GetActiveScene().name, loadTimes[(int)ending]));
                break;
        }

        // Stop the power up spawner's spawn routine
        if (powerUpSpawner != null)
        {
            powerUpSpawner.StopAllCoroutines();
        }
        if (tutorialObject != null)
        {
            tutorialObject.SetActive(false);
        }
		gameOver = true;
	} // END method

	// Wipe all enemies from the battlefield in the way specified,
    // permanent kill, temporary kill, or simply by destroying immediately
	public void WipeEnemies (MinionWipeType wipeType)
	{
        switch (wipeType)
        {
            case MinionWipeType.TempKill:
                // Kill all active enemies
                foreach (Minion enemy in enemies)
                {
                    if (enemy.IsActive)
                    {
                        enemy.Kill();
                    }
                }
                break;
            case MinionWipeType.PermanentKill:
                // Kill all active enemies, and destroy any inactive ones
                foreach (Minion enemy in enemies)
                {
                    if (enemy.IsActive)
                    {
                        enemy.Kill();
                        enemy.StopAllCoroutines();
                        Destroy(enemy.gameObject, minionDestroyDelay);
                    }
                    else
                    {
                        Destroy(enemy.gameObject);
                    }
                }
                break;
            case MinionWipeType.Destroy:
                // Destroy all minions
                foreach (Minion enemy in enemies)
                {
                    Destroy(enemy.gameObject);
                }
                break;
        }
	} // END method

	// Load the specified scene after locally specified time
	private IEnumerator LoadSceneAfterTime (string sceneName, float time)
	{
		yield return new WaitForSeconds (time);
		SceneManager.LoadScene (sceneName);
	}

	// Add a minion script to the list
	public void Subscribe (Minion enteringMinion)
	{
		enemies.Add (enteringMinion);
	}

	// Add a green unit to the list of green units
	public void Subscribe (GreenUnit enteringUnit)
	{
		units.Add (enteringUnit);
	}

	// Check the current states of the green units
	// If all of them are fully charged, end the game - the player won
	// Return true if they are all fully charged and false otherwise
	public bool CheckGreenUnitCharge ()
	{
		bool allFullCharged;	// True if every green unit is fully charged
		allFullCharged = units.TrueForAll (x => x.fullyCharged);

		// If all green units are fully charged...
		if (allFullCharged) {
			//...end the game - the player won
			EndGame (EndType.GreenUnitSaved);
		} // END if

		return allFullCharged;
	} // END method
}

public enum EndType
{
    GreenUnitSaved,
    GreenUnitKilled,
    PlayerKilled
}

public enum MinionWipeType
{
    TempKill,
    PermanentKill,
    Destroy
}