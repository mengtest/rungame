using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlatformController : MonoBehaviour {
	public PlatformJSON platformObject;
	public GameObject platformController;
	public ArrayList platformArray = new ArrayList();
	public ArrayList platformJSON = new ArrayList();
	void Awake(){

		// Add the JSON strings to an arraylist
		platformJSON.Add ("{\"sizeX\":\"80\",\"sizeY\":\"5\",\"leftCenterX\":\"-40\",\"leftCenterY\":\"-22.2\"}");
		platformJSON.Add ("{\"sizeX\":\"20\",\"sizeY\":\"5\",\"leftCenterX\":\"20\",\"leftCenterY\":\"-2\"}");
		platformJSON.Add ("{\"sizeX\":\"30\",\"sizeY\":\"5\",\"leftCenterX\":\"-40\",\"leftCenterY\":\"15\"}");
		platformJSON.Add ("{\"sizeX\":\"16\",\"sizeY\":\"5\",\"leftCenterX\":\"24\",\"leftCenterY\":\"15\"}");

		foreach(string jsonstring in platformJSON){
			PlatformJSON result = JsonUtility.FromJson<PlatformJSON>(jsonstring);
			Vector2 size = new Vector2 (result.sizeX, result.sizeY);
			Vector2 leftCenter = new Vector2 (result.leftCenterX, result.leftCenterY);
			generatePlatformArray (size, leftCenter);
		}
	}

	void generatePlatformArray(Vector2 size, Vector2 leftCenter){
		Dictionary<string, Vector2> platform = new Dictionary<string, Vector2>();
		platform.Add("size", size);
		platform.Add ("leftCenter", leftCenter);
		// Add platform dictionary into array at the index that matches the platformID-1
		platformArray.Add(platform);
	}
}