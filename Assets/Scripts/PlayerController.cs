using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : GameItem {
	public float baseSpeedRateOfRadius = 2.0f/3; 
	public float decaySpeedRateOfVolume = 0.25f;
	public bool dir = true;

	private bool _isUserOperation;
	private Vector3 _lastUserOperationPos;

	void Update() {
		if (Config.Instance.userOperationType == UserOperationType.Cricle) {
			CricleOperation();
		}
	}

	void FixedUpdate() {
		if (Config.Instance.userOperationType == UserOperationType.Free) {
			FreeOperation();
		}
	}

	void OnTriggerEnter(Collider other) {
		GameItem gItem = other.gameObject.GetComponent<GameItem>();
		if (gItem != null) {
			if (radius > gItem.radius) {
				// eat it
				float selfValue = GetValue(radius);
				float add = gItem.GetValue(gItem.radius)*Config.Instance.absorbRate;
				UpdateRadiusByValue(selfValue + add);
				gItem.BeEat(this);
			} else if (radius < gItem.radius) {
				// back
			} // else nothing happened
		}
	}
	
	public float GetSpeed() {
		int rate = Mathf.FloorToInt(GetValue (radius) / GetValue (_baseRadius));
		return baseSpeedRateOfRadius * radius * Mathf.Pow (decaySpeedRateOfVolume, rate);
	}

	public Vector3 GetDirection() {
		Vector3 initSpeed = new Vector3 (transform.localPosition.z, 0, -transform.localPosition.x) * (dir ? 1 : -1);
		return initSpeed / initSpeed.magnitude;
	}

	public Vector3 GetDistanceToCenter() {
		Vector3 res = -transform.position;
		res.y = 0;
		return res;
	}

	public void SetStartPos(Vector3 pos) {
		transform.position = pos;
	}

	// user operation
	private void ResetUserOperationInfo() {
		_isUserOperation = false;
		_lastUserOperationPos = Vector3.zero;
	}
	
	private void RecordUserOperationInfo(Vector3 pos) {
		_isUserOperation = true;
		_lastUserOperationPos = pos;
	}

	private void CricleOperation() {
		// move
		Config config = Config.Instance;
		float speed = GetSpeed ();
		bool hasInput = false;
		Vector3 pos = Vector3.zero;
		if (Input.GetMouseButton (0)) {
			hasInput = true;
			pos = Input.mousePosition;
		}
		if (!hasInput && Input.touchCount > 0) {
			Touch myTouch = Input.GetTouch(0);
			if (myTouch.phase == TouchPhase.Began || myTouch.phase == TouchPhase.Moved) {
				hasInput = true;
				pos = myTouch.position;
			}
		}
		if (hasInput) {
			if (_isUserOperation) {
				// turn 
				float disX = pos.x - _lastUserOperationPos.x;
				float rate = config.moveRate * speed * config.cricleMoveRate / Screen.width;
				Vector3 dirToCenter = new Vector3 (transform.localPosition.x, 0, transform.localPosition.z);
				dirToCenter = dirToCenter.normalized * (disX * rate * (dir ? -1 : 1));
				Debug.Log("dirToCenter = " + dirToCenter + ",disX = " + disX);
				transform.Translate(dirToCenter);
			} 
			RecordUserOperationInfo(pos);
		} else {
			ResetUserOperationInfo();
		}
		
		_rb.velocity = GetDirection()*speed*config.cricleSpeedRate;
	}

	private void FreeOperation() {
		// move
		Config config = Config.Instance;
		float speed = GetSpeed ();
		bool hasInput = false;
		Vector3 pos = Vector3.zero;
		if (Input.GetMouseButton (0)) {
			hasInput = true;
			pos = Input.mousePosition;
		}
		if (!hasInput && Input.touchCount > 0) {
			Touch myTouch = Input.GetTouch(0);
			if (myTouch.phase == TouchPhase.Began || myTouch.phase == TouchPhase.Moved) {
				hasInput = true;
				pos = myTouch.position;
			}
		}
		if (hasInput) {
			if (_isUserOperation) {
				// turn 
				float disX = pos.x - _lastUserOperationPos.x;
				float rate = config.moveRate * speed;
				float disY = pos.y - _lastUserOperationPos.y;
				Vector3 movement = new Vector3 (disX * rate, 0, disY * rate);
				_rb.AddForce (movement * _rb.mass, ForceMode.Impulse);
			} 
			RecordUserOperationInfo(pos);
		} else {
			ResetUserOperationInfo();
		}
	}
}
