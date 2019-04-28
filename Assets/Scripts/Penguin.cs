using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Penguin : MonoBehaviour
{
	public GameManager gameManager;
	
	[Space]
	public Transform penguinBody;
	
	[Space]
	public Transform leftWing;
	public Transform rightWing;
	public float waveZRotAmount = 130f;
	public float waveSpeed;
	
	void Start()
	{
	}
	
	
	void Update()
	{
		Vector3 penguinLocalRot;
		penguinLocalRot = penguinBody.localEulerAngles;
		
		penguinLocalRot.y = (180f + gameManager.playerLook.transform.localEulerAngles.y) * -1f;
		penguinLocalRot.y = Mathf.Clamp(penguinLocalRot.y, -360f - 25f, -360f + 25f);
		
		penguinBody.localEulerAngles = penguinLocalRot;
	}
	
	
	
	
	
	public void WaveWing(int dir)
	{
		switch (dir)
		{
			case -1:
				StartCoroutine(WaveCoroutine(rightWing, -1, true));
				break;
			case 1:
				StartCoroutine(WaveCoroutine(leftWing, 1, true));
				break;
		}
	}
	public void BothWingsUp()
	{
		StartCoroutine(WaveCoroutine(rightWing, -1, false));
		StartCoroutine(WaveCoroutine(leftWing, 1, false));
	}
	IEnumerator WaveCoroutine(Transform wing, int dir, bool back)
	{
		Vector3 rot = wing.localEulerAngles;
		
		float startZRot = rot.z;
		float desiredZRot = startZRot + waveZRotAmount * dir;
		for(float t=0; t<=1; t+=Time.deltaTime * waveSpeed)
		{
			rot.z = Mathf.Lerp(startZRot, desiredZRot, t);
			wing.localEulerAngles = rot;
			yield return null;
		}
		
		rot.z = desiredZRot;
		wing.localEulerAngles = rot;
		
		if (back)
		{
			
			for(float t=0; t<=1; t+=Time.deltaTime * waveSpeed)
			{
				rot.z = Mathf.Lerp(desiredZRot, startZRot, t);
				wing.localEulerAngles = rot;
				yield return null;
			}
			
			rot.z = startZRot;
			wing.localEulerAngles = rot;
		}
	}
}
