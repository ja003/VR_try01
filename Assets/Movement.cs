using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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

	EDirection btnDirection = EDirection.None;

	bool isAdjustingRotation;


	[SerializeField] MeshRenderer btnForward;
	[SerializeField] MeshRenderer btnRight;
	[SerializeField] MeshRenderer btnBack;
	[SerializeField] MeshRenderer btnLeft;

	private void Update()
	{
		Move(GetWantedMove());

		//if(btnDirection == EDirection.None)
		JoystickInput();

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

	GameObject hitBtnObject;


	private void JoystickInput()
	{
		if(Input.GetKey(KeyCode.JoystickButton4) ||
			Input.GetKey(KeyCode.Q))
		{
			RaycastHit hit;
			Physics.Raycast(new Ray(camera.transform.position, camera.transform.forward),
				out hit);


			if(hit.transform != null)
			{
				hitBtnObject = hit.transform.gameObject;
				ExecuteEvents.Execute(hitBtnObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerDownHandler);
				Debug.Log("pointerDownHandler " + hitBtnObject.name);

			}
		}

		else if(hitBtnObject != null && (
			Input.GetKeyUp(KeyCode.JoystickButton4) ||
			Input.GetKeyUp(KeyCode.Q)))
		{
			ExecuteEvents.Execute(hitBtnObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerUpHandler);
			Debug.Log("pointerUpHandler " + hitBtnObject.name);
		}

		//if(Input.GetKey(KeyCode.JoystickButton5))
		//	...();
		//if(Input.GetKey(KeyCode.JoystickButton4))
		//	...();
	}

	private EDirection GetWantedMove()
	{
		if(Input.GetKey(KeyCode.JoystickButton2) ||
					Input.GetKey(KeyCode.W) ||
					btnDirection == EDirection.Forward)
			return EDirection.Forward;

		if(Input.GetKey(KeyCode.JoystickButton3) ||
					Input.GetKey(KeyCode.D) ||
					btnDirection == EDirection.Right)
			return EDirection.Right;

		if(Input.GetKey(KeyCode.JoystickButton1) ||
					Input.GetKey(KeyCode.S) ||
					btnDirection == EDirection.Back)
			return EDirection.Back;

		if(Input.GetKey(KeyCode.JoystickButton0) ||
				Input.GetKey(KeyCode.A) ||
				btnDirection == EDirection.Left)
			return EDirection.Left;

		return EDirection.None;
	}

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

	//bool isMoveSetForward;

	public void MoveForward()
	{
		//isMoveSetForward = true;
		btnDirection = EDirection.Forward;
	}
	public void MoveRight()
	{
		btnDirection = EDirection.Right;
	}
	public void MoveBack()
	{
		btnDirection = EDirection.Back;
	}
	public void MoveLeft()
	{
		btnDirection = EDirection.Left;
	}

	public void StopMove()
	{
		btnDirection = EDirection.None;
	}

	private void Move(EDirection pDir)
	{
		Vector3 dir = Vector3.zero;
		switch(pDir)
		{
			case EDirection.Forward:
				//dir = Vector3.forward;
				dir = camera.transform.forward;
				break;
			case EDirection.Right:
				dir = camera.transform.right;
				break;
			case EDirection.Back:
				dir = -camera.transform.forward;
				//dir = Vector3.back;
				break;
			case EDirection.Left:
				dir = -camera.transform.right;
				break;
		}
		dir.y = 0;

		player.transform.position += dir * Time.deltaTime * moveSpeed;
	}


}

public enum EDirection
{
	None,
	Forward,
	Right,
	Back,
	Left
}