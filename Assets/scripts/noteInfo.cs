using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class noteInfo : MonoBehaviour {

	SpriteRenderer[] renderer;

	void Start() {

		renderer = GetComponentsInChildren<SpriteRenderer> ();

		if (Songs.noteData[PlayerFeedback.index] == "good") {

			gameObject.GetComponent<SpriteRenderer> ().color = Color.green;
		}
		else {

			gameObject.GetComponent<SpriteRenderer> ().color = Color.red;
		}

		/*if (Songs.playedData [PlayerFeedback.index] == "C") {
		
			Debug.Log (PlayerFeedback.index + " " + Songs.playedData[PlayerFeedback.index]);
			renderer[1].sprite = PlayerFeedback.C;
		}*/

		PlayerFeedback.index++;
	}
}
