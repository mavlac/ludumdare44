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
	public Color dotNavColor;
	
	
	Color dotDefaultColor;
	
	void Awake()
	{
		dotDefaultColor = centerDot.color;
	}
	
	
	
	public void SetHintToScope()
	{
		centerDot.color = dotHintColor;
		hintScope.SetActive(!gameManager.playerUsedScopeAction);
		hintNav.SetActive(false);
		hintNavWait.SetActive(false);
	}
	public void SetHintToNav()
	{
		centerDot.color = dotNavColor;
		hintScope.SetActive(false);
		hintNav.SetActive(!gameManager.playerUsedNavAction);
		hintNavWait.SetActive(false);
	}
	public void SetHintToWait()
	{
		centerDot.color = dotNavColor;
		hintScope.SetActive(false);
		hintNav.SetActive(false);
		hintNavWait.SetActive(true);
	}
	public void NoHint()
	{
		centerDot.color = dotDefaultColor;
		hintScope.SetActive(false);
		hintNav.SetActive(false);
		hintNavWait.SetActive(false);
	}
	public void HideEverything()
	{
		centerDot.color = new Color(0,0,0,0); // transparent
		hintScope.SetActive(false);
		hintNav.SetActive(false);
		hintNavWait.SetActive(false);
	}
}
