using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnPoint : MonoBehaviour {

	public static spawnPoint Instance;

	void Start () {

		Instance = this;
	}

	public static Vector3 getPosition() {

		return Instance.gameObject.transform.position;
	}
}
