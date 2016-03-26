using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameUI : MonoBehaviour {
	public Button energyButton;
	public Text energyText;

	private PlayerController _player;

	void Start() {
		_player = GameControl.Instance.player.GetComponent<PlayerController> ();
		_player.energyValueController.onValueChangedCallBack += OnPlayerEnergyValueChanged;
		OnPlayerEnergyValueChanged (_player.energyValueController);
	}

	void OnPlayerEnergyValueChanged (ValueController vc) {
		energyText.text = string.Format ("{0}/{1}", vc.GetValue(), vc.levelLimits[0]);
		energyButton.interactable = vc.GetCurrLevel() > 0;
	}

	public void OnEnergySkill() {
		_player.DoEnergySkill ();
	}
}
