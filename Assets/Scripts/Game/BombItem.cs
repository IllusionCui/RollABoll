using UnityEngine;
using System.Collections;

public class BombItem : MonoBehaviour {
	public CdController bombCdController;
	public float bombRange;

	public void Trigger() {
		bombCdController.Active = true;
	}

	void Update () {
		if (bombCdController.Active && bombCdController.IsFinished) {
			bombCdController.Active = false;
			GameControl.Instance.BombTriggered (this.transform.position, bombRange);
			GameControl.Instance.RemoveItem (this.gameObject);
		}
	}
}
