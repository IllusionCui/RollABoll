using UnityEngine;
using System.Collections;

public class Config : MonoBehaviour {
	public float moveRate = 1.5f;
	public float absorbRate = 1.0f/6;

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
