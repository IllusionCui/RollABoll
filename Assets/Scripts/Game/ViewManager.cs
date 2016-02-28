using UnityEngine;
using System.Collections;

public class ViewManager : MonoBehaviour {
	public GameItem player;

	private float _lastAngle = 0;

	void LateUpdate () {
		if (player != null) {
			Vector3 targetPos = player.transform.position;
			float angle = (Mathf.Atan2 (targetPos.y, targetPos.x)*180/Mathf.PI + 360)%360;
			transform.Rotate(new Vector3(0, 0, angle - _lastAngle));
			_lastAngle = angle;
		}
	}
}
