using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class playerMovement : MonoBehaviour {

	public GameObject player;
	public GameObject gameBoard;

	public Vector3 desiredLocation;
	public float speed=2;

	//6x5 (wider) board
	//horizontal movement first followed by vertical
	//own a classroom
	//stats can be classroom based
	//device doesn't matter too much but phone/tablet prioritized then other stuff
	//Enemies that change numbers/differ in where and how they move

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		desiredLocation = player.transform.position;
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			RaycastHit hit;

			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100)) {
				Vector3 destination = hit.point;
				destination.x = (float)System.Math.Floor (destination.x) + .5f;
				destination.y = (float)System.Math.Floor (destination.y) + .5f;
				destination.z = 0f;
				//player.transform.position = destination;
				desiredLocation = destination;
				GameObject number = hit.collider.gameObject;
				if (number.tag == "Number") {
					number.GetComponent<EatNumber> ().eatNumber ();
				}
			}
		}
		if (desiredLocation.x != player.transform.position.x) {
			Vector3 movement = new Vector3 (desiredLocation.x - player.transform.position.x, 0f, 0f).normalized * speed * Time.deltaTime;
			if (movement.x > Mathf.Abs(desiredLocation.x - player.transform.position.x)) {
				movement.x *= ((desiredLocation.x - player.transform.position.x) / movement.x);
			}
			player.transform.Translate (movement);

		} else if (desiredLocation.y != player.transform.position.y) {
			Vector3 movement = new Vector3 (0f, desiredLocation.y - player.transform.position.y, 0f).normalized * speed * Time.deltaTime;
			if (movement.y >  Mathf.Abs(desiredLocation.y - player.transform.position.y)) {
				movement.y *= ((desiredLocation.y - player.transform.position.y) / movement.y);
			}
			player.transform.Translate (movement);

		}
	}
}
