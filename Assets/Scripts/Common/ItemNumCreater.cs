using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemNumCreater : MonoBehaviour {
	public GameObject itemPerfab;
	public int numLimit;
	public float waveInterval;
	public int waveNum;

	public delegate void OnItemsCreate(ItemNumCreater creater, GameObject item); 
	public OnItemsCreate onItemsCreateEvent;

	private bool _isCreating;
	private int _currWave;
	private int _createNumPerWave;
	private HashSet<GameObject> _items = new HashSet<GameObject>();

	public HashSet<GameObject> Items {
		get { 
			return _items;
		}
	}

	public bool TryCreate() {
		if (_isCreating) {
			return false;
		}

		StartCreate ();
		return true;
	}

	public bool CheckWithCollision(GameObject item) {
		Collider baseCollider = item.GetComponent<Collider> ();
		if (baseCollider != null) {
			foreach(var it in _items) {
				if (item == it) {
					continue;
				}
				Collider checkTarget = it.GetComponent<Collider> ();
				if (checkTarget != null && baseCollider.bounds.Intersects(checkTarget.bounds)) {
					return true;
				}
			}
		}
		return false;
	}

	public bool RemoveItem(GameObject item) {
		bool res = _items.Remove(item);
		if (res) {
			Destroy (item);
		}
		return res;
	}

	public void RemoveItems(List<GameObject> items) {
		for (int i = 0; i < items.Count; i++) {
			RemoveItem (items[i]);
		}
	}

	public void Reset() {
		foreach(var it in _items) {
			Destroy (it);
		}
		_items.Clear ();
		FinishCreate ();
	}

	void Update() {
		TryCreate ();
	}

	private void StartCreate() {
		FinishCreate ();
		_isCreating = true;
		_createNumPerWave = (numLimit - _items.Count)/waveNum;
		_createNumPerWave = Mathf.Max (_createNumPerWave, 1);

		StartCoroutine ("CreateItem");
	}

	private void FinishCreate() {
		_isCreating = false;
		_currWave = 0;
		_createNumPerWave = 0;
	}

	IEnumerator CreateItem() {
		while(true) {
			yield return new WaitForSeconds(waveInterval);

			if (onItemsCreateEvent == null || _createNumPerWave == 0 || _currWave >= waveNum || _items.Count >= numLimit) {
				FinishCreate ();
				break;
			}

			for(int i = 0; i < _createNumPerWave; i++) {
				GameObject item = Instantiate (itemPerfab);
				_items.Add (item);
				onItemsCreateEvent.Invoke (this, item);
			}
			_currWave++;
		}
	}
}
