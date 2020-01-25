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

	bool isAdjustingRotation;

	[SerializeField] ActionKey btnForward;
	[SerializeField] ActionKey btnRight;
	[SerializeField] ActionKey btnBack;
	[SerializeField] ActionKey btnLeft;

	private void Update()
	{
		JoystickInput();

		Move(GetWantedMove());

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

	//ActionKey hitBtnObject;

	bool joystickPressed;

	[SerializeField]
	GvrReticlePointer pointer;

	[SerializeField]
	FlashLight flashLight;

	private void JoystickInput()
	{
		if(IsActionKeyPressed(EActionKey.Click))
		{
			if(!joystickPressed)
			{
				ActionKey hitBtnObject = pointer.LastEnteredGO.GetComponent<ActionKey>();

				if(hitBtnObject != null)
				{
					//Debug.Log(hitBtnObject.name);
					btnForward.SetNotPressed();
					btnRight.SetNotPressed();
					btnBack.SetNotPressed();
					btnLeft.SetNotPressed();

					hitBtnObject.SetPressed();
					joystickPressed = true;
				}
			}
		}
		else
		{
			joystickPressed = false;

			btnForward.SetNotPressed();
			btnRight.SetNotPressed();
			btnBack.SetNotPressed();
			btnLeft.SetNotPressed();

			//Debug.Log("up");
		}

		if(IsActionKeyPressed(EActionKey.Interact))
		{
			const int light_intensity = 20;
			const int light_angle = 100;
			const int boost_duration = 1;
			flashLight.SetLight(light_intensity, light_angle, boost_duration);
		}
	}

	private EDirection GetWantedMove()
	{
		if(IsActionKeyPressed(EActionKey.MoveForward))
			return EDirection.Forward;

		if(IsActionKeyPressed(EActionKey.MoveRight))
			return EDirection.Right;

		if(IsActionKeyPressed(EActionKey.MoveBack))
			return EDirection.Back;

		if(IsActionKeyPressed(EActionKey.MoveLeft))
			return EDirection.Left;

		return EDirection.None;
	}

	private bool IsActionKeyPressed(EActionKey pKey)
	{
		switch(pKey)
		{
			case EActionKey.None:
				break;
			case EActionKey.MoveForward:
				return Input.GetKey(KeyCode.JoystickButton2) ||
					Input.GetKey(KeyCode.W) ||
					btnForward.IsPressed;
			case EActionKey.MoveRight:
				return Input.GetKey(KeyCode.JoystickButton3) ||
					Input.GetKey(KeyCode.D) ||
					btnRight.IsPressed;
			case EActionKey.MoveBack:
				return Input.GetKey(KeyCode.JoystickButton1) ||
					Input.GetKey(KeyCode.S) ||
					btnBack.IsPressed;
			case EActionKey.MoveLeft:
				return Input.GetKey(KeyCode.JoystickButton0) ||
					Input.GetKey(KeyCode.A) ||
					btnLeft.IsPressed;

			case EActionKey.Click:
				return Input.GetKey(KeyCode.JoystickButton4) ||
					Input.GetKey(KeyCode.Q);

			case EActionKey.Interact:
				return Input.GetKey(KeyCode.JoystickButton5) ||
					Input.GetKey(KeyCode.R);

		}

		return false;
	}

	public void UpdateVisibility()
	{
		bool isCameraLookingDown =
			camera.transform.localRotation.eulerAngles.x > cameraLookDowsnMin &&
			camera.transform.localRotation.eulerAngles.x < cameraLookDowsnMax;

		float diffMin = Mathf.Abs(camera.transform.localRotation.eulerAngles.x - cameraLookDowsnMin);
		float diffMax = Mathf.Abs(camera.transform.localRotation.eulerAngles.x - cameraLookDowsnMax);
		float diff = Mathf.Min(diffMax, diffMin);

		if(!isCameraLookingDown)
		{
			float diffCoeff = diff / 10;

			btnForward.UpdateColor(diffCoeff);
			btnRight.UpdateColor(diffCoeff);
			btnBack.UpdateColor(diffCoeff);
			btnLeft.UpdateColor(diffCoeff);
		}
	}

	private void Move(EDirection pDir)
	{
		Vector3 dir = Vector3.zero;
		switch(pDir)
		{
			case EDirection.Forward:
				dir = camera.transform.forward;
				break;
			case EDirection.Right:
				dir = camera.transform.right;
				break;
			case EDirection.Back:
				dir = -camera.transform.forward;
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

public enum EActionKey
{
	None,

	MoveForward,
	MoveRight,
	MoveBack,
	MoveLeft,

	Click,
	Interact
}