using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MctsNode : MonoBehaviour
{

	internal List<int> m_unexpandedMoves;
	internal int m_incomingMove;
	internal int m_incomingMovePlayer;
	internal MctsNode m_parent = null;
	internal List<MctsNode> m_children = new List<MctsNode>();
	internal int m_depth = 0;

	private float m_totalReward = 0;
	private int m_visits = 0;

	private LineRenderer m_lineRenderer_cached = null;
	private LineRenderer m_lineRenderer
	{
		get
		{
			if (m_lineRenderer_cached == null)
				m_lineRenderer_cached = GetComponent<LineRenderer>();
			return m_lineRenderer_cached;
		}
	}

	// Use this for initialization
	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{

	}

	private float calculateUcb()
	{
		return m_totalReward / m_visits + 0.7f * Mathf.Sqrt(Mathf.Log(m_parent.m_visits) / m_visits);
	}

	public MctsNode selectChild()
	{
		float bestScore = 0;
		MctsNode bestChild = null;

		foreach(MctsNode child in m_children)
		{
			float score = child.calculateUcb() + Random.Range(0, 1.0e-5f);
			if (bestChild == null || score > bestScore)
			{
				bestChild = child;
				bestScore = score;
			}
		}

		return bestChild;
	}

	public void updateStatistics(float reward)
	{
		if (m_incomingMovePlayer != 1)
			reward = 1 - reward;

		m_totalReward += reward;
		m_visits++;
	}

	public void setPosition(Vector3 position)
	{
		transform.position = position;

		if (m_parent != null)
		{
			Vector3[] positions = new Vector3[] { m_parent.transform.position - position, Vector3.zero };
			m_lineRenderer.positionCount = 2;
			m_lineRenderer.SetPositions(positions);
		}
	}
}

