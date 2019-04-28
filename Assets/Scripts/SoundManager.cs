using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class soundGroup
{
	public AudioClip uiClick;
	
	public AudioClip scopeIn;
	public AudioClip scopeOut;
	
	public AudioClip navSelect;
	public AudioClip navOption;
	public AudioClip noAction;

	public AudioClip crash;
}

public class SoundManager : MonoBehaviour {

	public enum soundId : int
	{
		uiClick,
		
		scopeIn,
		scopeOut,

		navSelect,
		navOption,
		noAction,
		
		crash
	};

	AudioSource audioSrc; // global audio source

	// sound file references
	[Header("Assets")]
	public soundGroup gameSounds;


	void Awake()
	{
		audioSrc = gameObject.GetComponent<AudioSource>();
	}


	public void Play(soundId id, float volume = 1)
	{
		switch (id)
		{
			// app
			case soundId.uiClick:
				audioSrc.PlayOneShot(gameSounds.uiClick, volume);
				break;

			// gameplay
			case soundId.scopeIn:
				audioSrc.PlayOneShot(gameSounds.scopeIn, volume);
				break;
			case soundId.scopeOut:
				audioSrc.PlayOneShot(gameSounds.scopeOut, volume);
				break;
			case soundId.navSelect:
				audioSrc.PlayOneShot(gameSounds.navSelect, volume);
				break;
			case soundId.navOption:
				audioSrc.PlayOneShot(gameSounds.navOption, volume);
				break;
			case soundId.noAction:
				audioSrc.PlayOneShot(gameSounds.noAction, volume);
				break;

			case soundId.crash:
				audioSrc.PlayOneShot(gameSounds.crash, volume);
				break;
		}
	}

}
