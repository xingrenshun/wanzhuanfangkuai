using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WorldCameraBehaviour : MonoBehaviour
{
	//RayhitUnit unit;
	bool isDragging = false;
	Vector3 tempPosition;
	public float lerpValue;
	public Vector3 targetPosition;
	public float uiworldRatio;
	[SerializeField]
	public WorldMapBoundary boundary;
	LevelBtn btnSelected;
	[SerializeField]
	private Vector3 offset;

	public Text debugText;

	void Start ()
	{

		//targetPosition = offset;
	}
	/*
	public override void Init (CameraRaycaster raycaster)
	{
		base.Init (raycaster);        
		targetPosition = Camera.main.transform.position;
	}

	public override void ActiveBehaviour ()
	{
		base.ActiveBehaviour ();
		//camTransform.position = Vector3.zero;
		LocalUserProfile profile = ProfileSystem.localProfile.Profile;
		targetPosition = GameSystem.WorldManager.GetCurrentLookingPosition ();
		targetPosition.x = offset.x;
		targetPosition.z = offset.z;
		targetPosition = GameSystem.WorldManager.worldmapBoundary.ClampCameraPosition (targetPosition, raycaster.Cam.orthographicSize);
		camTransform.position = targetPosition;
	}
*/

	public void SetBoundary (float x, float y)
	{
		boundary = new WorldMapBoundary ();
		boundary.Set (x, y);
	}

	public void FixedUpdate ()
	{
		if (Time.timeScale > 0) {
			CheckPointerDown ();
			CheckPointerUp ();
			CheckPointerDrag ();
			UpdateCamera ();
		}
	}

	void CheckPointerDown ()
	{
		if (Input.GetMouseButtonDown (0)) {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit raycastHit;
			bool hit = Physics.Raycast (ray, out raycastHit);

			if (hit) {

				//raycastHit.transform.gameObject.GetComponent<LevelBtn> ().MouseDown ();
				btnSelected = raycastHit.transform.gameObject.GetComponent<LevelBtn> ();

			} else {
				isDragging = true;
				//tempPosition = raycaster.ConvertFromScreenToWorld(Input.mousePosition);
				tempPosition = Input.mousePosition;

			}

		}
	}

	void CheckPointerUp ()
	{
		if (Input.GetMouseButtonUp (0)) {

			if (btnSelected != null) {
				//btnSelected.MouseUp ();
				//debugText.text = SelectLevelControler.instance.selectLevelPopup.activeInHierarchy.ToString ();

				btnSelected = null;

			}

			if (isDragging) {
				isDragging = false;
			} 

		}

	}

	void CheckPointerDrag ()
	{
		if (Input.GetMouseButton (0) && isDragging) {
			Vector3 cur = Input.mousePosition;
			Vector3 diff = cur - tempPosition;
			diff.x = 0;
			diff.z = 0;
			targetPosition -= diff * uiworldRatio * 2f;
			targetPosition = boundary.ClampCameraPosition (targetPosition, Camera.main.orthographicSize); //GameSystem.WorldManager.worldmapBoundary.ClampCameraPosition (targetPosition, raycaster.Cam.orthographicSize);
			tempPosition = cur;
		}
	}

	public void UpdateCamera ()
	{

		Camera.main.transform.position = Vector3.Lerp (Camera.main.transform.position, targetPosition, lerpValue);
	}
}

[System.Serializable]
public class WorldMapBoundary
{
	[SerializeField]
	public float minY;
	[SerializeField]
	public float maxY;

	public void Set (float min, float max)
	{
		minY = min;
		maxY = max;
	}

	public float Clamp (float value)
	{
		return Mathf.Clamp (value, minY, maxY);
	}

	public Vector3 ClampCameraPosition (Vector3 position, float orthoSize)
	{
		return new Vector3 (position.x, Mathf.Clamp (position.y, minY, maxY - orthoSize), position.z);
	}
}