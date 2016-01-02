using UnityEngine;
using System.Collections;

public class GameItem : MonoBehaviour {
	public float density;
	public float radius;

	public bool autoCricle = false;
	public bool dir = true;

	protected Rigidbody _rb;
	protected float _baseRadius;
	protected MassItem _massItem;

	public float GetMassValue() {
		return _massItem.value;
	}

	void Awake() {
		_rb = GetComponent<Rigidbody>();
		_massItem = GetComponent<MassItem>();
		_massItem.onValueChangedCallBack += OnMassItemValueChanged;
	}

	// 更新顯示
	void OnMassItemValueChanged(ValueItem massItem) {
		if (_massItem == massItem) {
			float newRadius = Mathf.Pow ((_massItem.value/density) * 4 / 3, 1.0f / 3);
			_rb.mass = _massItem.value;
			transform.localScale = Vector3.one * (newRadius/radius);
		}
	}

	//速度計算相關
	void FixedUpdate() {
		if (autoCricle) {
			CricleAction ();
		} 
	}

	public float GetSpeed() {
		float resA = Mathf.Pow (Config.Instance.speedParmA / Mathf.Pow (radius, 2), 0.5f);
		return Config.Instance.speedParmB + resA;
	}

	public Vector3 GetDirection() {
		Vector3 initSpeed = new Vector3 (transform.localPosition.z, 0, -transform.localPosition.x) * (dir ? 1 : -1);
		return initSpeed.normalized;
	}

	public Vector3 GetCricleRunVector() {
		// move
		Config config = Config.Instance;
		float speed = GetSpeed ();
		return GetDirection()*speed*config.moveRate;
	}

	protected void CricleAction() {
		_rb.velocity = GetCricleRunVector ();
	}
}
