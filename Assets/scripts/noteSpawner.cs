using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class noteSpawner : MonoBehaviour {

	public static float speed = 1.0f;
	public static string name = "";
	public GameObject obj;
	public static Vector3 pos = new Vector3(940f, 345f, 0f);
	public static int idx = 0;
	public static noteSpawner Instance;
	static float bpm = 1.0f;
	static float delay = 0.0f;
	public static int tabIdx = 0;
	static int length = 0;
	static string[] noteArray;

	void Start() {
	
		Instance = this;
		//pos = spawnPoint.getPosition ();
		//kuzaPazi ();
	}

	public static void Spawn(int song) {

		int tempo = PlayerPrefs.GetInt ("tempo", 60);

		if (tempo == 60) {

			speed = 2.85f;
			delay = 1f;
		}
		else if (tempo == 90) {

			speed = 2.85f;
			delay = 0.5f;
		}
		else if (tempo == 120) {

			speed = 2.85f;
			delay = 0.25f;
		}

		idx = 0;
		tabIdx = 0;
		bpm = 60f / tempo;
	
		if (song == 0) {
		
			kuzaPazi ();
		}
		else if (song == 1) {

			cukSeJeOzenil();
		}
		else if (song == 2) {

			plugInBaby();
		}
	}

	public static void kuzaPazi() {
	
		/*for (int i = 0; i < Songs.kuzapaziNotes.Length; i++) {

			name = Songs.kuzapaziNotes [i];
			Instantiate (obj, pos, Quaternion.identity);
			StartCoroutine (Wait ());
		}*/
		//InvokeRepeating ("SpawnNote", 0, frequency);
		/*tabIdx = 0;
		speed = 2.85f;
		bpm = 0.5f;
		idx = 0;
		delay = 0.25f;*/
		noteArray = (string[]) Songs.kuzapaziNotes.Clone ();
		length = Songs.kuzapaziNotes.Length;
		Instance.InvokeRepeating("SpawnNote", delay, bpm);

	}

	public static void cukSeJeOzenil() {
	
		/*tabIdx = 0;
		speed = 3.35f;
		bpm = 0.25f;
		idx = 0;
		delay = 0.5f;*/
		noteArray = (string[]) Songs.cuksejeozenilNotes.Clone ();
		length = Songs.cuksejeozenilNotes.Length;
		Instance.InvokeRepeating("SpawnNote", delay, bpm);
	}

	public static void plugInBaby() {
	
		/*tabIdx = 0;
		speed = 3.85f;
		bpm = 0.125f;
		idx = 0;
		delay = 1.0f;*/
		noteArray = (string[]) Songs.pluginbabyNotes.Clone ();
		length = Songs.pluginbabyNotes.Length;
		Instance.InvokeRepeating("SpawnNote", delay, bpm);
	}
	/*static IEnumerator Wait() {
	
		yield return new WaitForSeconds (1);
	}*/

	void SpawnNote() {

		//name = Songs.kuzapaziNotes [idx];
		name = noteArray [idx];
		if (name != "X") {
		
			Instantiate (obj, pos, Quaternion.identity, Instance.transform);
		}

		idx++;
		if (idx >= length) {

			CancelInvoke ("SpawnNote");
		}
	}
}
