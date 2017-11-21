using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class noteAnimation : MonoBehaviour {

	float speed;
	Text text;

	void Start () {
	
		speed = noteSpawner.speed;
		text = GetComponentInChildren<Text> ();
		text.text = noteSpawner.name;
	}

	void Update () {

		gameObject.transform.position += Vector3.left * speed;
	}

	void OnTriggerEnter2D(Collider2D other) {

		/*if (other.gameObject.name == "noteEvaluator") {

			if (Songs.playedNotesIsOk[noteSpawner.tabIdx]) {
			
				gameObject.GetComponent<Image> ().color = Color.green;
			}
			else {
			
				gameObject.GetComponent<Image> ().color = Color.red;
			}
			noteSpawner.tabIdx++;
			//Debug.Log ("passed collider");
		}*/

		if (other.gameObject.name == "leftSideNoteDestroyer") {

			Destroy (gameObject);
		}
	}
}
