// https://docs.microsoft.com/en-us/windows/mixed-reality/holograms-101

using UnityEngine;
using UnityEngine.XR.WSA.Input;

public class GazeGestureManager : MonoBehaviour
{
	public static GazeGestureManager s_instance { get; private set; }

	// Represents the hologram that is currently being gazed at.
	public GameObject m_focusedObject { get; private set; }

	GestureRecognizer m_recognizer;

	// Use this for initialization
	void Awake()
	{
		s_instance = this;

		// Set up a GestureRecognizer to detect Select gestures.
		m_recognizer = new GestureRecognizer();
		m_recognizer.Tapped += (args) =>
		{
			// Send an OnSelect message to the focused object and its ancestors.
			if (m_focusedObject != null)
			{
				m_focusedObject.SendMessageUpwards("OnTap", SendMessageOptions.DontRequireReceiver);
			}
		};
		m_recognizer.StartCapturingGestures();
	}

	// Update is called once per frame
	void Update()
	{
		// Figure out which hologram is focused this frame.
		GameObject oldFocusObject = m_focusedObject;

		// Do a raycast into the world based on the user's
		// head position and orientation.
		var headPosition = Camera.main.transform.position;
		var gazeDirection = Camera.main.transform.forward;

		RaycastHit hitInfo;
		if (Physics.Raycast(headPosition, gazeDirection, out hitInfo))
		{
			// If the raycast hit a hologram, use that as the focused object.
			m_focusedObject = hitInfo.collider.gameObject;
		}
		else
		{
			// If the raycast did not hit a hologram, clear the focused object.
			m_focusedObject = null;
		}

		// If the focused object changed this frame,
		// start detecting fresh gestures again.
		if (m_focusedObject != oldFocusObject)
		{
			Debug.LogFormat("Focus: {0} -> {1}", oldFocusObject, m_focusedObject);

			if (oldFocusObject != null)
				oldFocusObject.SendMessageUpwards("OnUnfocus", SendMessageOptions.DontRequireReceiver);

			if (m_focusedObject != null)
				m_focusedObject.SendMessageUpwards("OnFocus", SendMessageOptions.DontRequireReceiver);

			m_recognizer.CancelGestures();
			m_recognizer.StartCapturingGestures();
		}
	}
}
