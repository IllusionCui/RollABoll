using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	public GameItem player;
	public float length = 10.0f;
	public float rate = 0.6f;
	
	// Update is called once per frame
	void LateUpdate () {
		if (player != null) {
			Vector3 targetPos = player.transform.position;
			float dis = player.transform.localScale.x*length;
			Vector3 pos = targetPos;
			Vector3 v = player.GetCricleRunVector();
			v = v.normalized;
			pos.x -= dis*v.x;
			pos.y += dis*rate;
			pos.z -= dis*v.z;
			transform.position = pos;
			transform.rotation = Quaternion.LookRotation(targetPos - pos);
		}
	}
}
