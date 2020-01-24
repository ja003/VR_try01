using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
	[SerializeField]
	Player player;

	[SerializeField]
	float moveSpeed = 1;
	[SerializeField]
	float rotateSpeed = 1;
	[SerializeField]
	float maxRotateYDiff = 10;


	[SerializeField]
	float cameraLookDowsnMin;
	[SerializeField]
	float cameraLookDowsnMax;


	[SerializeField]
	Transform origin;

	[SerializeField]
	Camera camera;

	Direction direction = Direction.None;

	bool isAdjustingRotation;


	[SerializeField] MeshRenderer btnForward;
	[SerializeField] MeshRenderer btnRight;
	[SerializeField] MeshRenderer btnBack;
	[SerializeField] MeshRenderer btnLeft;

	private void FixedUpdate()
	{
		Move(direction);

		float rotateYDiff = Mathf.Abs(
			origin.transform.localRotation.eulerAngles.y -
			camera.transform.rotation.eulerAngles.y);

		if(rotateYDiff > maxRotateYDiff)
			isAdjustingRotation = true;
		else if(rotateYDiff < 1)
			isAdjustingRotation = false;


		if(isAdjustingRotation)
		{
			origin.transform.localRotation = Quaternion.RotateTowards(
				  origin.transform.localRotation,
				  Quaternion.Euler(0, camera.transform.rotation.eulerAngles.y, 0),
				  rotateSpeed * Time.deltaTime);
		}



	}
	//bool isCameraLookingDown;

	public void UpdateVisibility()
	{
		bool isCameraLookingDown =
			camera.transform.localRotation.eulerAngles.x > cameraLookDowsnMin &&
			camera.transform.localRotation.eulerAngles.x < cameraLookDowsnMax;

		//gameObject.SetActive(isCameraLookingDown);

		float diffMin = Mathf.Abs(camera.transform.localRotation.eulerAngles.x - cameraLookDowsnMin);
		float diffMax = Mathf.Abs(camera.transform.localRotation.eulerAngles.x - cameraLookDowsnMax);
		float diff = Mathf.Min(diffMax, diffMin);

		if(!isCameraLookingDown)
		{
			Color c = btnForward.material.color;
			float diffCoeff = diff / 10;
			btnForward.material.color = new Color(c.r, c.g, c.b, 1 - diffCoeff);
			btnRight.material.color = new Color(c.r, c.g, c.b, 1 - diffCoeff);
			btnBack.material.color = new Color(c.r, c.g, c.b, 1 - diffCoeff);
			btnLeft.material.color = new Color(c.r, c.g, c.b, 1 - diffCoeff);
		}
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
				dir = camera.transform.right;
				break;
			case Direction.Back:
				dir = -camera.transform.forward;
				//dir = Vector3.back;
				break;
			case Direction.Left:
				dir = -camera.transform.right;
				break;
		}
		dir.y = 0;

		player.transform.position += dir * Time.deltaTime * moveSpeed;
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