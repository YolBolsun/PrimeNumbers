using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EatNumber : MonoBehaviour {
	

	public bool numberMatchesLevel;
	public int number;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public bool eatNumber()
	{
		if (numberMatchesLevel) {
			GameObject gameController = GameObject.FindGameObjectWithTag ("GameController");
			gameController.GetComponent<LevelManager> ().numbersOnBoard.Remove (this.gameObject);
			Destroy (this.gameObject);
			Text textValueObject = GameObject.FindGameObjectWithTag ("ScoreValue").GetComponent<Text> ();
			textValueObject.text = (int.Parse (textValueObject.text) + this.number).ToString ();
			gameController.GetComponent<LevelManager> ().AnalyzeBoard ();
		} else {
			GameObject.FindGameObjectWithTag ("GameController").GetComponent<LevelManager>().PlayerDeath();
		}
		return numberMatchesLevel;
	}
}
