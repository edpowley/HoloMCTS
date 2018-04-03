using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class FocusMaterialChanger : MonoBehaviour
{

	public Material m_focusMaterial, m_unfocusMaterial;
	private MeshRenderer m_renderer;

	// Use this for initialization
	void Start()
	{
		m_renderer = GetComponent<MeshRenderer>();
		m_renderer.sharedMaterial = m_unfocusMaterial;
	}

	public void OnFocus()
	{
		m_renderer.sharedMaterial = m_focusMaterial;
	}

	public void OnUnfocus()
	{
		m_renderer.sharedMaterial = m_unfocusMaterial;
	}
}
