using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PlatformJSON
{
	public float sizeX;
	public float sizeY;
	public float leftCenterX;
	public float leftCenterY;

	public static PlatformJSON CreateFromJSON(string jsonString)
	{
		Debug.Log(JsonUtility.FromJson<PlatformJSON> (jsonString));
		return JsonUtility.FromJson<PlatformJSON> (jsonString);
	}
}