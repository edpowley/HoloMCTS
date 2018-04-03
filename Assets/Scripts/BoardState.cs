using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class BoardState
{
	public int[] m_cellState = new int[9];
	public int m_currentPlayer = 1;
	public bool IsTerminal = false;
	public float m_terminalReward = 0; // from player 1's POV

	public BoardState()
	{

	}

	public BoardState(BoardState other)
	{
		for (int i = 0; i < 9; i++)
			m_cellState[i] = other.m_cellState[i];

		m_currentPlayer = other.m_currentPlayer;
	}

	public bool playMove(int cellIndex)
	{
		if (m_cellState[cellIndex] == 0)
		{
			m_cellState[cellIndex] = m_currentPlayer;
			m_currentPlayer = 3 - m_currentPlayer;

			checkTerminal();

			return true;
		}
		else
		{
			return false;
		}
	}

	private void checkTerminal()
	{
		IsTerminal = false;

		for (int i = 0; i < 3; i++)
		{
			checkLine(i, 3);
			checkLine(3 * i, 1);
		}

		checkLine(0, 4);
		checkLine(2, 2);

		// Check for board full with no line
		if (!IsTerminal && !m_cellState.Contains(0))
		{
			m_terminalReward = 0.5f;
			IsTerminal = true;
		}
	}

	private void checkLine(int first, int stride)
	{
		int p = m_cellState[first];
		if (p != 0 && m_cellState[first+stride]==p && m_cellState[first+2*stride]==p)
		{
			m_terminalReward = (p == 1) ? 1 : 0;
			IsTerminal = true;
		}
	}
}
