using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class noteInfoOrg : MonoBehaviour {

	SpriteRenderer[] renderer;

	void Start() {

		renderer = GetComponentsInChildren<SpriteRenderer> ();

		if (Songs.kuzapaziNotes [PlayerFeedback.index2] == "C") {
		
			renderer [1].sprite = PlayerFeedback.C;
		}
		else if (Songs.kuzapaziNotes [PlayerFeedback.index2] == "D") {

			renderer [1].sprite = PlayerFeedback.D;
		}
		else if (Songs.kuzapaziNotes [PlayerFeedback.index2] == "E") {

			renderer [1].sprite = PlayerFeedback.E;
		}

		PlayerFeedback.index2++;
	}
}
