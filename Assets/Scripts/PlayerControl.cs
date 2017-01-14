using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{

	private Rigidbody2D characterRigidbody; // Reference to the rigidbody2d of the character object.
	private StrokeController strokeController; // Reference to the StrokeController script
	private Score score;					// Reference to the Score script.
	private LevelManager levelManager;	// LevelManager is needed to pull information about the level (width, etc)

	public bool facingRight = true;			// For determining which way the player is currently facing.
	public float moveForce = 1500f;			// Amount of force added to move the player left and right.
	public float jumpForce = 5000f;			// Amount of force added when the player jumps.
	private bool grounded = false;			// Whether or not the player is grounded (standing on a platform).


	void Awake()
	{
		// Setting up references to objects.
		score = GameObject.Find("Score").GetComponent<Score>();
		strokeController = GameObject.Find ("StrokeController").GetComponent<StrokeController>();
		characterRigidbody = GetComponent<Rigidbody2D>();
		levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
	}

	void Update(){
		// Detect if player pressed up, jump if so and disable collisions until they land.
		verticalMovement ();
		// Detect if left or right input was given and flip character and velocity if appropriate.
		horizontalMovement(); 
	}

	void FixedUpdate()
	{
		// Detect if character has reached left or right side of scene and teleport to opposite side if appropriate.
		wrapAround(); 
		// Detect if character is out of bounds and reset level if appropriate. 
		//(the character shouldn't ever hit this, it should only be called when something goes horribly wrong)
		boundsCheck(); 
	}

	void onCollisionEnter2D(){
		grounded = true;
	}
	void OnCollisionStay2D(Collision2D collision2D) 
	{
		//If the player collides with a platform stroke
		grounded = true;
		SpriteRenderer platformStroke = collision2D.gameObject.GetComponent( (typeof(SpriteRenderer))) as SpriteRenderer;
		// Change the color of the platform stroke.
		Color blue = new Color (0f, 0f, 1f, 1f);
		if (platformStroke.color != blue) {
			platformStroke.color = blue;
		// And update the score if they haven't touched the platform before.
			updateScore();
		}
	}

	void boundsCheck()
	{
		// If the player character is super far off the screen, reload game.
		if (Mathf.Abs(transform.position.y) > 60 || Mathf.Abs(transform.position.x) > 60) {
			ReloadGame ();
		}
	}

	void wrapAround()
	{
		// If the player hits the left or right edge of the playing screen, teleport them to the other side.
		if ((characterRigidbody.transform.position.x < (levelManager.leftWall )) || (characterRigidbody.transform.position.x > levelManager.rightWall)) {
			Teleport ();
		}
	}

	void verticalMovement()
	{
		bool jumpKey = false;
		// If the jump button is pressed and the player is grounded then the player should jump.
		if (Input.GetButtonDown ("Jump")) {
			jumpKey = true;
		} else {
			jumpKey = false;
		}

		// If the character is not moving upward (velocity.y = 0) AND it is not moving downward (negative velocity.y) 
		// then enable collisions on the character box collider so that it can land on platforms
		if (characterRigidbody.velocity.y > 0) {
			GetComponent<BoxCollider2D> ().enabled = false;
		}
		else{
			GetComponent<BoxCollider2D>().enabled = true;
		}
		bool jump = false;
		if (jumpKey && grounded) {
			jump = true;
		}

		// If the player should jump...
		if(jump)
		{
			// Add a vertical force to the player.
			//characterRigidbody.velocity = (new Vector2(characterRigidbody.velocity.x, 0));
			characterRigidbody.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Force);

			// Temporarily remove collision detection
			GetComponent<BoxCollider2D>().enabled = false;
			grounded = false;

			// Make sure the player can't jump again until the jump conditions from Update are satisfied.
			jump = false;
		}

	}
	void horizontalMovement()
	{
		float horizontalDirection = 0;
		// Sets a variable to indicate which direction the user is facing.
		if (Input.GetAxis ("Horizontal") > 0) {
			horizontalDirection = 1;
		} else if (Input.GetAxis ("Horizontal") < 0) {
			horizontalDirection = -1;
		}
		// If the character's velocity does not match the moveForce, update it. (this keeps the player character at the
		// correct speed).
		if (characterRigidbody.velocity.x != moveForce) {
			characterRigidbody.velocity = (new Vector2 (moveForce * Time.fixedDeltaTime, characterRigidbody.velocity.y));
		}

		// If the input is right and character is currently left, or vice versa...
		if((horizontalDirection > 0 && !facingRight) ||( horizontalDirection < 0 && facingRight))
			// ... flip the player and their direction.
			Flip();
	}
	void Flip ()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;
		moveForce = -moveForce;
		// Multiply the player's x local scale by -1 to flip it.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	void updateScore()
	{
		// Update the direct score by 1
		score.score += 1;
		// Calculate the percentage using the new score
		score.percentage = ((float)score.score / (float)strokeController.totalStrokeCount)*100;
	}
	

	void  Teleport ()
	{
		float newPosition;
		// If the player character is facing right, set new position to left wall.
		if (facingRight) {
			newPosition = levelManager.leftWall-1;
			if(grounded ){

			}
		// If the player is facing left, set new position to right wall.
		} else {
			newPosition = levelManager.rightWall+1;
		}

		// Apply the new position.
		transform.position = new Vector3 (newPosition, transform.position.y, transform.position.z);
	}


	void ReloadGame()
	{			
		// Reload the level.
		SceneManager.LoadScene(SceneManager.GetActiveScene(), LoadSceneMode.Single);
	}
}
