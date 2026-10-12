using UnityEngine;
using System.Collections;

public class Fading : MonoBehaviour
{

	// Use this for initialization
	public Texture2D fadeOutTexture;
	public float fadeSpeed = 0.5F;

	int drawDepth = -100;
	public float alpha = 0.0F;
	int fadeDir = -1;
	public static Fading instance;

	void Start ()
	{
		instance = this;
		Time.timeScale = 1F;
		alpha = 0;
		//BeginFade (-1);
	
	}

	void OnGUI ()
	{
		alpha += fadeDir * fadeSpeed * Time.fixedDeltaTime;
		alpha = Mathf.Clamp01 (alpha);

		GUI.color = new Color (GUI.color.r, GUI.color.g, GUI.color.b, alpha);
		GUI.depth = drawDepth;
		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), fadeOutTexture);
		
	}

	public float BeginFade (int direction)
	{
		/*
		if (direction == 1) {
			alpha = 0;
		} else {
			alpha = 1;
		}
		*/
		fadeDir = direction;
		//Debug.Log ("Begin Fade");
		return (fadeSpeed);

	}
	/*
	void OnLevelWasLoaded ()
	{
		alpha = 1;

	}
	*/







}
