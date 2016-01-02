using UnityEngine;
using System.Collections;

[System.Serializable]
public class ValueController {
	public float[] levelLimits;

	public delegate void OnValueChanged(ValueController vc);
	public event OnValueChanged onValueChangedCallBack;

	private float value;

	public void AddValue(float addValue) {
		SetValue (value + addValue);
	}

	public float GetValue() {
		return value;
	}

	public void ResetValue() {
		SetValue (0);
	}

	private void SetValue(float value_) {
		value = value_;
		if (onValueChangedCallBack != null) {
			onValueChangedCallBack.Invoke(this);
		}
	}

	public int GetCurrLevel() {
		if (levelLimits == null && levelLimits.Length == 0) {
			return -1;
		}

		int res = 0;
		for(int i = 0; i < levelLimits.Length; i++) {
			if (levelLimits[i] > value) {
				break;
			}
			res = i + 1;
		}
		return 0;
	}
}
