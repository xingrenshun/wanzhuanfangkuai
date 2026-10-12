using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class TimerControl : MonoBehaviour
{
	public static TimerControl instance;
	public int heartRebornSecs = 10;
	public int rebornHeartTime;

	void Awake ()
	{
		instance = this;
		heartRebornSecs = GameConfig.rebornHeartSecs;
	}



	public void CountDown ()
	{
		
		if (rebornHeartTime > 0) {
			
			rebornHeartTime--;

			if (rebornHeartTime == 0) {
				if (GameManager.dataSave.heart < 5) {
					GameManager.dataSave.heart++;
					InitNextHeartTime ();
					if (SceneManager.GetActiveScene ().name == "scn_selectLevel") {
						SelectLevelControler.instance.InitHeart ();						
                    }
					else if(SceneManager.GetActiveScene().name == "scn_home")
					{
                        HomeControl.Instance.text.text = GameManager.dataSave.heart + "/5";
                    }
				}

			}
		}
		if (SceneManager.GetActiveScene ().name == "scn_selectLevel") {
			if (GameManager.dataSave.heart < 5) {
				SelectLevelControler.instance.tHeartTime.text = rebornHeartTime / 60 + ":" + rebornHeartTime % 60;
				if (SelectLevelControler.instance.tHeartTime.gameObject.activeInHierarchy == false) {
					SelectLevelControler.instance.tHeartTime.gameObject.SetActive (true);

				}
			} else {
//				Debug.Log ("countDown");
				SelectLevelControler.instance.tHeartTime.gameObject.SetActive (false);
			}
		}
		else if(SceneManager.GetActiveScene().name == "scn_home")
		{
            if (GameManager.dataSave.heart < 5)
            {
                HomeControl.Instance.TimerText.text = rebornHeartTime / 60 + ":" + rebornHeartTime % 60;
                if (HomeControl.Instance.TimerText.gameObject.activeInHierarchy == false)
                {
                    HomeControl.Instance.TimerText.gameObject.SetActive(true);

                }
            }
            else
            {
                //				Debug.Log ("countDown");
                HomeControl.Instance.TimerText.gameObject.SetActive(false);
            }
        }
	}

	public void InitNextHeartTime ()
	{
		GameManager.dataSave.nextHeartTime = DateTime.Now.AddSeconds (heartRebornSecs).Ticks;
		rebornHeartTime = heartRebornSecs;
	}

	public void OnStart ()
	{
		if (GameManager.dataSave.nextHeartTime == 0) {
			GameManager.dataSave.nextHeartTime = DateTime.Now.Ticks;
		}
		DateTime nextHeartTime = new DateTime (GameManager.dataSave.nextHeartTime);
		DateTime now = DateTime.Now;
		while (now > nextHeartTime) {
			if (GameManager.dataSave.heart < 5) {
				GameManager.dataSave.heart++;
			}
			nextHeartTime = nextHeartTime.AddSeconds (heartRebornSecs);
		}
		rebornHeartTime = (int)(nextHeartTime - now).TotalSeconds;
		GameManager.SaveData ();
		InvokeRepeating ("CountDown", 0, 1);
	}
}
