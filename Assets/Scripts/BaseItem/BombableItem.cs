﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BombableItem : MonoBehaviour {
	public GameObject piecePerfab;
	public RangeController bombPiecesRangeController;
	public float bombRate = 1.0f;
	public float pieceRange = 0.5f;
	public float explodeLimit = 1.0f;

	private Collider _coll;

	public bool ExplodeAction(Vector3 explosionPos, float radius) {
		Vector3 closestPoint = _coll.ClosestPointOnBounds(explosionPos);
		float distance = Vector3.Distance(closestPoint, explosionPos);
		if (radius > distance) {
			int pieceNum = bombPiecesRangeController.RandInRange ();
			float currValue = 0;
			{
				EnergyItem energyItem = GetComponent<EnergyItem> ();
				if (energyItem != null) {
					currValue = energyItem.Value;
					energyItem.Value = currValue * (1 - bombRate);
				}
				MassItem massItem = GetComponent<MassItem> ();
				if (massItem != null) {
					currValue = massItem.Value;
					massItem.Value = currValue * (1 - bombRate);
				}
				currValue = currValue * bombRate;
			}
			while (pieceNum > 1) {
				float resValue = currValue / pieceNum;
				if (resValue > explodeLimit) {
					float scale = gameObject.transform.localScale.x / pieceNum;
					for(int i = 0; i < pieceNum; i++) {
						GameObject item = Instantiate (piecePerfab);
						item.transform.localScale = item.transform.localScale * scale;
						item.transform.position = gameObject.transform.position + new Vector3 (Random.Range(-1*pieceRange, 1*pieceRange), 0, Random.Range(-1*pieceRange, 1*pieceRange));
						item.transform.SetParent (GameControl.Instance.bombItemsHolder.transform, true);

						EnergyItem energyItem = item.GetComponent<EnergyItem> ();
						if (energyItem != null) {
							energyItem.Value = resValue;
						}
						MassItem massItem = item.GetComponent<MassItem> ();
						if (massItem != null) {
							massItem.Value = resValue;
						}
					}
					return true;
				} else {
					pieceNum--;
				}
			}
		}

		return false;
	}

	void Awake() {
		_coll = GetComponent<Collider>();
	}
}
