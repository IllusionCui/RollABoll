using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : GameItem {
	public CdController operationCdController;
	public ValueController energyValueController;
	public float originRadius;

	private bool _isUserOperation;
	private Vector3 _lastUserOperationPos;
	private bool _operationDir;// true : far from center; false : go to center

	private bool _isOperationable = false;
	public bool IsOperationable{
		set { 
			_isOperationable = value;
		}
		get { return _isOperationable; }
	}

	void FixedUpdate() {
		if (IsOperationable) {
			CricleOperation ();
		} else {
			CricleAction ();
		}
	}

	void OnTriggerEnter(Collider other) {
		CheckCollision (other);
	}

	void OnCollisionEnter(Collision collision) {
		CheckCollision (collision.collider);
	}

	void CheckCollision(Collider other) {
		if (!IsOperationable) {
			return;
		}

		ObstacleItem obstacle = other.gameObject.GetComponent<ObstacleItem>();
		if (obstacle != null) {
			GameControl.Instance.GameOver ();
			return;
		}

		EnergyItem energyItem = other.gameObject.GetComponent<EnergyItem> ();
		if (energyItem != null) {
			energyValueController.AddValue (energyItem.value);
			GameControl.Instance.OnItemBeEat (other.gameObject);
		}

		MassItem massItem = other.gameObject.GetComponent<MassItem>();
		if (massItem != null) {
			Config config = Config.Instance;
			if (_massItem.value >= massItem.value * config.absorbLimit) {
				// eat it
				_massItem.value += massItem.value*config.absorbRate;
				GameControl.Instance.OnItemBeEat (other.gameObject);
			}
		}
	}

	public Vector3 GetDistanceToCenter() {
		Vector3 res = -transform.position;
		res.y = 0;
		return res;
	}

	public void Reset() {
		IsOperationable = false;
		UpdateRadius (originRadius);
		transform.position = GameControl.Instance.GetRandomPosInPlane (this.gameObject);
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
//				Debug.Log("pos = " + pos + ", _lastUserOperationPos = " + _lastUserOperationPos + ", disX = " + disX + ",config.operationLimit = " + config.operationLimit + ", abDisX = " + abDisX);
				if (abDisX > config.operationLimit) {
					if (disX > 0) {
						_operationDir = !dir;
					} else {
						_operationDir = dir;
					}
					// turn 
					operationCdController.Active = true;
					operationCdController.timeLength = abDisX*config.cricleTimeRate;
//					Debug.Log("timeLength = " + operationCdController.timeLength + ", _operationDir = " + _operationDir);
				}
			} 
			RecordUserOperationInfo(pos);
		} else {
			ResetUserOperationInfo();
		}
		
		// base rate
		_rb.velocity = GetDirection()*speed*config.moveRate;
		if (operationCdController != null && operationCdController.Active) {
			if (operationCdController.IsFinished) {
				operationCdController.Active = false;
			} else {
				Vector3 operationSpeed = (new Vector3 (transform.localPosition.x, 0, transform.localPosition.z)).normalized * (_operationDir ? 1 : -1)*speed*config.cricleSpeedRate;
//				Debug.Log("operationSpeed = " + operationSpeed + ", _rb.velocity = " + _rb.velocity);
				_rb.velocity = _rb.velocity + operationSpeed;
			}
		}
	}
}
