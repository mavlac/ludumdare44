using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFader : MonoBehaviour {
	
	public enum FadeOutCallback {
		None,
		RestartScene,
		QuitApp
	}
	
	public AppManager appManager;
	
	[Space]
	public float lerpSpeed;

	public Color32 defaultColor;
	
	const float transparent = 0f;
	const float opaque = 1f;


	Color c;

	Image image;


	void Awake ()
	{
		image = GetComponent<Image>();
		image.enabled = false;
	}



	public void FadeScreenOut(FadeOutCallback callback) {
		FadeScreenOut(defaultColor, callback);
	}
	public void FadeScreenOut(Color fadeTo, FadeOutCallback callback)
	{
		Reset();
		image.color = fadeTo;
		image.enabled = true;
		
		StartCoroutine(
			FadeScreenOutCoroutine(lerpSpeed, callback));
	}
	IEnumerator FadeScreenOutCoroutine(float speed, FadeOutCallback callback)
	{
		for (float t = 0f; t <= 1f; t+= Time.unscaledDeltaTime * speed)
		{
			SetAlpha(t);
			yield return null;
		}
		
		SetAlpha(opaque);
		
		FadeScreenOutCallback(callback);
	}
	
	public void FadeScreenOutCallback(FadeOutCallback callback)
	{
		
		switch(callback)
		{
			case FadeOutCallback.RestartScene:
				appManager.UnlockCursor();
				appManager.RestoreTimescale();
				appManager.RestartCurrentScene();
				break;
				
			case FadeOutCallback.QuitApp:
				appManager.UnlockCursor();
				appManager.Exit();
				break;
		}
	}
	
	
	
	
	public void FadeScreenIn(bool delayed = false) {
		FadeScreenIn(defaultColor, delayed);
	}
	public void FadeScreenIn(Color fadeFrom, bool delayed = false)
	{
		Reset();
		SetAlpha(opaque);
		image.color = fadeFrom;
		image.enabled = true;
		
		StartCoroutine(FadeScreenInCoroutine(delayed));
	}
	IEnumerator FadeScreenInCoroutine(bool delayed)
	{
		for (float t = 0f; t <= 1f; t+= Time.unscaledDeltaTime * lerpSpeed)
		{
			SetAlpha(1 - t);
			if (delayed) { yield return new WaitForSecondsRealtime(0.1f); delayed = false; }
			yield return null;
		}
		
		SetAlpha(transparent);
		image.enabled = false;
	}
	
	
	
	
	private void SetAlpha(float alpha)
	{
		c = image.color;
		c.a = Mathf.Clamp(alpha, 0, 1);
		image.color = c;
	}
	
	public void Reset()
	{
		StopAllCoroutines();
		SetAlpha(transparent);
		image.enabled = false;
	}
}