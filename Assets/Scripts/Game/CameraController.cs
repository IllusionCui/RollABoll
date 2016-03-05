using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	public Transform checkView;
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

		for(int i = 0; i < checkView.childCount; i++) {
			Transform tfp = checkView.GetChild (i).gameObject;
			for(int j = 0; j < tfp.childCount; j++) {
				Transform tf= tfp.GetChild (i).gameObject;
				tf.gameObject.SetActive (CheckInView(tf));
			}
		}
	}

	bool CheckInView (Transform tf){
		Vector3 screenPos = _camera.WorldToScreenPoint(tf.position);
		if (screenPos.x > 0 && screenPos.x < Screen.width && screenPos.y > 0 && screenPos.y < Screen.height) {
			return true;
		}
		return false;
	}

}
