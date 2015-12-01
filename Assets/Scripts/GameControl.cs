using UnityEngine;
using System.Collections;

public class GameControl : MonoBehaviour {
	public GameObject plane;
	public GameObject pickPerfab;
	public GameObject pickHolder;
	public int pickNum = 1000;
	public GameObject player;

	private int _pickNum = 0;

	public void StartGame() {
		PlayerController pControl = player.GetComponent<PlayerController>();
		pControl.SetStartPos (GetRandomPosInPlane (pControl));

		_pickNum = 0;
		while (_pickNum < pickNum) {
			CreatePick();
		}
		StartCoroutine ("CheckCreateNewPick");
	}

	public Vector3 GetRandomPosInPlane(GameItem item) {
		float r = plane.transform.localScale.x*5 - item.radius;
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
	}

	void Start() {
		StartGame ();
	}

	IEnumerator CheckCreateNewPick() {
		while(true) {
			yield return new WaitForSeconds(0.2f);

			if (_pickNum < pickNum) {
				CreatePick();
			}
		}
	}

	private GameObject CreatePick() {
		GameObject pick = Instantiate (pickPerfab);
		GameItem gItem = player.GetComponent<GameItem>();
		pick.transform.position = GetRandomPosInPlane (gItem);
		pick.transform.SetParent (pickHolder.transform, true);

		_pickNum++;
		return pick;
	}
}
