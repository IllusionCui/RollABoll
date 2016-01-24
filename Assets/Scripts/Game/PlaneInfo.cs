using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlaneInfo : MonoBehaviour {
	public GameObject rootPlane;
	public float max;
	public float min;
	public float baseY;

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

	public float GetRandomRByIndex() {
		return Random.Range (min, max)*rootPlane.transform.localScale.x;
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
			for (int i = 0; i < _itemCreaters.Length; i++) {
				_itemCreaters[i].onItemsCreateEvent = OnItemsCreate;
			}
		}
	}
}
