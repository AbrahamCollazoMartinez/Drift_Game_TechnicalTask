using UnityEngine;
using System;
using System.Collections;

public class CarBrain : MonoBehaviour
{
	[SerializeField] private Rigidbody sphereRB;
	[SerializeField] private Rigidbody carRB;

	[SerializeField] private float maxFwdSpeed = 200f;
	[SerializeField] private float fwdSpeed;

	[SerializeField] private float fwdAccel;
	[SerializeField] private float stoppingAccel;

	[SerializeField] private float revSpeed;
	[SerializeField] private float turnSpeed;
	[SerializeField] private float driftTurnSpeed;

	[SerializeField] private LayerMask groundLayer;

	[SerializeField] private float modifiedDrag;

	[SerializeField] private float alignToGroundTime;
	[SerializeField] private float timeToResetDrift;
	private float moveInput;
	private float turnInput;
	private bool isCarGrounded;
	private bool isDrifting = false;
	private float normalDrag;
	private bool isDriftBlocked = false;

	public bool is_Drifting
	{
		get { return isDrifting; }
		set
		{
			if (value != isDrifting)
			{
				onDriftStateChange?.Invoke(value);
			}
			isDrifting = value;

		}
	}

	public Action<bool> onDriftStateChange = delegate { };

	void Start()
	{
		// Detach Sphere from car
		sphereRB.transform.parent = null;
		carRB.transform.parent = null;

		normalDrag = sphereRB.drag;
	}

	void Update()
	{
		// Get Input
		moveInput = Input.GetAxisRaw("Vertical");
		turnInput = Input.GetAxisRaw("Horizontal");

		// Check if the drift button (space bar) is pressed
		if (!isDriftBlocked)
		{
			if (Input.GetKey(KeyCode.Space))
			{
				is_Drifting = true;
			}
			else if (!Input.GetKey(KeyCode.Space))
			{
				is_Drifting = false;

			}
		}

		AddAcceleration();

		TurningRotation();

		AligmentAndDirection();
		
	
	}

	private void AligmentAndDirection()
	{
		// Set Cars Position to Our Sphere
		transform.position = sphereRB.transform.position;

		// Raycast to the ground and get normal to align car with it.
		RaycastHit hit;
		isCarGrounded = Physics.Raycast(transform.position, -transform.up, out hit, 1f, groundLayer);

		//Rotate Car to align with ground
		Quaternion toRotateTo = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
		transform.rotation = Quaternion.Slerp(transform.rotation, toRotateTo, alignToGroundTime * Time.deltaTime);

		// Calculate Movement Direction
		moveInput *= moveInput > 0 ? fwdSpeed : revSpeed;

		//Calculate Drag
		sphereRB.drag = isCarGrounded ? normalDrag : modifiedDrag;
	}

	private void TurningRotation()
	{
		// Calculate Turning Rotation
		float currentTurnSpeed = is_Drifting ? driftTurnSpeed : turnSpeed;
		float newRot = turnInput * currentTurnSpeed * Time.deltaTime * moveInput;

		if (isCarGrounded)
			transform.Rotate(0, newRot, 0, Space.World);
	}

	private void AddAcceleration()
	{
		// Added Acceleration to Forward Direction
		if (moveInput > 0)
		{
			if (fwdSpeed < maxFwdSpeed)
			{
				fwdSpeed += Time.deltaTime * fwdAccel;
			}
			else
			{
				fwdSpeed = maxFwdSpeed;
			}
		}
		else
		{
			if (fwdSpeed > 0)
			{
				fwdSpeed -= Time.deltaTime * stoppingAccel;
			}
		}
	}

	private void FixedUpdate()
	{
		if (isCarGrounded)
			sphereRB.AddForce(transform.forward * moveInput, ForceMode.Acceleration);
		else
			sphereRB.AddForce(transform.up * -200f); // Add Gravity

		carRB.MoveRotation(transform.rotation);
	}

	public void CollisionDetected(Collision collision)
	{
		if (is_Drifting && !isDriftBlocked)
			BlockDrift();
	}

	private void BlockDrift()
	{
		isDriftBlocked = true;
		is_Drifting = false;
		StartCoroutine(ResetDrift());
	}

	IEnumerator ResetDrift()
	{
		yield return new WaitForSeconds(timeToResetDrift);
		isDriftBlocked = false;
	}
}
