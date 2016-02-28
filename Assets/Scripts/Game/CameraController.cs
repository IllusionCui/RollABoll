using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	public GameObject itemHolder;
	public GameItem player;
	public RingInfo ringInfo;
	public float length;

	private Camera _camera;

	void Awake() {
		_camera = GetComponent<Camera> ();
	}
	
	// Update is called once per frame
	void LateUpdate () {
		float dis = player.transform.localScale.x * length;
		transform.localPosition = new Vector3(ringInfo.radius - dis, - dis, player.transform.position.z);

		for(int i = 0; i < itemHolder.transform.childCount; i++) {
			GameObject item = itemHolder.transform.GetChild (i).gameObject;
			item.SetActive (CheckInView(item));
		}
	}

	bool CheckInView (GameObject go){
		Vector3 screenPos = _camera.WorldToScreenPoint(go.transform.position);
		if (screenPos.x > 0 && screenPos.x < Screen.width && screenPos.y > 0 && screenPos.y < Screen.height) {
			return true;
		}
		return false;
	}

}
