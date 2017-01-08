using System.Collections;
using UnityEngine;

public class mygui : MonoBehaviour{

	void OnGUI(){
		//drawRect (0, 0, 10, 10);
	}

	public void drawRect(float height, float width, float x, float y)
	{
		Texture2D MyTexture = Resources.Load("PlatformWhiteSprite") as Texture2D;
		GUI.color = new Color(1.0f , 0, 0);//Set color to red
		GUI.DrawTexture(new Rect(0, 0, 10, 10), MyTexture);
		GUI.color = Color.white;//Reset color to white
		Debug.Log("Cat");
	}
}