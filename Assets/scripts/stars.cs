using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class stars : MonoBehaviour {

	public static Image[] images;
	public static Sprite whiteStar;
	public static Sprite goldStar;
	public static int songIndex = 0;

	void Start () {

		whiteStar = Resources.Load ("starwhite", typeof(Sprite)) as Sprite;
		goldStar = Resources.Load ("stargold", typeof(Sprite)) as Sprite;
		images = GetComponentsInChildren<Image> ();
		//Refresh ();
		Disable ();
	}

	public static void Enable() {

		for (int i = 0; i < images.Length; i++) {

			images [i].enabled = true;
		}
	}

	public static void Disable() {

		for (int i = 0; i < images.Length; i++) {

			images [i].enabled = false;
		}
	}

	/*public static void Refresh() {
	
		for (int i = 0; i < images.Length; i++) {

			if (playerStats.songStars [songIndex] > i) {

				images [i].sprite = goldStar;
			}
			else {

				images [i].sprite = whiteStar;	
			}
		}
	}*/

	public static void Set(int num) {
	
		//Debug.Log ("num: " + num);

		for (int i = 0; i < images.Length; i++) {

			if (num > i) {

				images [i].sprite = goldStar;
			}
			else {

				images [i].sprite = whiteStar;	
			}
		}
	}
}
