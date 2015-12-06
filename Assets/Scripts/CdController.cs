using UnityEngine;
using System.Collections;

[System.Serializable]
public class CdController {
	public float timeLength;

	private float _timeStart;
	private bool _active;

	public bool Active {
		set {
			_active = value;
			if (_active) {
				_timeStart = Time.time;
			}
		}
		get {
			return _active;
		}
	}

	public bool IsFinished {
		get { return Active && (Time.time - _timeStart >= timeLength); }
	}
}
