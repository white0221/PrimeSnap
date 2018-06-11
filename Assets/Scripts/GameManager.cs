using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Linq;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	// 定数定義
	private const int MAX_CIRCLE = 10;	// circle の同時最大発生数
	private const int MAX_LIFE = 5;		// life の最大値
	private const int RESPAWN_TIME = 5;	// circle が発生する秒数(仮)
	public int[] primeNumber = new int[] {2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97};
	
	// オブジェクト参照
	public GameObject circle;
	public GameObject canvasGame;
	public GameObject circleText;
	public GameObject life;
	public GameObject dislife;
	public GameObject menu;
	public GameObject good;
	public GameObject bad;
	public GameObject mi;
	public GameObject gameover;
	public GameObject backTitle;
	public GameObject scoreText;

	// Audio
	public AudioClip goodSE;
	public AudioClip badSE;
	public AudioClip buttonSE;

	// メンバ変数
	private int currentCircle = 0;
	private DateTime lastDateTime;			// 最後にcircleが発生した時間
	private int currentLife;
	private int damageLife = 0;
	private DateTime miChangeTime;			// miが変化した時の時間
	private bool gamePlay = true;
	private int score;
	private AudioSource audioSource;
	private List<int> getPrimeList = new List<int>(); 

	// Use this for initialization
	void Start () {
		gameover.SetActive (false);
		menu.SetActive (false);
		good.SetActive (false);
		bad.SetActive (false);
		backTitle.SetActive (false);
		Debug.Log (menu.activeSelf);
		score = 0;
		currentLife = MAX_LIFE;
		audioSource = this.gameObject.GetComponent<AudioSource> ();
		// 現在のライフ表示と、ライフが違っていればライフを更新
		for (int i = 0; i < currentLife; i++) {
			GameObject g_life = (GameObject)Instantiate (life);
			g_life.transform.SetParent (canvasGame.transform, false);
			int x = -300 + (150 * i);
			g_life.transform.localPosition = new Vector3 (x, -570.0f, 0f);
		}
		CreateCircle (UnityEngine.Random.Range(2, 99));
		lastDateTime = DateTime.UtcNow;
		updateScoretext ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!gamePlay) return;
		if (menuCheck()) return;
		if (!mi.activeSelf) {
			// mi == false;
			TimeSpan time = DateTime.UtcNow - miChangeTime;
			if (time >= TimeSpan.FromSeconds(1.0f)) {
				// timeが1秒を超えていたら
				good.SetActive (false);
				bad.SetActive (false);
				mi.SetActive (true);
			}
		}
		if (currentCircle < MAX_CIRCLE) {
			TimeSpan timeSpan = DateTime.UtcNow - lastDateTime;
			if (timeSpan >= TimeSpan.FromSeconds(RESPAWN_TIME)) {
				while (timeSpan >= TimeSpan.FromSeconds(RESPAWN_TIME)) {
					CreateNewCircle ();
					timeSpan -= TimeSpan.FromSeconds(RESPAWN_TIME);
				}
			}
		}
	}

	public bool menuCheck () {
		return menu.activeSelf;
	}
	public bool gameCheck () {
		return gamePlay;
	}

	public void mi_good () {
		audioSource.PlayOneShot (goodSE);
		bad.SetActive (false);
		mi.SetActive (false);
		good.SetActive (true);
		miChangeTime = DateTime.UtcNow;
	}
	public void mi_bad () {
		audioSource.PlayOneShot (badSE);
		good.SetActive (false);
		mi.SetActive (false);
		bad.SetActive (true);
		miChangeTime = DateTime.UtcNow;
	}

	public void CreateCircle(int num) {
		GameObject circles = (GameObject)Instantiate (circle);
		circles.transform.SetParent (canvasGame.transform, false);
		float x = UnityEngine.Random.Range (-350.0f, 350.0f);
		circles.transform.localPosition = new Vector3 (x, 590.0f, 0f);
		String str = num.ToString();
		circles.transform.FindChild ("Text").gameObject.GetComponent<Text>().text = str;
	}

	public void CreateNewCircle() {
		lastDateTime = DateTime.UtcNow;
		if (currentCircle >= MAX_CIRCLE) return;
		CreateCircle (UnityEngine.Random.Range(2, 99));
		currentCircle++;
	}

	public void TouchCircle() {
		currentCircle--;
	}

	public void damaged() {
		// ばつ表示
		mi_bad ();
		// ダメージを受けた時の処理
		currentLife--;
		damageLife++;
		GameObject g_dislife = (GameObject)Instantiate (dislife);
		g_dislife.transform.SetParent (canvasGame.transform, false);
		int x = 300 - (150 * (damageLife-1));
		// -300, -150, 0, 150, 300	
		g_dislife.transform.localPosition = new Vector3 (x, -570.0f, 0f);
		if (currentLife <= 0) {
			// GAME OVER
			GameOver ();
			return;
		}
	}
	public void getScore (int num) {
		score += num;
		updateScoretext ();
		getPrimeList.Add (num);
		getPrimeList.Sort ();
	}

	void updateScoretext () {
		scoreText.GetComponent<Text> ().text = "Score : " + score;
	}

	public void OnClick_openMenu () {
		if (menuCheck()) return;
		if (!gameCheck()) return;
		audioSource.PlayOneShot (buttonSE);
		Debug.Log ("openMenu");
		menu.SetActive (true);
		menu.transform.SetAsLastSibling();	// 最前面に
	}
	public void OnClick_closeMenu () {
		audioSource.PlayOneShot (buttonSE);
		Debug.Log ("closeMenu");
		menu.SetActive (false);
	}
	public void OnClick_whatPrime () {

	}
	public void OnClick_backTitle () {
		audioSource.PlayOneShot (buttonSE);
		Debug.Log ("Back to Title.");
		SceneManager.LoadScene ("StartScene");
	}
	private void GameOver () {
		gamePlay = false;
		gameover.SetActive (true);
		gameover.transform.SetAsLastSibling ();
		backTitle.SetActive (true);
		backTitle.transform.SetAsLastSibling ();
	}
}
