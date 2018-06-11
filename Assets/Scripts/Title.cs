using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour {
	
	public GameObject popup;
	public AudioClip buttonSE;
	private AudioSource audioSource;

	// Use this for initialization
	void Start () {
		popup.SetActive (false);
		audioSource = this.gameObject.GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void OnClick_howPrime () {
		// how to "prime"
		if (popup.activeSelf) return;
		audioSource.PlayOneShot (buttonSE);
		popup.SetActive (true);
	}

	public void OnClick_bg () {
		// start game.
		audioSource.PlayOneShot (buttonSE);
		Debug.Log ("StartGame.");
		SceneManager.LoadScene ("GameScene");
	}

	public void OnClick_panel () {
		popup.SetActive (false);
	}
}
