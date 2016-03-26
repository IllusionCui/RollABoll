using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RingInfo : MonoBehaviour {
	public float height;
	public float width;
	public int viewNum;
	public GameObject[] viewPerfab;

	public float radius;
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

	public Vector3 GetRandomPosInPlane(GameObject item, out float angle, float delR) {
		angle = Random.Range (0.0f, 360);
		float a = angle / 180 * Mathf.PI;
		float w = Random.Range (-width/2, width/2);
		float r = radius - delR;
		if (delR > 0) {
			Debug.Log ("a = " + a + ", w = " + w + ", r = " + r + ", delR = " + delR);
		}
		return new Vector3 (r * Mathf.Cos (a), r * Mathf.Sin (a), w);
	}

	public float GetCenterW() {
		return transform.position.z + width/2;
	}

	private void OnItemsCreate(ItemNumCreater creater, GameObject pick) {
		float angle = 0;
		pick.transform.position = GetRandomPosInPlane (pick, out angle, 0);
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
					pick.transform.position = GetRandomPosInPlane (pick, out angle, 0);
				}
			}
		}
		pick.transform.Rotate (new Vector3(0, 0, angle));
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
		float angleP = 2 * Mathf.PI / viewNum;
		radius = height / 2 / Mathf.Sin (angleP/2);
		for(int i = 0; i < viewNum; i++) {
			int index = Random.Range (0, viewPerfab.Length - 1);
			GameObject view = Instantiate (viewPerfab[index]);
			view.transform.Rotate (new Vector3(angle * i, 0, 0));
			view.transform.position = new Vector3 (radius*Mathf.Cos(angleP*i), radius*Mathf.Sin(angleP*i), 0);
			view.transform.SetParent (GameControl.Instance.ringHolder.transform, true);
		}
	}
}
