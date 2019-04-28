using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelmWheel : MonoBehaviour
{
	public float turnSpeed;
	
	[Space]
	public float zRotLeft;
	
	
	
	
	public void Turn(int dir)
	{
		//StopAllCoroutines();
		StartCoroutine(TurnCoroutine(dir));
	}
	IEnumerator TurnCoroutine(int dir)
	{
		Vector3 rot = transform.localEulerAngles;
		
		float startZRot = rot.z;
		float desiredZRot = startZRot + zRotLeft * dir;
		
		for(float t=0; t<=1; t+=Time.deltaTime * turnSpeed)
		{
			rot.z = Mathf.Lerp(startZRot, desiredZRot, t);
			transform.localEulerAngles = rot;
			
			yield return null;
		}
		
		rot.z = desiredZRot;
		transform.localEulerAngles = rot;
	}
}
