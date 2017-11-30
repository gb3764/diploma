using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour {

	float speed = -20f;
	float length = 2000f;

	void Update () {

		if (Input.GetMouseButton (0)) {
		
			transform.position += new Vector3(Input.GetAxisRaw("Mouse X") * Time.deltaTime * speed, 0f, 0f);

			transform.position = new Vector3(Mathf.Clamp(transform.position.x, 0f, length), transform.position.y, transform.position.z);
		}
	}
}
