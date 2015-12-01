using UnityEngine;
using System.Collections;

public class GameItem : MonoBehaviour {
	public float radius;
	
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
		//Debug.Log ("[GameItem] UpdateRadiusByValue value = " + value + ", radius = " + radius + ", rb.mass = " + _rb.mass + ",radiusRate = " + radiusRate);
	}
	
	public void Eat(GameItem gItem) {

	}
	
	public void BeEat(GameItem gItem) {
		//Debug.Log ("[GameItem] BeEat radius = " + radius);
		Destroy (this.gameObject);
	}

	void Awake() {
		_rb = GetComponent<Rigidbody>();
		_baseRadius = radius;
	}
}
