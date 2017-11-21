using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class button : MonoBehaviour {

	Button gumb;
	string ime;

	void Start () {

		gumb = GetComponent<Button> ();
		ime = gameObject.name;

		//Debug.Log (playerStats.GetStars());

		if (ime == "cuksejeozenil" && playerStats.GetStars() < 1) {
		
			gumb.interactable = false;
		}
		else if (ime == "pluginbaby" && playerStats.GetStars() < 4) {

			gumb.interactable = false;
		}
	}
}
