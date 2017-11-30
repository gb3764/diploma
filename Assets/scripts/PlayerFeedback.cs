using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFeedback : MonoBehaviour {

	public GameObject obj;
	public GameObject obj2;
	Vector3 pos = new Vector3(-5f, 0f, 0f);
	Vector3 pos2 = new Vector3(-5f, 2f, 0f);
	Vector3 zamik = new Vector3 (2f, 0f, 0f);
	Vector3 zamikRush = new Vector3 (7.5f, 0f, 0f);
	Vector3 zamikDrag = new Vector3 (12.5f, 0f, 0f);
	Vector3 offset = new Vector3 (0.25f, 0f, 0f);
	bool tempo = false;
	public static bool note = false;
	public static int index = 0;
	public static int index2 = 0;

	public static Sprite C;
	public static Sprite D;
	public static Sprite E;

	void Start() {

		C = Resources.Load ("C", typeof(Sprite)) as Sprite;
		D = Resources.Load ("D", typeof(Sprite)) as Sprite;
		E = Resources.Load ("E", typeof(Sprite)) as Sprite;
		populateScene ();
	}

	void populateScene() {

		for (int i = 0; i < Songs.playedData.Length; i++) {
		
			/*if (Songs.noteData [i] == "good") {
			
				note = true;
			}*/

			if (Songs.tempoData [i] == "rush") {
				
				pos -= offset;
				Instantiate (obj, pos, Quaternion.identity, transform);
				pos += offset;
			}
			else if (Songs.tempoData [i] == "drag") {
			
				pos += offset;
				Instantiate (obj, pos, Quaternion.identity, transform);
				pos -= offset;
			}
			else {
			
				Instantiate (obj, pos, Quaternion.identity, transform);
			}

			Instantiate (obj2, pos2, Quaternion.identity, transform);

			pos += zamik;
			pos2 += zamik;
			note = false;
		}
	}
}
