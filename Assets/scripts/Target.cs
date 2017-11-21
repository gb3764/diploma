using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Target : MonoBehaviour {

	Text text;
	static float bpm = 0.5f;
	// add actual bpms later
	static float[] bpms = {0.5f, 0.5f, 0.5f};
	static int idx = 0;
	string[] tab = {"3", "2", "1", "GO", ""};
	public static Target Instance;
	public static Image image;

	void Start () {

		Instance = this;
		text = GetComponentInChildren<Text> ();
		image = GetComponent<Image> ();
		image.enabled = false;
	}

	public static void Countdown() {

		int tempo = PlayerPrefs.GetInt ("tempo", 60);
		bpm = 60f / tempo;

		image.enabled = true;
		idx = 0;
		//Instance.InvokeRepeating ("Count", 0, bpms [PlayerPrefs.GetInt ("currentsong", 0)]);
		Instance.InvokeRepeating ("Count", 0, bpm);
	}
	
	void Count() {
	
		text.text = tab[idx];
		idx++;

		if (idx >= tab.Length) {
		
			CancelInvoke ("Count");
		}
	}
}
