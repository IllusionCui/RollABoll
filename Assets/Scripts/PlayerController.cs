using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : GameItem {
	public CdController cdController;

	private bool _isUserOperation;
	private Vector3 _lastUserOperationPos;
	private bool _operationDir;// true : far from center; false : go to center

	void FixedUpdate() {
		CricleOperation();
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
				float disX = (pos.x - _lastUserOperationPos.x) / Screen.width;
				float abDisX = Mathf.Abs(disX);
				Debug.Log("pos = " + pos + ", _lastUserOperationPos = " + _lastUserOperationPos + ", disX = " + disX + ",config.operationLimit = " + config.operationLimit + ", abDisX = " + abDisX);
				if (abDisX > config.operationLimit) {
					if (disX > 0) {
						_operationDir = !dir;
					} else {
						_operationDir = dir;
					}
					// turn 
					cdController.Active = true;
					cdController.timeLength = abDisX*config.cricleTimeRate;
					Debug.Log("timeLength = " + cdController.timeLength + ", _operationDir = " + _operationDir);
				}
			} 
			RecordUserOperationInfo(pos);
		} else {
			ResetUserOperationInfo();
		}
		
		// base rate
		_rb.velocity = GetDirection()*speed*config.moveRate;
		if (cdController != null && cdController.Active) {
			if (cdController.IsFinished) {
				cdController.Active = false;
			} else {
				Vector3 operationSpeed = (new Vector3 (transform.localPosition.x, 0, transform.localPosition.z)).normalized * (_operationDir ? 1 : -1)*speed*config.cricleSpeedRate;
				Debug.Log("operationSpeed = " + operationSpeed + ", _rb.velocity = " + _rb.velocity);
				_rb.velocity = _rb.velocity + operationSpeed;
			}
		}
	}
}
