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
		return new Vector3 (r * Mathf.Sin (a), item.transform.position.y, r * Mathf.Cos (a));
	}

	private void OnItemsCreate(ItemNumCreater creater, GameObject pick) {
		pick.transform.position = GetRandomPosInPlane (pick);
		if (_itemCreaters != null) {
			for(int i =0; i < 3; i++) {
				bool free = true;
				foreach(var it in _itemCreaters) {
					if (it.CheckWithCollision (pick)) {
						free = false;
						break;
					}
				}
				if (free) {
					break;
				} else {
					if (i == 2) {
						Debug.Log ("hehehheehhehehe");
					}
					pick.transform.position = GetRandomPosInPlane (pick);
				}
			}
		}
		pick.transform.position = pick.transform.position + new Vector3(0, baseY, 0);
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
