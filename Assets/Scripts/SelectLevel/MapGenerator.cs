using UnityEngine;
using System.Collections;

public class MapGenerator : MonoBehaviour
{

	public string path;
	public float BASE_WIDTH;

	[SerializeField]
	private WorldCameraBehaviour cam;

	void Awake ()
	{
		//Vector2 scale = CalculateScaleVector ();
		LoadMap ();
	}
	//private LevelFactory factory;
	/*
	public void Init ()
	{
		factory.Init ();
		LoadMap ();
	}
*/
	void LoadMap ()
	{
		Transform root = this.transform;
		//GameObject temp;
		Transform tempTf;
		SpriteRenderer sr;
		Vector2 scale = CalculateScaleVector ();
		float orthoSize = Camera.main.orthographicSize;
		float baseHeight = 0;
		//int startLevel = 0;
		//root.localScale = scale;
		//root.position = Vector3.zero;
		tempTf = null;
		for (int i = 0; i < root.childCount; i++) {
			tempTf = root.GetChild (i);
			//tempTf.localScale = scale;
			//tempTf.localPosition = new Vector3 (0, baseHeight, 0);
			sr = tempTf.GetComponent<SpriteRenderer> ();
		}
		//the last part is add 1/2 into height
		tempTf = root.GetChild (root.childCount - 1);
		//tempTf.localScale = scale;
		sr = tempTf.GetComponent<SpriteRenderer> ();
		//baseHeight -= sr.bounds.size.y / 2f;
		//tempTf.localPosition = new Vector3 (0, baseHeight - sr.bounds.size.y / 2f, 0);
		if (sr != null) {
			baseHeight = tempTf.position.y + sr.bounds.size.y / 2;
			cam.SetBoundary (0, baseHeight);
		}
		//Debug.Log (baseHeight);
	}

	Vector2 CalculateScaleVector ()
	{
		Vector2 v = Vector2.zero;
		float orthoSize = Camera.main.orthographicSize * 2f;
		float curWidth = orthoSize * Screen.width / Screen.height;
		v = new Vector2 (curWidth * 100f / BASE_WIDTH, curWidth * 100f / BASE_WIDTH);
		return v;
	}
}
