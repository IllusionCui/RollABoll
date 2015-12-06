using UnityEngine;
using System.Collections;

public class GameItem : MonoBehaviour {
	public float baseSpeedRateOfRadius = 2.0f/3; 
	public float decaySpeedRateOfVolume = 0.25f;
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
	
	public void Eat(GameItem gItem) {

	}
	
	public void BeEat(GameItem gItem) {
		Destroy (this.gameObject);
	}
	
	public float GetSpeed() {
		int rate = Mathf.FloorToInt(GetValue (radius) / GetValue (_baseRadius));
		return baseSpeedRateOfRadius * radius * Mathf.Pow (decaySpeedRateOfVolume, rate);
	}
	
	public Vector3 GetDirection() {
		Vector3 initSpeed = new Vector3 (transform.localPosition.z, 0, -transform.localPosition.x) * (dir ? 1 : -1);
		return initSpeed / initSpeed.magnitude;
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
			_rb.velocity = GetCricleRunVector();
		}
	}
}
