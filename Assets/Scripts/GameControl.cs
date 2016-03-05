using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameControl : MonoBehaviour {
	public RingInfo[] planeInfo;
	public GameObject itemsHolder;
	public GameObject bombItemsHolder;
	public GameObject ringHolder;
	public GameObject player;

	public GameObject startBox;
	public GameObject endBox;
	public GameObject pasueBox;

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

	public void PasueGame() {
		ShowBox (pasueBox);
		Time.timeScale = 0;
	}

	public void ResumeGame() {
		CloseBox ();
		Time.timeScale = 1;
	}

	public void Restart() {
		CloseBox ();
		_pControl.Reset();
		Time.timeScale = 1;
		_pControl.IsOperationable = true;
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
		if (planeInfo != null) {
			for(int i =0; i < planeInfo.Length; i++) {
				planeInfo [i].Reset ();
			}
		}
	}

	public bool RemoveItem(GameObject item) {
		if (planeInfo != null) {
			for(int i =0; i < planeInfo.Length; i++) {
				if (planeInfo [i].RemoveItem (item)) {
					return true;
				}
			}
		}

		Destroy (item);
		return false;
	}

	public void BombTriggered(Vector3 explosionPos, float radius) {
		if (planeInfo != null) {
			for(int i =0; i < planeInfo.Length; i++) {
				planeInfo [i].BombTriggered (explosionPos, radius);
			}
		}
		if (player != null) {
			BombableItem bombableItem = player.GetComponent<BombableItem> ();
			if (bombableItem != null) {
				bombableItem.ExplodeAction (explosionPos, radius);
			}
		}
	}

	public Vector3 GetRandomPosInPlane(GameObject item, int index = 0) {
		if (planeInfo != null) {
			index = Mathf.Min (planeInfo.Length - 1, index);
			if (index >= 0) {
				return planeInfo [index].GetRandomPosInPlane (item);
			}
		}

		return item.transform.localPosition;
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
	}

	void Start() {
		InitGameView ();
		ShowBox (startBox);
	}
}
