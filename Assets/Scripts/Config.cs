using UnityEngine;
using System.Collections;

[System.Serializable]
public enum UserOperationType {
	Free,
	Cricle
}

public class Config : MonoBehaviour {
	public float moveRate = 1.5f;
	public float absorbRate = 1.0f/6;

	// operation
	public UserOperationType userOperationType = UserOperationType.Cricle;
	// cricle
	public float cricleSpeedRate = 100.0f;
	public float cricleMoveRate = 5.0f;

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
