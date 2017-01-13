using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

	public Vector2 levelWidth;
	public float leftWall;
	public float rightWall;
	public float width;
	public float midway;
	public float third;
	public float quarter;

	// Use this for initialization
	void Awake () {
		Vector2 levelSize = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
		width = (levelSize.x * 2)+1;
		leftWall = -width/2;
		rightWall = width/2;
		midway = width / 2;
		third = width / 3;
		quarter = width / 4;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
