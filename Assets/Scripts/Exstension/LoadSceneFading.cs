using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneFading : MonoBehaviour
{

	public static LoadSceneFading instance;
	public Fading fading;

	void Awake ()
	{
		instance = this;
	}

	IEnumerator loadSceneCoroutine;

	public static void LoadScene (string sceneName)
	{
		if (instance.loadSceneCoroutine == null) {
			instance.loadSceneCoroutine = instance.LoadSceneCoroutine (sceneName);
			instance.StartCoroutine (instance.loadSceneCoroutine);
		}
	}

	IEnumerator LoadSceneCoroutine (string sceneName)
	{
		fading.alpha = 0;
		yield return  new WaitForSecondsRealtime (fading.BeginFade (1));
		SceneManager.LoadScene (sceneName);
		yield return null;
		fading.alpha = 1;
		fading.BeginFade (-1);
		loadSceneCoroutine = null;
	}
}
