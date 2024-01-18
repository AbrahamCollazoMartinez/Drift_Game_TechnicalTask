using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionInvoker : MonoBehaviour
{
	public UnityEvent<Collider> onTriggerEnterEvent;
	public UnityEvent<Collision> onCollisionEnterEvent;

	private void OnTriggerEnter(Collider other)
	{
		onTriggerEnterEvent?.Invoke(other);
	}

	private void OnCollisionEnter(Collision other)
	{
		onCollisionEnterEvent?.Invoke(other);
	}
}
