using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

	public AudioSource efxSource;
	public AudioSource musicSource;
	public static SoundManager instance = null;

	// Use this for initialization
	void Awake () {
		if (instance == null) {
			instance = this;
		}
		else if (instance != this) {
			Destroy (gameObject);
		}
		
		DontDestroyOnLoad (gameObject);
	}

	public void PlayMusic (AudioClip clip) {
		musicSource.volume = 1;
		musicSource.clip = clip;
		musicSource.Play();
	}

	public void PlaySingle (AudioClip clip) {
		efxSource.clip = clip;
		efxSource.Play();
	}
}
