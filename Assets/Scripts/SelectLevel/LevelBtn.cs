using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using TMPro;
using UnityEngine.UI;

public class LevelBtn : MonoBehaviour
{

	// Use this for initializatio


	public int level;


	public TextMeshProUGUI number;

	public GameObject starGroup;
	public GameObject[] stars;
	public Sprite Star, UnStar;
	public Material norFont, overFont;

	Image spr;
	//	Vector3 startPost;
	void OnValidate ()
	{
		TextInit ();
		gameObject.name = "Level_" + level;
	}

	void Start ()
	{
		/*
		GameObject lvBtnGroup = GameObject.FindGameObjectWithTag ("Item");
		int children = lvBtnGroup.transform.childCount;
		for (int i = 0; i < children; i++) {
			lvBtnGroup.transform.GetChild (i).gameObject.GetComponent<LevelBtn> ().level = i + 1;
			//Debug.Log ("lv : " + i);
		}
		*/
		if (Application.isPlaying) {		
			if (level == GameManager.dataSave.lastLevel + 1) {
				
				if (transform.position.y < 7) {
					//Camera.main.GetComponent<WorldCameraBehaviour> ().targetPosition = new Vector3 (0, transform.position.y + 5, -10);
					//Camera.main.transform.position = new Vector3 (0, transform.position.y + 5, -10);
				} else {
					WorldCameraBehaviour cameraBehaviour = Camera.main.GetComponent<WorldCameraBehaviour> ();
					cameraBehaviour.targetPosition = new Vector3 (0, transform.position.y, -10);
					Debug.Log ("targetPos = " + cameraBehaviour.targetPosition.y);
					cameraBehaviour.targetPosition = new Vector3 (0, Mathf.Clamp (cameraBehaviour.targetPosition.y, 0, cameraBehaviour.boundary.maxY - Camera.main.orthographicSize), -10);
					Debug.Log ("targetPosClamp = " + cameraBehaviour.targetPosition.y);
					Camera.main.transform.position = cameraBehaviour.targetPosition;
					//Camera.main.GetComponent<WorldCameraBehaviour> ().UpdateCamera ();		
				}

			} 
			if (level == GameManager.dataSave.levelMax) {
				Debug.Log ("leve max is " + level);
				//SelectLevelControler.instance.highLight.transform.position = transform.position;
			}
			//startPost = number.transform.localPosition;
			spr = GetComponent<Image> ();
			TextInit ();
			StarInit ();
			SpriteInit ();
		}

	}
	
	// Update is called once per frame

	void SpriteInit ()
	{
		spr.sprite = level > GameManager.dataSave.levelMax ? SelectLevelControler.instance.sprLevelButtonNoPassed : SelectLevelControler.instance.sprLevelButtonPassed;
	}

	void TextInit ()
	{
		number.text = level.ToString ();
		number.fontSharedMaterial = level > GameManager.dataSave.levelMax ? norFont : overFont;
        //number.offsetZ = -2;
    }

	void StarInit ()
	{
		for (int i = 0; i < 3; i++) {

            //stars [i].transform.parent.GetComponent<SpriteRenderer> ().sprite = SelectLevelControler.instance.star0;
            //stars [i].GetComponent<SpriteRenderer> ().sprite = SelectLevelControler.instance.star;

        }


        int star = GameManager.dataSave.stagesList[level].stars;
        if (star == 3)
        {
            //numStar = 3;
            stars[0].SetActive(true);
            stars[1].SetActive(true);
            stars[2].SetActive(true);
            stars[0].GetComponent<Image>().sprite = Star;
            stars[1].GetComponent<Image>().sprite = Star;
            stars[2].GetComponent<Image>().sprite = Star;
        }
        else if (star == 2)
        {
            //numStar = 2;
            stars[0].SetActive(true);
            stars[1].SetActive(true);
            stars[2].SetActive(true);
            stars[0].GetComponent<Image>().sprite = Star;
            stars[1].GetComponent<Image>().sprite = Star;
            stars[2].GetComponent<Image>().sprite = UnStar;
        }
        else if (star == 1)
        {
            //numStar = 1;
            stars[0].SetActive(true);
            stars[1].SetActive(true);
            stars[2].SetActive(true);
            stars[0].GetComponent<Image>().sprite = Star;
            stars[1].GetComponent<Image>().sprite = UnStar;
            stars[2].GetComponent<Image>().sprite = UnStar;
        }
        else
        {
            stars[0].SetActive(false);
            stars[1].SetActive(false);
            stars[2].SetActive(false);
            //starGroup.SetActive(false);
        }


    }



    public void OnMouseDown ()
	{
		Color col = spr.color;
		col.r = 0.8F;
		col.g = 0.8F;
		col.b = 0.8F;
		spr.color = col;
		//Debug.Log ("Level : " + level.ToString ());
	}

	public void OnMouseUp ()
	{
		Color col = spr.color;
		col.r = 1F;
		col.g = 1F;
		col.b = 1F;
		spr.color = col;
		if (Time.timeScale == 0) {
			return;
		}
		if (level <= GameManager.dataSave.levelMax) {
			if (GameManager.dataSave.heart > 0) {
				GameManager.currentLevel = level;
				GameManager.dataSave.lastLevel = level;
				GameManager.SaveData ();
				LoadSceneFading.LoadScene ("scn_play");
			} else {
				SelectLevelControler.instance.MoreHeartPopup.SetActive (true);
			}
		}


	}


}
