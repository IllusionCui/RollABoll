using UnityEngine;
using System.Collections;

public class Config : MonoBehaviour {
	public float moveRate = 1.5f;
	public float absorbRate = 1.0f/6;

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
