using UnityEngine;
using System.Collections;

public class PickUpController : MonoBehaviour {
	public Vector3 rotate;

	// Update is called once per frame
	void Update () {
		transform.Rotate (rotate*Time.deltaTime);
	}
}
