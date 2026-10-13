using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
		
		StartCoroutine (FlyToTarget ());
	}

	IEnumerator FlyToTarget ()
	{
		yield return new WaitForSecondsRealtime (0.5F);
		Vector3 startPos = transform.position;
		Vector3 endPos = UITopControl.instance.tCoin.transform.position;
		float inceaseTime = Random.Range (1.8F, 2.2F);
		float t = 0;
		while (t < 1) {
			t += Time.deltaTime * inceaseTime;
			transform.position = Vector3.Lerp (startPos, endPos, t);
			yield return null;
		}
		Animator ani = UITopControl.instance.tCoin.GetComponent<Animator> ();
		if (ani != null) {
			ani.Play (0);
		}
		GameplayControl.instance.coinCount++;
		GameManager.dataSave.coin = GameplayControl.instance.coinCount;
		AudioManager.instance.PlaySound (AudioClipType.AC_COIN);
		Destroy (gameObject);
	}

}