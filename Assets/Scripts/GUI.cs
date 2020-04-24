using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI : MonoBehaviour
{
	public AppManager appManager;
	
	[Space]
	public GameObject titleScreen;
	public RectTransform titleTransform;
	public Text titleText;
	
	[Space]
	public GameObject menu;
	public Image menuOverlay;
	float menuOverlayDefaultAlpha;
	
	[Space]
	public Text newGameButtonLabel;
	public Text quitGameButtonLabel;
	public Toggle invertMouse;
	
	
	
	void Awake()
	{
		appManager.gameManager.playerLook.GetInvert();
		invertMouse.isOn = appManager.gameManager.playerLook.invertVertical;
		
		menuOverlayDefaultAlpha = menuOverlay.color.a;
	}
	
	void Update()
	{
		// only menu mechanics
		if (appManager.appState != AppManager.AppState.Menu) return;
		
		// escape
		if (Input.GetButtonDown("Cancel"))
		{
			//NewGameButtonDown();
		}
	}
	
	
	
	
	
	public void ShowMenu()
	{
		menu.SetActive(true);
		
		RestoreMenuOverlay();
		
		// called from game, not from scene begin (that is default menu labels - title screen)
		newGameButtonLabel.text = "CONTINUE";
		quitGameButtonLabel.text = "BACK TO TITLE";
	}
	
	
	
	
	public void NewGameButtonDown()
	{
		menu.SetActive(false);
		FadeOutMenuOverlay();
		SlideOutTitle();
		
		appManager.BeginGame();
		
		appManager.soundManager.Play(SoundManager.soundId.uiClick);
	}
	public void RestoreMenuOverlay()
	{
		menuOverlay.enabled = true;
		Color c = menuOverlay.color;
		c.a = menuOverlayDefaultAlpha;
		menuOverlay.color = c;
	}
	
	public void FadeOutMenuOverlay()
	{
		StartCoroutine(FadeOutMenuOverlayCoroutine());
	}
	IEnumerator FadeOutMenuOverlayCoroutine()
	{
		Color c = menuOverlay.color;
		
		for (float t=0; t<=1; t+=Time.unscaledDeltaTime*2f)
		{
			c.a = Mathf.Lerp(menuOverlayDefaultAlpha, 0, t);
			menuOverlay.color = c;
			yield return null;
		}
		menuOverlay.enabled = false;
	}
	
	void SlideOutTitle()
	{
		if (!titleScreen.activeSelf) return;
		
		StartCoroutine(SlideOutTitleCoroutine());
	}
	IEnumerator SlideOutTitleCoroutine()
	{
		Color transparentWhite = new Color(255, 255, 255, 0);
		
		for (float t=0; t<=1; t+=Time.unscaledDeltaTime*0.75f)
		{
			titleTransform.localScale = 
				Vector3.Lerp(Vector3.one, Vector3.one * 1.2f, t);
			
			titleText.color = 
				Color.Lerp(Color.white, transparentWhite, t);
			
			yield return null;
		}
		
		titleScreen.SetActive(false);
	}
	
	
	
	
	public void InvertMouseValChanged(bool newValue) 
	{
		appManager.gameManager.playerLook.SetInvert(newValue);
		
		appManager.soundManager.Play(SoundManager.soundId.uiClick);
	}
	
	
	public void QuitButtonDown()
	{
		menu.SetActive(false);
		
		appManager.soundManager.Play(SoundManager.soundId.uiClick);
		
		switch(appManager.appState)
		{
			case AppManager.AppState.Title:
				//appManager.Exit();
				appManager.screenFader.FadeScreenOut(ScreenFader.FadeOutCallback.QuitApp);
				break;
			
			case AppManager.AppState.Menu:
				appManager.screenFader.FadeScreenOut(ScreenFader.FadeOutCallback.RestartScene);
				break;
		}
	}
}
