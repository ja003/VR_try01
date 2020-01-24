using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	[SerializeField]
	Movement movement;

	[SerializeField]
	Camera camera;

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		movement.UpdateVisibility();
		Debug.Log("rotation = " + camera.transform.rotation.eulerAngles);
		Debug.Log("localEulerAngles = " + camera.transform.localEulerAngles);
	}
}
