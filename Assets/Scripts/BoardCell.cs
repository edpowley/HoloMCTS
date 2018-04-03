using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardCell : MonoBehaviour
{
	private Board m_board;
	internal int m_cellIndex = -1;
	private TextMesh m_text;

	// Use this for initialization
	void Start()
	{
		m_board = GetComponentInParent<Board>();
		m_text = GetComponentInChildren<TextMesh>();
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void SetState(int state)
	{
		switch (state)
		{
			case 0: m_text.text = ""; break;
			case 1: m_text.text = "O"; break;
			case 2: m_text.text = "X"; break;
			default: m_text.text = "?"; break;
		}
	}

	public void OnTap()
	{
		m_board.TryPlayMove(m_cellIndex);
	}

	// For testing without the HoloLens
	public void OnMouseDown()
	{
		OnTap();
	}
}
