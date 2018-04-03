using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{
	// Use this for initialization
	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{
		Camera camera = Camera.main;
		Vector3 headPos = camera.transform.position;
		Vector3 gazeDir = camera.transform.forward;
		float z = transform.position.z;

		float delta = (z - headPos.z) / gazeDir.z;

		Vector3 newPos = headPos + delta * gazeDir;
		newPos.z = z;
		transform.position = newPos;

	}
}
