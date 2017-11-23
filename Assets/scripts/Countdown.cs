using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Countdown : MonoBehaviour {

	public static Countdown Instance;
	public static AudioSource[] posnetek;
	static int counter = 0;
	static int index = 0;

	void Start () {

		posnetek = GetComponents<AudioSource> ();
		Instance = this;
	}

	public static void Count() {
	
		counter = 4;
		int tempo = PlayerPrefs.GetInt ("tempo", 60);
		float bpm = 60f / (float) tempo;
		index = (tempo / 30) - 2;
		posnetek[index].Play ();
		//Instance.InvokeRepeating ("Play", 0f, bpm);
	} 

	/*void Play () {

		posnetek[index].Play ();
		counter--;

		if (counter == 0) {
		
			CancelInvoke ("Play");
		}
	}*/
}
