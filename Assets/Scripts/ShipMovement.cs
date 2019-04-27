using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovement : MonoBehaviour
{
	
	public float defaultForwardSpeed = 1f;
	public float slowDownSpeed = 1f;
	
	bool slowdown = false;
	float forwardSpeed;
	
	Rigidbody rb;
	
	
	
	void Awake() {
		rb = GetComponent<Rigidbody>();
		forwardSpeed = defaultForwardSpeed;
	}
	
	void FixedUpdate()
	{
		rb.MovePosition(rb.position + (transform.forward * forwardSpeed * Time.fixedDeltaTime));
	}
	
	public void EndJourneyByCollision()
	{
		if (slowdown) return;
		
		StartCoroutine(SlowDownCoroutine());
		
		slowdown = true;
	}
	IEnumerator SlowDownCoroutine()
	{
		for(float t = 0; t <= 1; t+=slowDownSpeed * Time.deltaTime)
		{
			forwardSpeed = Mathf.Lerp(defaultForwardSpeed, 0f, t);
			yield return null;
		}
		forwardSpeed = 0f;
	}
}
