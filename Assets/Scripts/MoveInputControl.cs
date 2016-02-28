using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveInputItem {
	private int _order = -1;
	private bool _active = false;
	private float _activeTime = 0;
	private Vector3 _pos = new Vector3(0, 0, 0);
	
	public int Order {
		get {
			return _order;
		}
		set {
			_order = value;
		}
	}
	
	public bool Active {
		get {
			return _active;
		}
		set {
			_active = value;
		}
	}
	
	public Vector3 Position {
		get {
			return _pos;
		}
		set {
			_pos = value;
		}
	}
	
	public void UpdateActiveTime() {
		_activeTime = Time.time;
	}
	
	public bool CheckInterval(float interval) {
		return (Time.time - _activeTime) >= interval;
	}
	
	public void LogSelf() {
		Debug.Log ("InputItem\t  _activeTime = " + _activeTime + "\t _active ="  + _active + "\t _order ="  + _order);
	}
}

public class MoveInputControl : MonoBehaviour {
	public float operateInterval;

	private Dictionary<int, MoveInputItem> _inputItems = new Dictionary<int, MoveInputItem>();

	public void Reset() {
		_inputItems.Clear();
	}

	public MoveInputItem getMaxActiveOrderItem() {
		MoveInputItem item = null;
		int order = 0;
		foreach(var it in _inputItems) {
			if (it.Value.Active) {
				if (null == item || it.Value.Order > order) {
					item = it.Value;
					order = it.Value.Order;
				}
			}
		}
		return item;
	}

	public bool CheckItemInterval(ref MoveInputItem item) {
		if (null != item && item.CheckInterval(operateInterval)) {
			item.UpdateActiveTime();
			return true;
		}
		return false;
	}
	
	bool AddInputItem(int key, Vector3 pos) {
		bool res = RemoveInputItem (key);
		MoveInputItem item = new MoveInputItem();
		_inputItems.Add(key, item);
		item.Position = pos;
		item.Active = !Helper.isPositionInUI (pos);
		item.Order = _inputItems.Count;
		return res;
	}
	
	void UpdateInputItemPos(int key, Vector3 pos) {
		if(_inputItems.ContainsKey(key)) {
			_inputItems[key].Position = pos;
		}
	}
	
	bool RemoveInputItem(int key) {
		if(_inputItems.ContainsKey(key)) {
			_inputItems.Remove(key);
			foreach(var it in _inputItems) {
				if (it.Value.Order > _inputItems.Count) {
					it.Value.Order -= 1;
				}
			}
			return true;
		}
		return false;
	}

	void Update () {
		// mouse
		{
			int key = -1;
			if (Input.GetMouseButtonDown (0)) {
				AddInputItem(key, Input.mousePosition);
			} else if (Input.GetMouseButtonUp (0)) {
				RemoveInputItem(key);
			}
			UpdateInputItemPos(key, Input.mousePosition);
		}
		// touches
		{
			for (var i = 0; i < Input.touchCount; ++i) {
				Touch touch = Input.GetTouch(i);
				int key = touch.fingerId;
				if (touch.phase == TouchPhase.Began) {
					AddInputItem(key, touch.position);
				} else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) {
					RemoveInputItem(key);
				}
				UpdateInputItemPos(key, touch.position);
			}
		}
	}
}
