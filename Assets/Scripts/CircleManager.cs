using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleManager : MonoBehaviour {
	// 定数
	public int[] primeNumber = new int[] {2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97};
	// オブジェクト参照
	private GameObject gameManager;
	private Renderer rend;
	private Color color;
	private float red, green, blue, alfa;
	private bool isFade;
	private int num;

	// Use this for initialization
	void Start () {
		gameManager = GameObject.Find ("GameManager");
		rend = this.GetComponent<Renderer> ();
		red = this.GetComponent<Image> ().color.r;
		green = this.GetComponent<Image> ().color.g;
		blue = this.GetComponent<Image> ().color.b;
		alfa = 1.0f; // 1 == 透明。　0 == 不透明
		isFade = false;
		string getText = this.gameObject.transform.FindChild ("Text").gameObject.GetComponent<Text>().text;
		// Debug.Log (getText);
		num = int.Parse(getText);
	}
	
	// Update is called once per frame
	void Update () {
		// 素数をタップした時にFade outする処理
		if (isFade) {
	        GetComponent<Image>().color = new Color(red, green, blue, alfa);
    	    alfa -= 0.15f;
			// Debug.Log(alfa);
			if (alfa <= 0) {
				Destroy (this.gameObject);
			}
			return;
		}
		bool checkGame = gameManager.GetComponent<GameManager> ().gameCheck ();
		bool checkMenu = gameManager.GetComponent<GameManager> ().menuCheck ();
		if (!checkGame) return;
		if (checkMenu) return;
		// circleのy値を変更(平行移動)
		Vector3 lp = this.gameObject.transform.localPosition;
		float x = lp.x;
		if (x < -400 || x > 400) {
			x = System.Math.Abs (x);
		}
		float y = lp.y - 5;
		this.gameObject.transform.localPosition = new Vector2(x, y);
		// ボーダーラインを超えたらデストロイ
		// ただし、それが素数ならcongraturation.
		if (y < -500.0) {
			// prime serch
			for (int i = 0; i < primeNumber.Length; i++) {
				if (num == primeNumber [i]) {
					// ボーダーを超えたものが素数だった時の処理
					// 獲得した素数リストに追加　＆　星マークでも出すか？　SEは必要だよね。　嬉しい感じのやつ。
					// まる表示
					gameManager.GetComponent<GameManager> ().mi_good ();
					gameManager.GetComponent<GameManager> ().getScore (num);
					Destroy (this.gameObject);
					return;
				} 
			}
			// ボーダー超えちゃったけど、素数じゃなかったやーつ。
			// ライフ減少。
			gameManager.GetComponent<GameManager> ().damaged ();
			Destroy (this.gameObject);
		}
	}

	public void TouchCircle() {
		bool checkGame = gameManager.GetComponent<GameManager> ().gameCheck ();
		if (!checkGame) return;
		if (Input.GetMouseButton (0) == false) return;
		gameManager.GetComponent<GameManager> ().TouchCircle ();
		// prime serch
		for (int i = 0; i < primeNumber.Length; i++) {
			if (num == primeNumber [i]) {
				// 残像を残して終了する。
				// 本来選んじゃダメである。
				// ライフ-1
				isFade = true;
				gameManager.GetComponent<GameManager> ().damaged ();
				return;
			}
		}
		if (num > 3) {
			int mid = UnityEngine.Random.Range (2, num-2);
			gameManager.GetComponent<GameManager> ().CreateCircle (mid);
			gameManager.GetComponent<GameManager> ().CreateCircle (num - mid);
		}
		Destroy (this.gameObject);
	}
}
