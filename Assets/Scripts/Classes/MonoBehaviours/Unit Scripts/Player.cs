using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * CLASS Player : MonoBehaviour
 * ----------------------------
 * Enables player input to take control of a mover and a
 * launcher to move the player object around and shoot bullets
 * ----------------------------
 */ 

public class Player : MonoBehaviour, IDamageable, IKillable 
{
	[SerializeField]
	private Mover2D mover;	// Script that moves the player
	[SerializeField]
	private Launcher gun;	// Script that shoots bullets for the player
    [SerializeField]
    private Collider2D col; // Collider of the player object
	[SerializeField]
	private int maxHealth;	// Max health and starting health of the player
	private int health;	// Current health of the player
	private bool invincible;	// True if the player is presently invulnerable
	[SerializeField]
	private float speed;
	[SerializeField]
	private float powerUpTime;	// Time for which the player is powered up with invincibility, stronger bullets, etc.
	[SerializeField]
	private FloatConstraint xLimits;	// Horizontal limits of player's movement
	[SerializeField]
	private FloatConstraint yLimits;    // Vertical limits of player's movement
    private FloatConstraint xDirLimits = new FloatConstraint(-1f, 1f);  // Used to constrain horizontal velocity
    private FloatConstraint yDirLimits = new FloatConstraint(-1f, 1f);  // Used to constrain vertical velocity
    [SerializeField]
	private int bulletsInPool;
	[SerializeField]
	private PlayerGun redGun;	// Data for the player's red bullets
	[SerializeField]
	private PlayerGun greenGun;	// Data for the player's green bullets
    [SerializeField]
    private PlayerEffects effects;  // Manages visual/audio effects for the player

    [SerializeField]
    private string menuNavTag;  // Tag of the object that has the main menu navigation script on it
    private ToMainMenu mainMenuNav; // Script that pauses the game and brings up a panel to see if the player wants to return to the main menu
    [SerializeField]
    private KeyCode menuButton; // Button that pauses the game to navigate to the main menu

	private void Start ()
	{
		health = maxHealth;
		redGun.InitializePool (bulletsInPool, "Player Red Bullet Pool");
		greenGun.InitializePool (bulletsInPool, "Player Green Bullet Pool");
		Default ();

        // Grab a reference to the object that enables the player to navigate back to the main menu
        mainMenuNav = GameObject.FindGameObjectWithTag(menuNavTag).GetComponent<ToMainMenu>();
	}

	private void Update ()
	{
        // If the "p" key is pressed, use script to bring up main menu confirmation panel
        if (Input.GetKeyDown(menuButton))
        {
            mainMenuNav.RequestConfirmation();
        }

        // The main thing to note about these selection statements is that they prevent one gun
        // from firing if the button of the other gun is also pressed.  This means the player
        // cannot fire both guns at the same time
        if (!(greenGun.buttonPressed) && redGun.buttonPressed && redGun.fireReady) {
			redGun.LaunchBullet (gun);
            effects.RedShot();
		} else if (!(redGun.buttonPressed) && greenGun.buttonPressed && greenGun.fireReady) {
			greenGun.LaunchBullet (gun);
            effects.GreenShot();
		}
	}

	private void FixedUpdate ()
	{
		// Recieve horizontal and vertical input and store the resutls
		float x = Input.GetAxisRaw ("Horizontal");
		float y = Input.GetAxisRaw ("Vertical");

        // Reset the limits back to default
        xDirLimits.min = -1f;
        xDirLimits.max = 1f;
        yDirLimits.min = -1f;
        yDirLimits.max = 1f;

		// Set local directional limits based on player's position relative
		// to global positional limits
		if (transform.localPosition.x < xLimits.min) {
			xDirLimits.min = 0f;
		}
		if (transform.localPosition.x > xLimits.max) {
			xDirLimits.max = 0f;
		}
		if (transform.localPosition.y < yLimits.min) {
			yDirLimits.min = 0f;
		}
		if (transform.localPosition.y > yLimits.max) {
			yDirLimits.max = 0f;
		}

		// Clamp directional input within directional limits
		x = Mathf.Clamp (x, xDirLimits.min, xDirLimits.max);
		y = Mathf.Clamp (y, yDirLimits.min, yDirLimits.max);

		// Move towards input with the default speed
		mover.MoveTowards (new Vector2 (x, y), speed);
	}

	// Deplete health if player is not invulnerable
	public void TakeDamage (int damageTaken)
	{
		if (!invincible) {
			health -= damageTaken;
		
			// If health is depleted, die
			if (health <= 0) {
				Kill ();
			} // END if
		} else {
            effects.ShieldStruck();
		}
	} // END method

	public void Kill ()
	{
		enabled = false;
        col.enabled = false;
		GameManager.Instance.EndGame (EndType.PlayerKilled);
        mover.Stop();
        effects.Die();
	} // END method

	// Powers up the player based on the power up type given
	public void PowerUp (PowerUpType type)
	{
		// Cancel any existing invokes of Default method
		CancelInvoke ();

		switch (type) {
		case PowerUpType.Power:
			redGun.AssignPower (true);
			break;
		case PowerUpType.FireRate:
			redGun.AssignFireRate (true);
			greenGun.AssignFireRate (true);
			break;
		case PowerUpType.Shield:
			invincible = true;
			break;
		case PowerUpType.Bomb:
			GameManager.Instance.WipeEnemies (MinionWipeType.TempKill);
			break;
		} // END switch

		// If player was powered up, schedule to have them powered down after the preset time
		if (type == PowerUpType.Power || type == PowerUpType.FireRate || type == PowerUpType.Shield) {
			Invoke ("Default", powerUpTime);
		}

        // Use effects script to produce an effect
        effects.PowerUp(type, powerUpTime);
	} // END method

	// Restore default power on the player's guns and make them vulnerable
	public void Default ()
	{
		redGun.AssignPower (false);
		redGun.AssignFireRate (false);
		greenGun.AssignPower (false);
		greenGun.AssignFireRate (false);
		invincible = false;
		// AN EFFECT YAY
	} // END method

    // Function disables script and plays an effect
    // when the player wins the stage
    public void OnWin ()
    {
        enabled = false;
        col.enabled = false;
        mover.Stop();
        effects.OnWin();
    }
}