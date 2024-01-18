using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Extensions;

public class WheelController : MonoBehaviour
{
	[SerializeField] private GameObject[] wheelsToRotate;
	[SerializeField] private GameObject[] wheelsdirection;
	[SerializeField] private TrailRenderer[] trails;
	[SerializeField] private ParticleSystem smoke;

	[SerializeField] private float rotationSpeed;
	[SerializeField] private float rotationSpeedRightLeft;

	[SerializeField] private float minYRot;
	[SerializeField] private float maxYRot;

	private CarBrain accessCharacterController;

	private void Start()
	{
		accessCharacterController = this.gameObject.transform.parent.parent.GetComponent<CarBrain>();
	}


	// Update is called once per frame
	void Update()
	{
		float verticalAxis = Input.GetAxisRaw("Vertical");
		float horizontalAxis = Input.GetAxisRaw("Horizontal");

		foreach (var wheel in wheelsToRotate)
		{
			wheel.transform.Rotate(Time.deltaTime * verticalAxis * rotationSpeed, 0, 0, Space.Self);
		}

		if (horizontalAxis > 0)
		{
			RotateWheels(rotationSpeedRightLeft); // Turning right
			RotateWheels(rotationSpeedRightLeft); // Turning right
		}
		else if (horizontalAxis < 0)
		{
			RotateWheels(-rotationSpeedRightLeft); // Turning left
			RotateWheels(-rotationSpeedRightLeft); // Turning left
		}
		else
		{
			// Must be going straight
			RotateWheels(0f);
		}

		// Adjusting trail and smoke emission based on horizontalAxis
		if (accessCharacterController.is_Drifting)
		{
			foreach (var trail in trails)
			{
				trail.emitting = true;
			}

			var emission = smoke.emission;
			emission.rateOverTime = 50f;
		}
		else
		{
			foreach (var trail in trails)
			{
				trail.emitting = false;
			}

			var emission = smoke.emission;
			emission.rateOverTime = 0f;
		}
	}

	// Function to rotate wheels based on the given rotation speed and rotation angle limits
	void RotateWheels(float speed)
	{
		if (speed == 0)
		{
			wheelsdirection[0].transform.SetLocalEulerAnglesY(0);
			wheelsdirection[1].transform.SetLocalEulerAnglesY(0);
		}
		else
		{
			wheelsdirection[0].transform.Rotate(0, Time.deltaTime * speed, 0, Space.Self);
			wheelsdirection[1].transform.Rotate(0, Time.deltaTime * speed, 0, Space.Self);

			LimitRot(wheelsdirection[0].transform);
			LimitRot(wheelsdirection[1].transform);

		}
	}

	private void LimitRot(Transform limitObjectRotation)
	{

		Vector3 playerEulerAngles = limitObjectRotation.localRotation.eulerAngles;

		playerEulerAngles.y = (playerEulerAngles.y > 180) ? playerEulerAngles.y - 360 : playerEulerAngles.y;
		playerEulerAngles.y = Mathf.Clamp(playerEulerAngles.y, minYRot, maxYRot);

		limitObjectRotation.localRotation = Quaternion.Euler(playerEulerAngles);
	}


	
}
