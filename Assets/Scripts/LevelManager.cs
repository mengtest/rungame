﻿using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

	public Vector2 levelWidth;
	public float leftWall;
	public float rightWall;
	public float width;
	public float midway;
	public float third;
	public float quarter;
	public int currentLevel;
	private int highestLevel;

	// Use this for initialization
	void Awake () {
		Vector2 levelSize = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
		width = (levelSize.x * 2)+1;
		leftWall = -width/2;
		rightWall = width/2;
		midway = width / 2;
		third = width / 3;
		quarter = width / 4;
		highestLevel = 1;
		currentLevel = 1;
	}

	public void nextLevel() {
		if (currentLevel == highestLevel) {
			currentLevel = 1;
		} else {
			currentLevel++;

		}
	}
}
