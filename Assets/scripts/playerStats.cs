using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerStats : MonoBehaviour {

	public static int stars = 0;
	public static int[] songStars = new int[3];
	public static string[] songNames = {"kuzapazi", "cuksejeozenil", "pluginbaby"};

	/*void Start () {

		for (int i = 0; i < songStars.Length; i++) {
		
			songStars [i] = PlayerPrefs.GetInt (songNames[i], 0);
			stars += songStars [i];
		}

		//Debug.Log (stars);
	}*/

	public static int GetStars() {

		stars = 0;

		for (int i = 0; i < songStars.Length; i++) {

			songStars [i] = PlayerPrefs.GetInt (songNames[i], 0);
			stars += songStars [i];
		}

		return stars;
	}
}
