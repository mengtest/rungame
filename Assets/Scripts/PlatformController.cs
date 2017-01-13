using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlatformController : MonoBehaviour {
	// LevelManager is needed to pull information about the level (width, etc)
	private LevelManager levelManager;
	// PlatformJSON contains the platform JSON object
	public PlatformJSON platformObject;

	public GameObject platformController;
	// This array contains the list of platform dictionaries
	public ArrayList platformArray = new ArrayList();
	// This array contains the list of platforms in raw JSON strings
	public ArrayList platformRawJSON = new ArrayList();

	void Awake(){
		levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();

		// Add the JSON strings to an arraylist
		platformRawJSON.Add ("{\"sizeX\":\"" + levelManager.width + "\",\"sizeY\":\"5\",\"leftCenterX\":\"" + levelManager.leftWall + "\",\"leftCenterY\":\"-22\"}");
		platformRawJSON.Add ("{\"sizeX\":\"" + (levelManager.midway - levelManager.quarter) + "\",\"sizeY\":\"5\",\"leftCenterX\":\"" + (levelManager.midway - levelManager.quarter) + "\",\"leftCenterY\":\"-20\"}");
		platformRawJSON.Add ("{\"sizeX\":\"" + (levelManager.midway - levelManager.quarter) + "\",\"sizeY\":\"5\",\"leftCenterX\":\"" + levelManager.leftWall + "\",\"leftCenterY\":\"-20\"}");
		platformRawJSON.Add ("{\"sizeX\":\"" + levelManager.third*2 + "\",\"sizeY\":\"5\",\"leftCenterX\":\"" + -(levelManager.third) + "\",\"leftCenterY\":\"-18\"}");
		// For each of the entries in the platformJSON array, create vectors from the data and send the two vectors
		// to be added into the array of vectors
		foreach(string jsonstring in platformRawJSON){
			PlatformJSON result = JsonUtility.FromJson<PlatformJSON>(jsonstring);
			// This vector is the equivalent of the size value. X is width of the rectangle platform, Y is height.
			Vector2 size = new Vector2 (result.sizeX, result.sizeY);
			// This vector is the point of the center of the left side of the rectangle.
			Vector2 leftCenter = new Vector2 (result.leftCenterX, result.leftCenterY);
			generatePlatformArray (size, leftCenter);
		}
	}
	// Take two vectors that belong to the same platform...
	void generatePlatformArray(Vector2 size, Vector2 leftCenter){
		// Create a dictionary to store them
		Dictionary<string, Vector2> platform = new Dictionary<string, Vector2>();
		// Add each vector to the new dictionary
		platform.Add("size", size);
		platform.Add ("leftCenter", leftCenter);
		// Add platform dictionary into array at the index.
		platformArray.Add(platform);
	}
}