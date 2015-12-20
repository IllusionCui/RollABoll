using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlaneInfo : MonoBehaviour {
	public float max;
	public float min;
	public float baseY;

	private ItemNumCreater[] _itemCreaters;

	public void Reset() {
		if (_itemCreaters != null) {
			foreach(var it in _itemCreaters) {
				it.Reset ();
			}
		}
	}

	public void RemoveItem(GameObject item) {
		if (_itemCreaters != null) {
			foreach(var it in _itemCreaters) {
				if (it.RemoveItem (item)) {
					break;
				}
			}
		}
	}

	public float GetRandomRByIndex() {
		return Random.Range (min, max);
	}

	public Vector3 GetRandomPosInPlane(GameObject item) {
		float r = GetRandomRByIndex ();
		float a = Random.Range (0.0f, 360);
		return new Vector3 (r * Mathf.Sin (a), item.transform.position.y + baseY, r * Mathf.Cos (a));
	}

	private void OnItemsCreate(ItemNumCreater creater, GameObject pick) {
		pick.transform.position = GetRandomPosInPlane (pick);
		pick.transform.SetParent (GameControl.Instance.itemsHolder.transform, true);
	}

	void Awake() {
		_itemCreaters = GetComponents<ItemNumCreater>();
		if (_itemCreaters != null) {
			foreach(var it in _itemCreaters) {
				it.onItemsCreateEvent = OnItemsCreate;
			}
		}
	}
}
