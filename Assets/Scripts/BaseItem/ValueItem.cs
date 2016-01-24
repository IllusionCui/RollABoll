using UnityEngine;
using System.Collections;

public class ValueItem : MonoBehaviour {
	[SerializeField]
	private float _value;

	public float Value {
		set { 
			_value = value;
			if (onValueChangedCallBack != null) {
				onValueChangedCallBack.Invoke (this);
			}
		}
		get { 
			return _value;
		}
	}

	public delegate void OnValueChanged(ValueItem item);
	public event OnValueChanged onValueChangedCallBack;
}
