using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameControl : MonoBehaviour {
	public GameObject plane;
	public GameObject itemsHolder;
	public GameObject player;

	public GameObject startBox;
	public GameObject endBox;

	private ItemNumCreater[] _itemCreaters;
	private PlayerController _pControl;
	private GameObject _currBox;

	public void StartGame() {
		CloseBox ();
		_pControl.IsOperationable = true;
	}

	public void GameOver() {
		_pControl.Reset();
		ShowBox (endBox);
	}

	public void ShowBox(GameObject box) {
		CloseBox ();
		if (box != null) {
			_currBox = box;
			_currBox.SetActive (true);
		}
	}

	public void CloseBox() {
		if (_currBox != null) {
			_currBox.SetActive (false);
		}
	}

	public void InitGameView() {
		_pControl = player.GetComponent<PlayerController>();
		_pControl.Reset();
		if (_itemCreaters != null) {
			foreach(var it in _itemCreaters) {
				it.Reset ();
			}
		}
	}

	public void OnItemBeEat(GameObject item) {
		if (_itemCreaters != null) {
			foreach(var it in _itemCreaters) {
				if (it.RemoveItem (item)) {
					break;
				}
			}
		}
	}

	public Vector3 GetRandomPosInPlane(GameObject item) {
		float r = plane.transform.localScale.x*5;
		r = Random.Range (0, r);
		float a = Random.Range (0.0f, 360);
		return new Vector3 (r * Mathf.Sin (a), item.transform.position.y, r * Mathf.Cos (a));
	}
	
	private static GameControl _instance = null;
	public static GameControl Instance {
		get {
			return _instance;
		}
	}
	
	void Awake() {
		if (_instance != null && this != _instance) {
			Destroy(this.gameObject);
			return;
		}
		_instance = this;
		DontDestroyOnLoad (this);

		_itemCreaters = GetComponents<ItemNumCreater>();
		if (_itemCreaters != null) {
			foreach(var it in _itemCreaters) {
				it.onItemsCreateEvent = OnItemsCreate;
			}
		}
	}

	void Start() {
		InitGameView ();
		ShowBox (startBox);
	}

	private void OnItemsCreate(GameObject pick) {
		pick.transform.position = GetRandomPosInPlane (pick);
		pick.transform.SetParent (itemsHolder.transform, true);
	}
}
