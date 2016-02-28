using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : GameItem {
	public float originMassValue;
	public MoveInputControl moveInputController;
	public CdController energySkillCdController;
	public ValueController energyValueController;

	private float _energySpeedRate;
	private float _energyAngle;

	private bool _isOperationable = false;
	public bool IsOperationable{
		set { 
			_isOperationable = value;
		}
		get { return _isOperationable; }
	}

	public void DoEnergySkill(float angle = 90) {
		energySkillCdController.Active = true;
		energyValueController.ResetValue ();
		_energyAngle = angle;
		_energySpeedRate = 5f;
	}

	void FixedUpdate() {
		if (IsOperationable) {
			float angle = 0;
			float speedRate = 1;
			if (energySkillCdController.Active && !energySkillCdController.IsFinished) {
				angle = _energyAngle;
				speedRate = _energySpeedRate;
			} else {
				angle = ProcessInput ();
			}
			angle = angle * (Mathf.PI / 180);
			Vector3 circleVector = GetCricleRunVector ();
			Vector3 dirVector = new Vector3(0, 0, circleVector.magnitude / Mathf.Tan(angle)*(dir ? -1 : 1));
			_rb.velocity = (circleVector + dirVector)*speedRate;
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

	public void Reset() {
		IsOperationable = false;
		UpdateMassValue (originMassValue);
		energyValueController.ResetValue ();
//		transform.position = GameControl.Instance.GetRandomPosInPlane (this.gameObject);
	}

	float ProcessInput() {
		int touchPos = 0; // 0 - none, 1 - left, 2 - right
		if (null != moveInputController) {
			MoveInputItem item = moveInputController.getMaxActiveOrderItem ();
			if (moveInputController.CheckItemInterval(ref item)) {
				touchPos = item.Position.x > Screen.width / 2 ? 2 : 1;
			}
		}
		if (Input.GetKeyDown(KeyCode.RightArrow) || touchPos == 2) {
			return Config.Instance.operationAngle;
		} else if (Input.GetKeyDown(KeyCode.LeftArrow) || touchPos == 1) {
			return 180 - Config.Instance.operationAngle;
		}

		return 90;
	}
}
