using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RingInfo : MonoBehaviour {
	public float radius;
	public float width;
	public int viewNum;
	public GameObject[] viewPerfab;

	private ItemNumCreater[] _itemCreaters;
	private List<GameObject> _views = new List<GameObject>();

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
		float w = Random.Range (-width/2, width/2);
		float r = radius - 0.5f;
		return new Vector3 (r * Mathf.Sin (a), r * Mathf.Cos (a), w);
	}

	public float GetCenterW() {
		return transform.position.z + width/2;
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

		float angle = 360 / viewNum;
		for(int i = 0; i < viewNum; i++) {
			int index = Random.Range (0, viewPerfab.Length - 1);
			GameObject view = Instantiate (viewPerfab[index]);
			view.transform.Rotate (new Vector3(angle*i, 0, 0));
			view.transform.SetParent (GameControl.Instance.ringHolder.transform, true);
		}
	}
}
