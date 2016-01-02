using UnityEngine;
using System.Collections;

[System.Serializable]
public class ValueController {
	public float[] levelLimits;

	private float value;

	public void AddValue(float addValue) {
		value += addValue;
	}

	public void ResetValue() {
		value = 0;
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
