using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardCell : MonoBehaviour
{
	private Board m_board;
	internal int m_cellIndex = -1;
	private TextMesh m_text;

	public Material m_focusMaterial, m_unfocusMaterial;
	private MeshRenderer m_renderer;

	// Use this for initialization
	void Start()
	{
		m_renderer = GetComponent<MeshRenderer>();
		m_renderer.sharedMaterial = m_unfocusMaterial;
		m_board = GetComponentInParent<Board>();
		m_text = GetComponentInChildren<TextMesh>();
	}

	public void OnFocus()
	{
		if (m_board.m_waitingForPlayerMove)
			m_renderer.sharedMaterial = m_focusMaterial;
	}

	public void OnUnfocus()
	{
		m_renderer.sharedMaterial = m_unfocusMaterial;
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
		if (m_board.TryPlayMove(m_cellIndex))
			m_renderer.sharedMaterial = m_unfocusMaterial;
	}

	// For testing without the HoloLens
	public void OnMouseDown()
	{
		OnTap();
	}
}
