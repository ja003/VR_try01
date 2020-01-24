using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
	[SerializeField]
	Player player;

	[SerializeField]
	float speed = 1;

	[SerializeField]
	Transform origin;

	[SerializeField]
	Camera camera;

	Direction direction = Direction.None;

	private void Update()
	{
		Move(direction);
		origin.transform.localRotation = 
			Quaternion.Euler(0, camera.transform.rotation.eulerAngles.y, 0);
		//origin.Rotate(Vector3.up, camera.transform.rotation.eulerAngles.y);
	}

	public void MoveForward()
	{
		direction = Direction.Forward;
	}
	public void MoveRight()
	{
		direction = Direction.Right;
	}
	public void MoveBack()
	{
		direction = Direction.Back;
	}
	public void MoveLeft()
	{
		direction = Direction.Left;
	}

	public void StopMove()
	{
		direction = Direction.None;
	}

	private void Move(Direction pDir)
	{
		Vector3 dir = Vector3.zero;
		switch(pDir)
		{
			case Direction.Forward:
				//dir = Vector3.forward;
				dir = camera.transform.forward;
				break;
			case Direction.Right:
				dir = Vector3.right;
				break;
			case Direction.Back:
				dir = -camera.transform.forward;
				//dir = Vector3.back;
				break;
			case Direction.Left:
				dir = Vector3.left;
				break;
		}
		dir.y = 0;

		player.transform.position += dir * Time.deltaTime * speed;
	}

	
}

public enum Direction
{
	None,
	Forward,
	Right,
	Back,
	Left
}