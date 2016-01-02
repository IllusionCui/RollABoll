using UnityEngine;
using System.Collections;

public class ValueItem : MonoBehaviour {
	public float value;

	public delegate void OnValueChanged(ValueItem item);
	public event OnValueChanged onValueChangedCallBack;
}
