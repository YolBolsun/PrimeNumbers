using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

	public int width = 8;
	public int height = 6;
	public int difficulty = 1; //number of primes to jump
	public GameObject gamePlane;
	public List<GameObject> numbersOnBoard = new List<GameObject> ();
	private int currentLevel = 0;
	public HashSet<int> primes = new HashSet<int> (); 
	public int maxPrime;
	//private int[,] gameBoard;
	public List<GameObject> enemyPrefab;
	public float timeBetweenEnemySpawn;
	float timeOfLastSpawn = 0f;
	public Transform spawnLoc;

	public List<GameObject> extraLives;
	public const int maxLives = 3;
	public int numLives = maxLives;

	public GameObject numberPrefab;

	// Use this for initialization
	void Start () {
		if (difficulty > 5) {
			difficulty = 5;
		}
		primes.Add(2);
		primes.Add(3);
		primes.Add(5);
		primes.Add(7);
		primes.Add(11);
		primes.Add(13);
		primes.Add(17);
		/*primes.Add(19);
		primes.Add(23);
		primes.Add(29);
		primes.Add(31);
		primes.Add(37);
		primes.Add(41);*/
		maxPrime = 17;
		PopulateBoard ();
		//gameBoard = new int[width,height];

	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > timeOfLastSpawn + timeBetweenEnemySpawn) {
			SpawnEnemy ();
		}
		
	}
	void PopulateBoard()
	{
		Debug.Log ("Ensure appropriate number of primes spawn");
		for (float i = 0; i < height; i++) {
			for (float j = 0; j < width; j++) {
				//gameBoard [i, j] = i + j;
				GameObject temp = GameObject.Instantiate (numberPrefab, new Vector3 (0f, 0f, 0f), Quaternion.identity);
				temp.transform.position = new Vector3 (i-3.5f, j-2.5f, 0f);

				int number = (int)Mathf.Floor(Random.Range (0f, (float)maxPrime))+1;

				if (primes.Contains (number)) {
					temp.GetComponent<EatNumber> ().numberMatchesLevel = true;
				}
				temp.GetComponent<EatNumber> ().number = number;
				temp.GetComponent<TextMesh> ().text = (number).ToString();
				numbersOnBoard.Add (temp);

			}
		}
	}
	void ClearBoard()
	{
		while (numbersOnBoard.Count > 0) {
			GameObject numToRemove = numbersOnBoard [0];
			numbersOnBoard.RemoveAt (0);
			GameObject.Destroy (numToRemove);
		}
	}
	public void AnalyzeBoard()
	{
		foreach (GameObject num in numbersOnBoard) {
			if (num.GetComponent<EatNumber> ().numberMatchesLevel) {
				return;
			}
		}
		Reset (true);

	}
	void Reset(bool nextLevel)
	{
		ClearEnemies ();
		if (nextLevel) {
			SetNextPrime ();
			ClearBoard ();
			PopulateBoard ();
		} else {
			primes.Clear ();
			ClearBoard ();
			foreach(GameObject go in extraLives)
			{
				go.SetActive (true);
			}
			numLives = maxLives;
			GameObject.FindGameObjectWithTag ("ScoreValue").GetComponent<Text> ().text = "0";
			Start ();
		}

	}
	void SetNextPrime()
	{
		int numPrimesToAdd = difficulty;
		while (true) {
			maxPrime += 2;
			bool isPrime = true;
			foreach (int prime in primes) {
				float temp = ((float)maxPrime) / ((float)prime);
				//Debug.Log(maxPrime.ToString() + "  " + prime.ToString() + "  " + temp.ToString());
				if (temp - Mathf.Floor (temp) == 0f) {
					isPrime = false;
					break;
				}
			}
			if (!isPrime) {
				continue;
			}
			primes.Add (maxPrime);
			if (numPrimesToAdd > 0) {
				numPrimesToAdd--;
			} else {
				return;
			}
		}
	}

	public void PlayerDeath()
	{
		Debug.Log ("Player Death");
		if (numLives == 0) {
			Reset (false);
			return;
		}
		numLives--;
		extraLives [numLives].gameObject.SetActive (false);
		ClearEnemies ();
	}

	void ClearEnemies()
	{
		GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy");
		for(int i = 0; i < enemies.Length; i++) {
			GameObject.Destroy (enemies[i]);
		}
		Debug.Log ("Clear Enemies");
	}
	void SpawnEnemy()
	{
		timeOfLastSpawn = Time.time;
		Debug.Log ("Spawn Enemy");
		GameObject go = GameObject.Instantiate(enemyPrefab[0], spawnLoc.position,Quaternion.identity);
		/*
		 * Spawn enemy game object
		 * enemy game object movement controlled
		 * -3.5,2.5
		 * */
	}
}
