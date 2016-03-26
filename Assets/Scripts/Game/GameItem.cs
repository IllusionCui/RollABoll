using UnityEngine;
using System.Collections;

public class GameItem : MonoBehaviour {
	public bool dir = true;

	protected Rigidbody _rb;
	protected Vector3 _baseScale;
	protected MassItem _massItem;

	public float GetMassValue() {
		return _massItem.Value;
	}

	protected virtual void Awake() {
		_baseScale = transform.localScale;
		_rb = GetComponent<Rigidbody>();
		_massItem = GetComponent<MassItem>();
		_massItem.onValueChangedCallBack += OnMassItemValueChanged;
	}

	// 更新顯示
	public void UpdateMassValue(float value) {
		_massItem.Value = value;
	}

	void OnMassItemValueChanged(ValueItem massItem) {
		if (_massItem == massItem) {
			_rb.mass = _massItem.Value * Config.Instance.density;
			transform.localScale = _baseScale * Mathf.Pow (_massItem.Value / _massItem.valueDefalut, 1.0f / 3);
		}
	}

	public float GetSpeed() {
		float resA = Mathf.Pow (Config.Instance.speedParmA / Mathf.Pow (transform.localScale.x, 2), 0.5f);
		return Config.Instance.speedParmB + resA;
	}

	public Vector3 GetDirection() {
		Vector3 initSpeed = new Vector3 (-transform.position.y, transform.position.x, 0) * (dir ? 1 : -1);
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

	public void GotoGroudForce() {
		_rb.AddForce (new Vector3(transform.position.x, transform.position.y, 0).normalized*9.8f, ForceMode.VelocityChange);
	}
}
