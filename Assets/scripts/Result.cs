using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Result : MonoBehaviour {

	public static Text text;

	void Start () {

		text = GetComponent<Text> ();
	}
	

	public static void DisplayResult (string result, string note) {
	
		if (result == "" && note == "") {
		
			text.text = "";
		} else {
		
			text.text = "frequency: " + result + " Hz\nnote: " + note;
		}
	}

	public static void TestDisplayResult (float[] topTen, float[] topTenFreq, string[] topTenStr) {
	
		text.text = "";

		for (int i = 0; i < topTen.Length; i++) {
		
			text.text += "raw: " + topTen [i] + " freq: " + topTenFreq [i] + " note: " + topTenStr [i] + "\n";
		}
	}

	public static void WriteLineResult (string line) {
	
		text.text += line + "\n";
	}
}
