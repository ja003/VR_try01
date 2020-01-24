using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionKey : MonoBehaviour
{
	public bool IsPressed;

	private Color color;
	private MeshRenderer renderer;

	private void Awake()
	{
		renderer = GetComponent<MeshRenderer>();
		color = renderer.material.color;
	}

	public void SetPressed()
	{
		IsPressed = true;
		Debug.Log(gameObject.name + " pressed");
	}

	public void SetNotPressed()
	{
		IsPressed = false;
		Debug.Log(gameObject.name + " not-pressed");
	}

	internal void UpdateColor(float pCameraCloseToButtonCoefficient)
	{
		renderer.material.color = new Color(color.r, color.g, color.b, 
			1 - pCameraCloseToButtonCoefficient);
	}
}
