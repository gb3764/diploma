using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endSound : MonoBehaviour {

	public static AudioSource posnetek;

	void Start () {

		posnetek = GetComponent<AudioSource>();
	}

	public static void Play(float delay) {
	
		posnetek.PlayDelayed (delay);
	}
}