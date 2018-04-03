using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardCell : MonoBehaviour
{

	TextMesh m_text;

	// Use this for initialization
	void Start()
	{
		m_text = GetComponentInChildren<TextMesh>();
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void OnTap()
	{
		m_text.text = "X";
	}
}
