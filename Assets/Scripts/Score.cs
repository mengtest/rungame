using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Score : MonoBehaviour
{
	public int score = 0;	// The player's score.
	public float percentage = 0; // The percentage of the level that is complete.

	void Update ()
	{
		int displayPercentage = Mathf.RoundToInt (percentage);
		// Set the score text.
		GetComponent<GUIText>().text = displayPercentage  + "%" ;
		if (displayPercentage == 100) {
			int x = (int)SceneManager.GetActiveScene () as int;
			x++;
			SceneManager.LoadScene(x, LoadSceneMode.Single);
		}
	}

}
