using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class AppManager : MonoBehaviour {


	public static bool mouselookReady = false;
	


	// scene management
	
	public void RestartCurrentScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
	
	/*public void LoadMenuScene()
	{
		RestoreTimescale();
		SceneManager.LoadScene("menu");
	}
	public void LoadGameScene()
	{
		RestoreTimescale();
		SceneManager.LoadScene("game");
	}*/




	// mouse cursor management
	
	public void UnlockCursor()
	{
		Cursor.lockState = CursorLockMode.None;
		mouselookReady = false;
	}
	public void LockCursor()
	{
		Cursor.lockState = CursorLockMode.Locked;
		StartCoroutine(DelayedMouseLook());
	}
	IEnumerator DelayedMouseLook() {
		
		do {
			// wait for first mouse movement after locking cursor
			yield return null;
		} while (Input.GetAxis(PlayerLook.mouseXInputName) == 0 && Input.GetAxis(PlayerLook.mouseYInputName) == 0);
		
		// and ignore a moment after it to prevent weird behav
		yield return new WaitForSecondsRealtime(0.1f);
		mouselookReady = true;
	}
	
	
	
	
	// OS
	
	public void Exit()
	{
#if UNITY_EDITOR
		Debug.Log("Unable to exit app in editor");
#endif
#if UNITY_ANDROID && !UNITY_EDITOR
		using (AndroidJavaClass javaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
		AndroidJavaObject unityActivity = javaClass.GetStatic<AndroidJavaObject>("currentActivity");
		unityActivity.Call<bool>("moveTaskToBack", true);
		}
#else
		Application.Quit();
#endif
	}
}