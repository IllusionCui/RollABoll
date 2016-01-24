using UnityEngine;
using System.Collections;

public class ValueItem : MonoBehaviour {
	public float value;

	public float Value {
		set { 
			value = value;
			if (onValueChangedCallBack != null) {
				onValueChangedCallBack.Invoke (this);
			}
		}
		get { 
			return value;
		}
	}

	public delegate void OnValueChanged(ValueItem item);
	public event OnValueChanged onValueChangedCallBack;
}
