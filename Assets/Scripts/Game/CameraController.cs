using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	public GameItem player;
	
	// Update is called once per frame
	void LateUpdate () {
		if (player != null) {
			Vector3 targetPos = player.transform.position;
			Vector3 pos = targetPos;
			float length = 10.0f;// base the play size
			Vector3 v = player.GetCricleRunVector();
			v = v.normalized;
			pos.x -= length*v.x;
			pos.y += length;
			pos.z -= length*v.z;
			transform.position = pos;
			transform.rotation = Quaternion.LookRotation(targetPos - pos);
		}
	}
}
