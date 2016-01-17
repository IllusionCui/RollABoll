using UnityEngine;
using System.Collections;

[System.Serializable]
public class RangeController {
	public int min;
	public int max;

	public int RandInRange() {
		int size = max - min + 1;
		return Random.Range (0, size) + min;
	}

	public bool InRange(int value) {
		if (value >= min && value <= max) {
			return true;
		}

		return false;
	}
}
