using UnityEngine;
using System.Collections;

public class GameItem : MonoBehaviour {
	public bool autoCricle = false;
	public bool dir = true;

	protected Rigidbody _rb;
	protected float _baseRadius;
	protected MassItem _massItem;

	public float GetMassValue() {
		return _massItem.Value;
	}

	void Awake() {
		_rb = GetComponent<Rigidbody>();
		_massItem = GetComponent<MassItem>();
		_massItem.onValueChangedCallBack += OnMassItemValueChanged;
		OnMassItemValueChanged (_massItem);
	}

	// 更新顯示
	public void UpdateMassValue(float value) {
		_massItem.Value = value;
	}

	void OnMassItemValueChanged(ValueItem massItem) {
		if (_massItem == massItem) {
			_rb.mass = _massItem.Value * Config.Instance.density;
			transform.localScale = Vector3.one * Mathf.Pow (_massItem.Value * 3 / 4, 1.0f / 3);
		}
	}

	//速度計算相關
	void FixedUpdate() {
		if (autoCricle) {
			CricleAction ();
		} 
	}

	public float GetSpeed() {
		float resA = Mathf.Pow (Config.Instance.speedParmA / Mathf.Pow (transform.localScale.x, 2), 0.5f);
		return Config.Instance.speedParmB + resA;
	}

	public Vector3 GetDirection() {
		Vector3 initSpeed = new Vector3 (transform.localPosition.z, 0, -transform.localPosition.x) * (dir ? 1 : -1);
		return initSpeed.normalized;
	}

	public Vector3 GetCricleRunVector() {
		// move
		float speed = GetSpeed ();
		return GetDirection()*speed*Config.Instance.moveRate;
	}

	protected void CricleAction() {
		_rb.velocity = GetCricleRunVector ();
	}
}
