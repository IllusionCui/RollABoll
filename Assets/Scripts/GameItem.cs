using UnityEngine;
using System.Collections;

public class GameItem : MonoBehaviour {
	public float radius;

	public bool autoCricle = false;
	public bool dir = true;

	protected Rigidbody _rb;
	protected float _baseRadius;
	
	public float GetValue(float r) {
		return Mathf.Pow (r, 3)*3/4;
	}
	
	public void UpdateRadiusByValue(float value) {
		float newRadius = Mathf.Pow (value * 4 / 3, 1.0f / 3);
		float radiusRate = newRadius / radius;
		radius = newRadius;
		_rb.mass = value;
		transform.localScale = transform.localScale * radiusRate;
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

	void Awake() {
		_rb = GetComponent<Rigidbody>();
		_baseRadius = radius;
	}

	void FixedUpdate() {
		if (autoCricle) {
			CricleAction ();
		} 
	}

	protected void CricleAction() {
		_rb.velocity = GetCricleRunVector ();
	}
}
