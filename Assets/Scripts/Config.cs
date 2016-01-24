using UnityEngine;
using System.Collections;

public class Config : MonoBehaviour {
	public float density = 10.0f;

	public float speedParmB = 0.01f;
	public float speedParmA = 0.0001f;

	public float moveRate = 1.0f;
	public float absorbRate = 1.0f/6;
	public float absorbLimit = 1.1f;

	// cricle operation
	public float operationLimit = 0.0f;
	public float cricleSpeedRate = 0.3f;
	public float cricleTimeRate = 5.0f;

	private static Config _instance = null;
	public static Config Instance {
		get {
			return _instance;
		}
	}

	void Awake() {
		if (_instance != null && this != _instance) {
			Destroy(this.gameObject);
			return;
		}
		_instance = this;
		DontDestroyOnLoad (this);
	}
}
