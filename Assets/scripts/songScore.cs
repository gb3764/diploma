using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class songScore : MonoBehaviour {

	public static Text text;

	void Start () {

		text = GetComponent<Text> ();
	}
	
	public static void DisplayResult (string result) {

		text.text = result;
	}
}
