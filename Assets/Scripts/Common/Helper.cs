using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Helper {

	public static bool isPositionInUI(Vector3 position) {
		PointerEventData pe = new PointerEventData(EventSystem.current);
		pe.position = position;
		List<RaycastResult> hits = new List<RaycastResult>();
		EventSystem.current.RaycastAll(pe, hits);
		bool hit = false;
		//		Debug.Log("total hits: " + hits.Count);
		foreach (RaycastResult r in hits) {
			if (r.gameObject.GetComponent<Button>()) {
				hit = true;
			}
		}
		return hit;
	}
}
