using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class starsMenu : MonoBehaviour {

	public static Image[] images;
	public static Sprite whiteStar;
	public static Sprite goldStar;
	public static int songIndex = 0;

	void Start () {

		whiteStar = Resources.Load ("starwhite", typeof(Sprite)) as Sprite;
		goldStar = Resources.Load ("stargold", typeof(Sprite)) as Sprite;
		images = GetComponentsInChildren<Image> ();
		songIndex = int.Parse (gameObject.name);
		Refresh ();
	}

	public static void Refresh() {
	
		for (int i = 0; i < images.Length; i++) {

			if (playerStats.songStars [songIndex] > i) {

				images [i].sprite = goldStar;
			}
			else {

				images [i].sprite = whiteStar;	
			}
		}
	}
}
