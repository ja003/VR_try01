using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionKey : MonoBehaviour
{
	public bool IsPressed;

	public MeshRenderer Renderer;

	private void Awake()
	{
		Renderer = GetComponent<MeshRenderer>();
	}

	public void SetPressed()
	{
		IsPressed = true;
	}

	public void SetNotPressed()
	{
		IsPressed = false;
	}
}
