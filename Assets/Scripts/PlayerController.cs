using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public enum UserOperationType {
	Free,
	AlwaysUp
}

public class PlayerController : GameItem {
	public UserOperationType userOperationType = UserOperationType.AlwaysUp;
	public float baseSpeedRateOfRadius = 2.0f/3; 
	public float decaySpeedRateOfVolume = 0.25f;

	private bool _isUserOperation;
	private Vector3 _lastUserOperationPos;
	private Vector3 _lastMovement;

	void FixedUpdate() {
		// move
		float speed = GetSpeed ();
		Vector3 movement = Vector3.zero;
		if (userOperationType == UserOperationType.AlwaysUp) {
			movement = new Vector3 (0, 0, speed);
		}
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
				_lastMovement.x = disX*Config.Instance.moveRate*speed;
				if (userOperationType == UserOperationType.Free) {
					float disY = pos.y - _lastUserOperationPos.y;
					_lastMovement.z = disY*Config.Instance.moveRate*speed;
				}
			} 
			_rb.AddForce (_lastMovement*_rb.mass, ForceMode.Impulse);
			RecordUserOperationInfo(pos);
		} else {
			_rb.AddForce (-_lastMovement*_rb.mass, ForceMode.Impulse);
			ResetUserOperationInfo();
		}
		_rb.AddForce (movement*_rb.mass, ForceMode.Impulse);
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

	// user operation
	private void ResetUserOperationInfo() {
		_isUserOperation = false;
		_lastUserOperationPos = Vector3.zero;
		_lastMovement = Vector3.zero;
	}
	
	private void RecordUserOperationInfo(Vector3 pos) {
		_isUserOperation = true;
		_lastUserOperationPos = pos;
	}
}
