using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineBombEffect : MonoBehaviour
{

	public GameObject line1, line2;
	public LineBombType type;

	void Start ()
	{
		Destroy (gameObject, 1F);
	}

	void Update ()
	{
		switch (type) {
		case LineBombType.Horizontal: 
			transform.rotation = Quaternion.Euler (0, 0, 0);
			line1.transform.position += new Vector3 (-0.2F, 0, 0);
			line2.transform.position += new Vector3 (0.2F, 0, 0);
			break;
		case LineBombType.Vertical: 
			transform.rotation = Quaternion.Euler (0, 0, 90);
			line1.transform.position += new Vector3 (0, -0.2F, 0);
			line2.transform.position += new Vector3 (0, 0.2F, 0);
			break;
		}

	}
}

public enum LineBombType
{
	Horizontal,
	Vertical
}
