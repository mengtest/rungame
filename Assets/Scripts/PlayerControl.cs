using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
	public bool facingRight = true;			// For determining which way the player is currently facing.
	public bool jump = false;				// Condition for whether the player should jump.

	public float moveForce = 10f;			// Amount of force added to move the player left and right.
	public float maxSpeed = 5f;				// The fastest the player can travel in the x axis.
	public float jumpForce = 5000f;			// Amount of force added when the player jumps.

	private Transform groundCheck;			// A position marking where to check if the player is grounded.
	private bool grounded = false;			// Whether or not the player is grounded.
	//private bool groundedNewPlatform = false;	// Whether or not the player is touching a platform they have not touched before
	private Score score;				// Reference to the Score script.
	private float newPosition = 0;
	public float characterXRadius = 0;
	public float characterYRadius = 0;
	public Vector2 platformStrokeCoords = new Vector2(0,0);
	//public string cat = UnityEngine.StackTraceUtility.ExtractStackTrace();


	void Awake()
	{
		// Setting up references.
		groundCheck = transform.Find("groundCheck");
		score = GameObject.Find("Score").GetComponent<Score>();
		characterXRadius = GameObject.Find("stickfigure").GetComponent<SpriteRenderer>().bounds.extents.x;
		characterYRadius = GameObject.Find("stickfigure").GetComponent<SpriteRenderer>().bounds.extents.y;
	}


	void Update()
	{
		verticalMovement(); // Detect if user jumped, jump if so, and adjust collisions
		horizontalMovement(); // Detect if left or right input was given and flip character and velocity if appropriate.
		wrapAround(); // Detect if character has reached left or right side of scene and teleport to opposite side if appropriate.
		updateScore(); // Detect if character is touching ground (grounded), increase score if appropriate.
		boundsCheck(); // Detect if character is out of bounds and reset level if appropriate. (the character shouldn't ever hit this, it should only be called when something goes horribly wrong)
		updatePlatforms(); // Detect if collision with top of platform has occurred, create new stroke sprite is appropriate.
	}

	void updatePlatforms()
	{
		if(grounded)
			calculatePlatformStrokeCoords();
			drawPlatformStroke(platformStrokeCoords);
	}

	void calculatePlatformStrokeCoords()
	{
		float offset = -0.2f;
		float platformStrokeCoordX = transform.position.x + characterXRadius;
		float platformStrokeCoordY = transform.position.y - characterYRadius - offset;
		platformStrokeCoords = new Vector2(platformStrokeCoordX, platformStrokeCoordY);
	}

	void drawPlatformStroke(Vector2 platformStrokeCoords)
	{
		GameObject platformStroke = Instantiate(Resources.Load("platformStroke")) as GameObject;		// A platformStroke object;
		platformStroke.GetComponent<SpriteRenderer>().sortingLayerName="platformStrokes";
		Instantiate(platformStroke, platformStrokeCoords, transform.rotation);
	}

	void FixedUpdate ()
	{
		// The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
		grounded = Physics2D.Linecast (transform.position, groundCheck.position, 1 << LayerMask.NameToLayer ("Ground")); 
	}

	void boundsCheck()
	{
		if (GetComponent<Rigidbody2D> ().transform.position.y < -60)
			ReloadGame();
	}

	void wrapAround()
	{
		if ((GetComponent<Rigidbody2D> ().transform.position.x <= -39) || (GetComponent<Rigidbody2D> ().transform.position.x >= 40))
			Teleport();
	}

	void verticalMovement()
	{

		if (GetComponent<Rigidbody2D>().velocity.y <= 0 )
			GetComponent<BoxCollider2D>().enabled = true;
		else
			GetComponent<BoxCollider2D>().enabled = false;
		// If the jump button is pressed and the player is grounded then the player should jump.
		if (Input.GetButtonDown("Jump") && grounded)
			jump = true;

		// If the player should jump...
		if(jump)
		{
			// Add a vertical force to the player.
			GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);

			// Temporarily remove collision detection
			GetComponent<BoxCollider2D>().enabled = false;

			// Make sure the player can't jump again until the jump conditions from Update are satisfied.
			jump = false;
		}

	}
	void horizontalMovement()
	{
		transform.Translate(moveForce, 0,0);
		// If the player is changing direction (h has a different sign to velocity.x)
		float h = Input.GetAxis("Horizontal");
		// If the input is right and character is currently left, or vice versa...
		if((h > 0 && !facingRight) ||( h < 0 && facingRight))
			// ... flip the player and their direction.
			Flip();
	}
	void Flip ()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;
		moveForce = -moveForce;
		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	void updateScore()
	{
		// If the player is grounded, then increase score.
		if(grounded)
			score.score += 100;
	}
	

	void  Teleport ()
	{
		if (facingRight)
			newPosition = -38;
		else
			newPosition = 40;
		GetComponent<Rigidbody2D>().transform.position = new Vector3(newPosition, transform.position.y, transform.position.z);
	}


	void ReloadGame()
	{			
		// Reload the level.
		//Debug.Log("Cat cat cat");

		//Application.LoadLevel(Application.loadedLevel);
		SceneManager.LoadScene("Level", LoadSceneMode.Single);
	}
}
