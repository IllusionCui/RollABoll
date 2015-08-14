using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public float speed;
	public Text countText;
	public Text winText;

	private Rigidbody rb;
	private int count;

	void Start() {
		rb = GetComponent<Rigidbody>();
		setCount (0);
		winText.gameObject.SetActive (false);
	}


	void FixedUpdate() {
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		rb.AddForce (movement);
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag("PickUp")) {
			Debug.Log("PlayerController\tPickUp");
			other.gameObject.SetActive(false);
			setCount (count + 1);
			if (count == 12) {
				winText.gameObject.SetActive (true);
			}
		}
	}

	void setCount(int value) {
		count = value;
		countText.text = "Score:\t" + count.ToString ();
	}
}
