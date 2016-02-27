using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RingInfo : MonoBehaviour {
	public float radius;
	public float width;

	private ItemNumCreater[] _itemCreaters;

	public void Reset() {
		if (_itemCreaters != null) {
			for (int i = 0; i < _itemCreaters.Length; i++) {
				_itemCreaters[i].Reset ();
			}
		}
	}

	public bool RemoveItem(GameObject item) {
		if (_itemCreaters != null) {
			for (int i = 0; i < _itemCreaters.Length; i++) {
				if (_itemCreaters[i].RemoveItem (item)) {
					return true;
				}
			}
		}

		return false;
	}

	public void BombTriggered(Vector3 explosionPos, float radius) {
		if (_itemCreaters != null) {
			for (int i = 0; i < _itemCreaters.Length; i++) {
				if (_itemCreaters[i].itemPerfab == null || _itemCreaters[i].itemPerfab.GetComponent<BombableItem>() == null) {
					continue;
				}
				List<GameObject> removeItems = new List<GameObject> ();
				foreach(var it in _itemCreaters[i].Items) {
					BombableItem bombableItem = it.GetComponent<BombableItem> ();
					if (bombableItem.ExplodeAction(explosionPos, radius)) {
						removeItems.Add (it);
					}
				}
				_itemCreaters[i].RemoveItems (removeItems);
			}
		}
	}

	public Vector3 GetRandomPosInPlane(GameObject item) {
		float a = Random.Range (0.0f, 360);
		float w = Random.Range (0, width);
		float r = radius - 0.5f;
		return new Vector3 (r * Mathf.Sin (a), r * Mathf.Cos (a), w);
	}

	private void OnItemsCreate(ItemNumCreater creater, GameObject pick) {
		pick.transform.position = GetRandomPosInPlane (pick);
		if (_itemCreaters != null) {
			for(int i =0; i < 3; i++) {
				bool free = true;
				for(int j = 0; j < _itemCreaters.Length; j++) {
					if (_itemCreaters[j].CheckWithCollision (pick)) {
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
		pick.transform.SetParent (GameControl.Instance.itemsHolder.transform, true);
	}

	void Awake() {
		_itemCreaters = GetComponents<ItemNumCreater>();
		if (_itemCreaters != null) {
			for (int i = 0; i < _itemCreaters.Length; i++) {
				_itemCreaters[i].onItemsCreateEvent = OnItemsCreate;
			}
		}
	}
}
