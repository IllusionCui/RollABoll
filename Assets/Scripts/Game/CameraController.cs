using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	public GameItem player;
	public float length;
	public float angle;
	
	// Update is called once per frame
	void LateUpdate () {
		if (player != null) {
			Vector3 targetPos = player.transform.position;
			float dis = player.transform.localScale.x * length;
			angle = (angle % 90 + 90 ) % 90;
			Vector3 pos = (1 - dis / targetPos.magnitude) * targetPos - player.GetDirection()*dis/Mathf.Tan(angle);
			transform.position = pos;
			transform.rotation = Quaternion.LookRotation(targetPos - pos);
		}
	}
}
