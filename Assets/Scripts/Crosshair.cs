using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Crosshair : MonoBehaviour
{
	public GameManager gameManager;
	
	[Space]
	public Image centerDot;
	public GameObject hintScope;
	public Color dotHintColor;
	public GameObject hintNav;
	public GameObject hintNavWait;
	public GameObject hintNavOptions;
	public Image hintNavOptionLeft;
	public Image hintNavOptionRight;
	public Sprite arrow, arrowFocus;
	public Color dotNavColor;
	public GameObject hintGameOver;
	
	
	Color dotDefaultColor;
	
	void Awake()
	{
		dotDefaultColor = centerDot.color;
		HideEverything();
	}
	
	
	
	public void SetHintToScope()
	{
		centerDot.color = dotHintColor;
		hintScope.SetActive(!gameManager.playerUsedScopeAction);
		hintNav.SetActive(false);
		hintNavWait.SetActive(false);
		hintNavOptions.SetActive(false);
	}
	public void SetHintToNav()
	{
		centerDot.color = dotNavColor;
		hintScope.SetActive(false);
		hintNav.SetActive(!gameManager.playerUsedNavAction);
		hintNavWait.SetActive(false);
		hintNavOptions.SetActive(false);
	}
	public void SetHintToWait()
	{
		centerDot.color = dotNavColor;
		hintScope.SetActive(false);
		hintNav.SetActive(false);
		hintNavWait.SetActive(true);
		hintNavOptions.SetActive(false);
	}
	public void SetHintToNavOptions(int dir)
	{
		centerDot.color = dotNavColor;
		hintScope.SetActive(false);
		hintNav.SetActive(false);
		hintNavWait.SetActive(false);
		hintNavOptions.SetActive(true);
		switch(dir)
		{
			case -1:
				hintNavOptionLeft.sprite = arrowFocus;
				hintNavOptionRight.sprite = arrow;
				break;
			case 1:
				hintNavOptionLeft.sprite = arrow;
				hintNavOptionRight.sprite = arrowFocus;
				break;
			case 0:
				hintNavOptionLeft.sprite = arrow;
				hintNavOptionRight.sprite = arrow;
				break;
		}
	}
	public void NoHint()
	{
		centerDot.color = dotDefaultColor;
		hintScope.SetActive(false);
		hintNav.SetActive(false);
		hintNavWait.SetActive(false);
		hintNavOptions.SetActive(false);
	}
	public void HideEverything()
	{
		centerDot.color = new Color(0,0,0,0); // transparent
		hintScope.SetActive(false);
		hintNav.SetActive(false);
		hintNavWait.SetActive(false);
		hintNavOptions.SetActive(false);
		hintGameOver.SetActive(false);
	}
	
	
	public void SetHintToGameOver()
	{
		HideEverything();
		hintGameOver.SetActive(true);
	}
}
