using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class StrokeController : MonoBehaviour {
	private float platformStrokeLength = 0f;
	private float platformLength;
	private float totalLengthOfStrokes;
	public int totalStrokeCount = 0;
	private GameObject platformStroke;
	private GameObject platformParent;
	private Vector2 platformStrokeCoords;
	GameObject platformController;
	ArrayList platformArray;
	private int strokeCount;
	private int platformCount;

	// Use this for initialization
	void Start () {
		// Find the PlatformController class and save a reference to it
		platformController = GameObject.Find("PlatformController"); 

		// Save the platformArray variable from the PlatformController class
		// Each entry in the ArrayList is a dictionary which contains two entries which contain Vector2 values:
		// | "Size" | Vector2(x,y) | ---- This is the width (x) and height (y) of the platform.
		// | "LeftCenter" | Vector2(x,y) | ---- This is the center of the left side of the platform.
		platformArray = platformController.GetComponent<PlatformController>().platformArray; 

		// Find the platformStroke prefab and load it into memory.
		platformStroke = Resources.Load<GameObject>("platformStroke");
		platformParent = Resources.Load<GameObject>("platform");

		platformCount = 0;


		// Loop through the platformArray, saving each entry as a dictionary
		foreach (Dictionary<string, Vector2> dictionary in platformArray) {
			// The total horizontal length of the platform to draw.
			// The height is set by the stroke sprite.
			platformLength = dictionary ["size"].x;

			// Mark the left center location of the platform
			platformStrokeCoords = new Vector2 (dictionary ["leftCenter"].x, dictionary ["leftCenter"].y);

			// Create a parent platform object
			GameObject parentPlatformObject = (GameObject)Instantiate(platformParent, platformStrokeCoords, transform.rotation);

			strokeCount = 0;
			// Draw the strokes to fill the coordinates described in the dictionary.
			drawStrokes (parentPlatformObject);
			platformCount++;
		}
	}

	void drawStrokes(GameObject parentPlatformObject){
		// The total length of the platform strokes drawn so far.
		totalLengthOfStrokes = 0f;

		// The length of each stroke
		platformStrokeLength = (platformStroke.GetComponent<SpriteRenderer>().bounds.size.x);

		// While the platform strokes do not cover the platform...
		while (platformLength > totalLengthOfStrokes) {
			// Draw a platform stroke
			GameObject platformStrokeObject = (GameObject)Instantiate (platformStroke, platformStrokeCoords, transform.rotation);
			platformStrokeObject.transform.SetParent (parentPlatformObject.transform, true);
			platformStrokeObject.name = "platform" + platformCount + "-Stroke" + strokeCount; 
			// Update the total length of strokes to include the new stroke
			totalLengthOfStrokes = platformStrokeLength + totalLengthOfStrokes;
			platformStrokeCoords.x = platformStrokeLength + platformStrokeCoords.x;
			strokeCount++;
			totalStrokeCount++;
		}
	}
}
