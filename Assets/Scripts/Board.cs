using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
	public BoardCell[] m_cells;
	public MctsNode m_nodePrefab;

	private BoardState m_state = new BoardState();
	private bool m_waitingForPlayerMove;

	public int m_iterations = 50;
	public int m_iterationsPerFrame = 1;

	// Use this for initialization
	void Start()
	{
		for (int i = 0; i < m_cells.Length; i++)
			m_cells[i].m_cellIndex = i;

		m_waitingForPlayerMove = true;
	}

	private void updateBoard()
	{
		for (int i=0; i<m_cells.Length; i++)
		{
			m_cells[i].SetState(m_state.m_cellState[i]);
		}
	}

	public void TryPlayMove(int cellIndex)
	{
		if (m_waitingForPlayerMove)
		{
			if (m_state.playMove(cellIndex))
			{
				updateBoard();
				m_waitingForPlayerMove = false;
				StartCoroutine(doMCTS());
			}
		}
	}

	private MctsNode createNode(BoardState state, MctsNode parent, int incomingMove)
	{
		GameObject gob = Instantiate(m_nodePrefab.gameObject);
		MctsNode node = gob.GetComponent<MctsNode>();
		node.m_unexpandedMoves = new List<int>();
		for (int i = 0; i < 9; i++)
			if (state.m_cellState[i] == 0)
				node.m_unexpandedMoves.Add(i);
		node.m_incomingMove = incomingMove;
		node.m_incomingMovePlayer = 3 - state.m_currentPlayer;
		node.m_parent = parent;
		if (parent != null)
			parent.m_children.Add(node);

		if (parent != null)
			node.m_depth = parent.m_depth + 1;
		else
			node.m_depth = 0;

		Vector3 position;
		if (parent == null)
		{
			position = transform.position + Vector3.back * transform.localScale.z;
		}
		else
		{
			position = parent.transform.position + Vector3.forward * transform.localScale.z;
			int cx = (incomingMove % 3)-1;
			int cy = (incomingMove / 3)-1;
			position.x += cx * transform.localScale.x * Mathf.Pow(0.25f, node.m_depth - 1);
			position.y += cy * transform.localScale.y * Mathf.Pow(0.25f, node.m_depth - 1);
		}

		node.setPosition(position);

		return node;
	}

	private IEnumerator doMCTS()
	{
		MctsNode rootNode = createNode(m_state, null, -1);

		for (int t = 0; t < m_iterations; t++)
		{
			MctsNode currentNode = rootNode;
			BoardState currentState = new BoardState(m_state);

			// Selection
			while (!currentState.IsTerminal && currentNode.m_unexpandedMoves.Count == 0)
			{
				currentNode = currentNode.selectChild();
				currentState.playMove(currentNode.m_incomingMove);
			}

			// Expansion
			if (!currentState.IsTerminal)
			{
				int index = Random.Range(0, currentNode.m_unexpandedMoves.Count);
				int move = currentNode.m_unexpandedMoves[index];
				currentNode.m_unexpandedMoves.RemoveAt(index);

				currentState.playMove(move);
				currentNode = createNode(currentState, currentNode, move);
			}

			// Simulation
			while (!currentState.IsTerminal)
			{
				int index = Random.Range(0, 9);
				currentState.playMove(index);
			}

			// Backpropagation
			for (; currentNode != null; currentNode = currentNode.m_parent)
			{
				currentNode.updateStatistics(currentState.m_terminalReward);
			}

			if (t % m_iterationsPerFrame == 0)
				yield return null;
		}
	}
}

