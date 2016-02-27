﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : GameItem {
	public float originMassValue;
	public CdController operationCdController;
	public CdController energySkillCdController;
	public ValueController energyValueController;

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

	public void DoEnergySkill() {
		energySkillCdController.Active = true;
		energyValueController.ResetValue ();
		_rb.velocity = _rb.velocity * 5;
	}

	void EnergySkillAction() {
		if (operationCdController.IsFinished) {
			operationCdController.Active = false;
		} 
	}

	void FixedUpdate() {
		if (IsOperationable) {
			if (energySkillCdController.Active) {
				EnergySkillAction ();
			} else {
				CricleOperation ();
			}
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
			if (energySkillCdController.Active) {
				energySkillCdController.Active = false;
			} else {
				GameControl.Instance.GameOver ();
			}
			return;
		}

		BombItem bombItem = other.gameObject.GetComponent<BombItem>();
		if (bombItem != null) {
			bombItem.Trigger();
			return;
		}

		bool eat = false;
		EnergyItem energyItem = other.gameObject.GetComponent<EnergyItem> ();
		if (energyItem != null) {
			energyValueController.AddValue (energyItem.Value);
			eat = true;
		}

		MassItem massItem = other.gameObject.GetComponent<MassItem>();
		if (massItem != null) {
			Config config = Config.Instance;
			if (_massItem.Value >= massItem.Value * config.absorbLimit) {
				// eat it
				_massItem.Value += massItem.Value*config.absorbRate;
				eat = true;
			}
		}

		if (eat) {
			GameControl.Instance.RemoveItem (other.gameObject);
		}
	}

	public Vector3 GetDistanceToCenter() {
		Vector3 res = -transform.position;
		res.y = 0;
		return res;
	}

	public void Reset() {
		IsOperationable = false;
		UpdateMassValue (originMassValue);
		energyValueController.ResetValue ();
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
				Vector3 operationSpeed = (new Vector3 (0, 1, 0)) * (_operationDir ? 1 : -1)*speed*config.cricleSpeedRate;
//				Debug.Log("operationSpeed = " + operationSpeed + ", _rb.velocity = " + _rb.velocity);
				_rb.velocity = _rb.velocity + operationSpeed;
			}
		}
	}
}
